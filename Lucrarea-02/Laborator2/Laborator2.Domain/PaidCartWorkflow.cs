﻿using Laborator2.Domain.Models;
using System;
using static Laborator2.Domain.Models.CartStates;
using static Laborator2.Domain.Models.PaidCartEvent;
using static Laborator2.Domain.CartPriceOperation;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Laborator2.Domain
{
    public class PaidCartWorkflow
    {
        public async Task<IPaidCartEvent> ExecuteAsync(PayCartCommand command, Func<ProductCode, TryAsync<bool>> checkProductExists)
        {
            UnvalidatedCartState unvalidatedCart = new UnvalidatedCartState(command.InputCart);
            ICartStates cart = await ValidateProductCart(checkProductExists, unvalidatedCart);
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
