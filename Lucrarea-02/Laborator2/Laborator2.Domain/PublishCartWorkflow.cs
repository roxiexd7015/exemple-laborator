using Laborator2.Domain.Models;
using static Laborator2.Domain.Models.CartPublishedEvent;
using static Laborator2.Domain.ProductsPriceOperation;
using System;
using static Laborator2.Domain.Models.CartStates;

namespace Laborator2.Domain
{
    public class PublishCartWorkflow
    {
        public ICartPublishedEvent Execute(PublishCartCommand command, Func<Client, bool> checkClientExists)
        {
            UnvalidatedCart unvalidatedCart = new UnvalidatedCart(command.InputCart);
            ICartStates cart = ValidateProductsPrice(checkClientExists, unvalidatedCart);
            cart = CalculateFinalCart(cart);
            cart = PublishCart(cart);

            return cart.Match(
                    whenUnvalidatedCart: unvalidatedCart => new CartPublishFailedEvent("Unexpected unvalidated state") as ICartPublishedEvent,
                    whenInvalidatedCart: invalidCart => new CartPublishFailedEvent(invalidCart.Reason),
                    whenValidatedCart: validatedCart => new CartPublishFailedEvent("Unexpected validated state"),
                    whenCalculatedCart: calculatedCart => new CartPublishFailedEvent("Unexpected calculated state"),
                    whenPublishedCart: publishedCart => new CartPublishSucceededEvent(publishedCart.Csv)
                );
        }
    }
}
