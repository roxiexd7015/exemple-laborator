using Laborator2.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Laborator2.Domain.Models.CartStates;

namespace Laborator2.Domain
{
    public static class CartPriceOperation
    {
        public static ICartStates ValidateProductCart(Func<ProductCode, bool> checkProductExists, UnvalidatedCartState cart)
        {
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
                    invalidReason = $"Invalid product price ({unvalidatedProduct.ProductCode}, {unvalidatedProduct.ProductPrice})";
                    isValidList = false;
                    break;
                }
                if (!ProductQuantity.TryParseQuantity(unvalidatedProduct.ProductQuantity, out ProductQuantity quantity)
                    && checkProductExists(code))
                {
                    invalidReason = $"Invalid product quantity ({unvalidatedProduct.ProductCode}, {unvalidatedProduct.ProductQuantity})";
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

        }

        public static ICartStates CalculateFinalCartPrice(ICartStates cart) => cart.Match(
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
                                                                     validCart.quantity));
                return new CalculatedCartState(calculatedList.ToList().AsReadOnly());
            }
        );

        public static ICartStates PayFinalCartPrice(ICartStates cart) => cart.Match(
            whenEmptyCartState: emptyCart => emptyCart,
            whenUnvalidatedCartState: unvalidatedCart => unvalidatedCart,
            whenInvalidatedCartState: invalidCart => invalidCart,
            whenValidatedCartState: validatedCart => validatedCart,
            whenPaidCartState: paidCart => paidCart,
            whenCalculatedCartState: calculatedCart =>
            {
                StringBuilder csv = new();
                calculatedCart.ProductsList.Aggregate(csv, (export, list) => export.AppendLine($"{list.code.Value}, {list.price.Value}, {list.quantity.Value}"));

                PaidCartState paidCartState = new(calculatedCart.ProductsList, csv.ToString());

                return paidCartState;
            });
    }
}
