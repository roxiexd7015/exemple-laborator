using Laborator2.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LanguageExt.Prelude;
using LanguageExt;
using static Laborator2.Domain.Models.CartStates;

namespace Laborator2.Domain
{
    public static class CartPriceOperation
    {
        public static Task<ICartStates> ValidateProductCarts(Func<ProductCode, Option<ProductCode>> checkProductExists, UnvalidatedCartState cart) =>
            cart.ProductsList
                    .Select(ValidateProductCart(checkProductExists))
                    .Aggregate(CreateEmptyValidatedProductList().ToAsync(), ReduceValidProduct)
                    .MatchAsync(
                        Right: validatedCart => new ValidatedCartState(validatedCart),
                        LeftAsync: errorMessage => Task.FromResult((ICartStates)new InvalidatedCartState(cart.ProductsList, errorMessage))
                    );
        private static Func<UnvalidatedCart, EitherAsync<string, ValidatedCart>> ValidateProductCart(Func<ProductCode, Option<ProductCode>> checkProductExists) =>
            unvalidatedProduct => ValidateProductCart(checkProductExists, unvalidatedProduct);
        private static EitherAsync<string, ValidatedCart> ValidateProductCart(Func<ProductCode, Option<ProductCode>> checkProductExists, UnvalidatedCart unvalidatedProduct) =>
            from productCode in ProductCode.TryParseCode(unvalidatedProduct.ProductCode)
                                   .ToEitherAsync($"Invalid product code ({unvalidatedProduct.ProductCode})")
            from productPrice in ProductPrice.TryParsePrice(unvalidatedProduct.ProductPrice)
                                   .ToEitherAsync(() => $"Invalid product price ({unvalidatedProduct.ProductPrice})")
            from productQuantity in ProductQuantity.TryParseQuantity(unvalidatedProduct.ProductQuantity)
                                   .ToEitherAsync(() => $"Invalid product quantity ({unvalidatedProduct.ProductQuantity})")
            from clientAddress in Client.TryParseAddress(unvalidatedProduct.ClientAddress)
                                   .ToEitherAsync(() => $"Invalid client address ({unvalidatedProduct.ClientAddress})")
            from productExists in checkProductExists(productCode)
                                   .ToEitherAsync(() => $"Product {productCode.Value} does not exist.")
            select new ValidatedCart(productCode, productPrice, productQuantity, clientAddress);
        private static Either<string, List<ValidatedCart>> CreateEmptyValidatedProductList() =>
            Right(new List<ValidatedCart>());
        private static EitherAsync<string, List<ValidatedCart>> ReduceValidProduct(EitherAsync<string, List<ValidatedCart>> acc, EitherAsync<string, ValidatedCart> next) =>
            from list in acc
            from nextProduct in next
            select list.AppendValidProduct(nextProduct);
        private static List<ValidatedCart> AppendValidProduct(this List<ValidatedCart> list, ValidatedCart validProduct)
        {
            list.Add(validProduct);
            return list;
        }
        public static ICartStates CalculateFinalCartPrice(ICartStates cart) => cart.Match(
           whenEmptyCartState: emptyCart => emptyCart,
           whenUnvalidatedCartState: unvalidatedCart => unvalidatedCart,
           whenInvalidatedCartState: invalidCart => invalidCart,
           whenFailedCartState: failedCart => failedCart,
           whenCalculatedCartState: calculatedCart => calculatedCart,
           whenPaidCartState: paidCart => paidCart,
           whenValidatedCartState: CalculateFinalProductPrice
       );
        private static ICartStates CalculateFinalProductPrice(ValidatedCartState validCart) =>
            new CalculatedCartState(validCart.ProductsList
                                             .Select(CalculateFinalCart)
                                             .ToList()
                                             .AsReadOnly());

        private static CalculatedFinalPrice CalculateFinalCart(ValidatedCart validCart) =>
            new CalculatedFinalPrice(validCart.ProductCode,
                                     validCart.ProductPrice,
                                     validCart.ProductQuantity,
                                     validCart.ProductPrice * validCart.ProductQuantity);
        public static ICartStates MergeOrders(ICartStates cart, IEnumerable<CalculatedFinalPrice> existingOrder) => cart.Match(
            whenEmptyCartState: emptyCart => emptyCart,
            whenUnvalidatedCartState: unvalidatedCart => unvalidatedCart,
            whenInvalidatedCartState: invalidCart => invalidCart,
            whenFailedCartState: failedCart => failedCart,
            whenValidatedCartState: validCart => validCart,
            whenPaidCartState: paidCart => paidCart,
            whenCalculatedCartState: calculatedCart => MergeOrders(calculatedCart.ProductsList, existingOrder)); //uhm, aici e ceva gresit..
        private static CalculatedCartState MergeOrders(IEnumerable<CalculatedFinalPrice> newList, IEnumerable<CalculatedFinalPrice> existingList)
        {
            var updatedAndNewCarts = newList.Select(o => o with { OrderId = existingList.FirstOrDefault(p => p.ProductCode == o.ProductCode)?.OrderId ?? 0, IsUpdated = true });
            var oldCarts = existingList.Where(o => !newList.Any(p => p.ProductCode == o.ProductCode));
            var allCarts = updatedAndNewCarts.Union(oldCarts)
                                                .ToList()
                                                .AsReadOnly();
            return new CalculatedCartState(allCarts);
        }
        public static ICartStates PayFinalCartPrice(ICartStates cart) => cart.Match(
            whenEmptyCartState: emptyCart => emptyCart,
            whenUnvalidatedCartState: unvalidatedCart => unvalidatedCart,
            whenInvalidatedCartState: invalidCart => invalidCart,
            whenFailedCartState: failedCart => failedCart,
            whenValidatedCartState: validatedCart => validatedCart,
            whenPaidCartState: paidCart => paidCart,
            whenCalculatedCartState: GenerateExport);
        private static ICartStates GenerateExport(CalculatedCartState calculatedCart) =>
            new PaidCartState(calculatedCart.ProductsList,
                              calculatedCart.ProductsList.Aggregate(new StringBuilder(), CreateCsvLine).ToString());
        private static StringBuilder CreateCsvLine(StringBuilder export, CalculatedFinalPrice list) =>
            export.AppendLine($"{list.ProductCode.Value}, {list.ProductPrice.Value}, {list.ProductQuantity.Value}, {list.TotalPrice.Value}");







        /*public static ICartStates PayFinalCartPrice(ICartStates cart) => cart.Match(
            whenEmptyCartState: emptyCart => emptyCart,
            whenUnvalidatedCartState: unvalidatedCart => unvalidatedCart,
            whenInvalidatedCartState: invalidCart => invalidCart,
            whenValidatedCartState: validatedCart => validatedCart,
            whenPaidCartState: paidCart => paidCart,
            whenCalculatedCartState: calculatedCart =>
            {
                StringBuilder csv = new();
                calculatedCart.ProductsList.Aggregate(csv, (export, list) => export.AppendLine($"Product: ID: {list.code.Value}, price: {list.price.Value}, quantity: {list.quantity.Value}. TOTAL price: {list.totalPrice.Value}"));

                PaidCartState paidCartState = new(calculatedCart.ProductsList, csv.ToString());

                return paidCartState;
            });
        */
        /*{
               List<ValidatedCart> validatedCart = new();
               bool isValidList = true;
               string invalidReason = string.Empty;
               foreach (var unvalidatedProduct in cart.ProductsList)
               {
                   if (!ProductCode.TryParseCode(unvalidatedProduct.ProductCode, out ProductCode code))
                   {
                       invalidReason = $"Invalid product code ({unvalidatedProduct.ProductCode})";
                       isValidList = false;
                       break;
                   }
                   if (!ProductPrice.TryParsePrice(unvalidatedProduct.ProductPrice, out ProductPrice price))
                   {
                       invalidReason = $"Invalid product price ({unvalidatedProduct.ProductPrice})";
                       isValidList = false;
                       break;
                   }
                   if (!ProductQuantity.TryParseQuantity(unvalidatedProduct.ProductQuantity, out ProductQuantity quantity)
                       && checkProductExists(code))
                   {
                       invalidReason = $"Invalid product quantity ({unvalidatedProduct.ProductQuantity})";
                       isValidList = false;
                       break;
                   }
                   ValidatedCart validCart = new(code, price, quantity);
                   validatedCart.Add(validCart);
               }

               if (isValidList)
               {
                   return new ValidatedCartState(validatedCart);
               }
               else
               {
                   return new InvalidatedCartState(cart.ProductsList, invalidReason);
               }

           }*/

        /* public static ICartStates CalculateFinalCartPrice(ICartStates cart) => cart.Match(
             whenEmptyCartState: emptyCart => emptyCart,
             whenUnvalidatedCartState: unvalidatedCart => unvalidatedCart,
             whenInvalidatedCartState: invalidCart => invalidCart,
             whenCalculatedCartState: calculatedCart => calculatedCart,
             whenPaidCartState: paidCart => paidCart,
             whenValidatedCartState: validatedCart =>
             {
                 var calculatedList = validatedCart.ProductsList.Select(validCart =>
                                             new CalculatedFinalPrice(validCart.code,
                                                                      validCart.price,
                                                                      validCart.quantity,
                                                                      validCart.price * validCart.quantity));
                 return new CalculatedCartState(calculatedList.ToList().AsReadOnly());
             }
         );*/
    }
}
