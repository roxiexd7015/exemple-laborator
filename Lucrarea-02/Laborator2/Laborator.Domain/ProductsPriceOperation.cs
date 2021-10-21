using Laborator2.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Laborator2.Domain.Models.CartStates;

namespace Laborator2.Domain
{
    public static class ProductsPriceOperation
    {
        public static ICartStates ValidateProductsPrice(Func<Client, bool> checkClientExist, UnvalidatedCart cart)
        {
            List<ValidateCart> validatedCart = new();
            bool isValidList = true;
            string invalidReson = string.Empty;
            foreach (var unvalidatedCart in cart.ProductsList)
            {
                if (!Price.TryParsePrice(unvalidatedCart.CartStates, out Client client))
                {
                    invalidReason = $"Invalid cart ({unvalidatedCart.Client}, {unvalidatedCart.CartStates})";
                    isValidList = false;
                    break;
                }
                if (!Price.TryParsePrice(unvalidatedCart.ActivityCart, out Client activityClient))
                {
                    invalidReason = $"Invalid activity client ({unvalidatedCart.Client}, {unvalidatedCart.ActivityCart})";
                    isValidList = false;
                    break;
                }
                if (!Client.TryParse(unvalidatedCart.Client, out Client client)
                    && checkClientExists(client))
                {
                    invalidReason = $"Invalid client name ({unvalidatedCart.Client})";
                    isValidList = false;
                    break;
                }
                ValidateCart validCart = new(client, cart, activityClient);
                validatedCart.Add(validCart);
            }

            if (isValidList)
            {
                return new ValidatedCart(validatedCart);
            }
            else
            {
                return new InvalidatedCart(cart.ProductsList, invalidReason);
            }

        }

        public static ICartStates CalculateFinalCart(ICartStates cart) => cart.Match(
            whenUnvalidatedCart: unvalidatedCart => unvalidatedCart,
            whenInvalidatedCart: invalidCart => invalidCart,
            whenCalculatedCart: calculatedCart => calculatedCart,
            whenPublishedCart: publishedCart => publishedCart,
            whenValidatedCart: validCart =>
            {
                var calculatedCart = validCart.ProductsList.Select(validCart =>
                                            new CalculatedFinalCart(validCart.Client,
                                                                      validCart.CartStates,
                                                                      validCart.ActivityClient,
                                                                      validCart.CartStates + validCart.ActivityClient));
                return new CalculatedCart(calculatedCart.ToList().AsReadOnly());
            }
        );

        public static ICartStates PublishCart(ICartStates cart) => cart.Match(
            whenUnvalidatedCart: unvalidatedCart => unvalidatedCart,
            whenInvalidatedCart: invalidCart => invalidCart,
            whenValidatedCart: validatedCart => validatedCart,
            whenPublishedCart: publishedCart => publishedCart,
            whenCalculatedCart: calculatedCart =>
            {
                StringBuilder csv = new();
                calculatedCart.ProductsList.Aggregate(csv, (export, client) => export.AppendLine($"{client.Client.Value}, {client.Cart}, {client.ActivityClient}"));

                PublishedCart publishedCart = new(calculatedCart.ProductsList, csv.ToString());

                return publishedCart;
            });
    }
}
