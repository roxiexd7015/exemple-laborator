using Laborator2.Domain.Models;
using System;
using static Laborator2.Domain.Models.CartStates;
using static Laborator2.Domain.Models.PaidCartEvent;
using static Laborator2.Domain.CartPriceOperation;

namespace Laborator2.Domain
{
    public class PaidCartWorkflow
    {
        public IPaidCartEvent Execute(PayCartCommand command, Func<ProductCode, bool> checkProductExists)
        {
            UnvalidatedCartState unvalidatedCart = new UnvalidatedCartState(command.InputCart);
            ICartStates cart = ValidateProductCart(checkProductExists, unvalidatedCart);
            cart = CalculateFinalCartPrice(cart);
            cart = PayFinalCartPrice(cart);

            return cart.Match(
                whenEmptyCartState: emptyCart => new PaidCartFailedEvent("Cart still empty"),
                whenUnvalidatedCartState: unvalidatedCart => new PaidCartFailedEvent("Unexpected unvalidated state") as IPaidCartEvent,
                whenInvalidatedCartState: invalidatedCart => new PaidCartFailedEvent("Unexpected invalidated state: " + invalidatedCart.Reason),
                whenValidatedCartState: validatedCart => new PaidCartFailedEvent("Unexpected validated state"),
                whenCalculatedCartState: calculatedCart => new PaidCartFailedEvent("Unexpected calculated state"),
                whenPaidCartState: paidCart => new PaidCartSuccedeedEvent(paidCart.Csv)
                );
        }
    }
}
