using Laborator2.Domain.Models;
using System;
using static Laborator2.Domain.Models.CartStates;
using static Laborator2.Domain.Models.PaidCartEvent;
using static Laborator2.Domain.CartPriceOperation;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using Laborator2.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;

namespace Laborator2.Domain
{
    public class PaidCartWorkflow
    {
        private readonly IProductsRepository productsRepository;
        private readonly IOrderLinesRepository orderLinesRepository;
        private readonly IOrderHeadersRepository orderHeadersRepository;
        private readonly ILogger<PaidCartWorkflow> logger;

        public PaidCartWorkflow(IProductsRepository productsRepository, IOrderLinesRepository orderLinesRepository, IOrderHeadersRepository orderHeadersRepository, ILogger<PaidCartWorkflow> logger)
        {
            this.productsRepository = productsRepository;
            this.orderLinesRepository = orderLinesRepository;
            this.orderHeadersRepository = orderHeadersRepository;
            this.logger = logger;
        }
        public async Task<IPaidCartEvent> ExecuteAsync(PayCartCommand command)
        {
            UnvalidatedCartState unvalidatedCart = new UnvalidatedCartState(command.InputCart);
            var result = from products in productsRepository.TryGetExistingProduct(unvalidatedCart.ProductsList.Select(p => p.ProductCode))
                                                            .ToEither(ex => new FailedCartState(unvalidatedCart.ProductsList, ex) as ICartStates)
                         from existingOrderLines in orderLinesRepository.TryGetExistingOrderLine()
                                          .ToEither(ex => new FailedCartState(unvalidatedCart.ProductsList, ex) as ICartStates)
                         let checkProductExists = (Func<ProductCode, Option<ProductCode>>)(product => CheckProductExists(products, product))
                         from paidCartState in ExecuteWorkflowAsync(unvalidatedCart, existingOrderLines, checkProductExists).ToAsync()
                         from _ in orderLinesRepository.TrySaveOrderLinePrice(paidCartState)
                                          .ToEither(ex => new FailedCartState(unvalidatedCart.ProductsList, ex) as ICartStates)
                         select paidCartState;

            return await result.Match(
                    Left: cartState => GenerateFailedEvent(cartState) as IPaidCartEvent,
                    Right: paidCart => new PaidCartSuccedeedEvent(paidCart.Csv)
                );
        }
        private async Task<Either<ICartStates, PaidCartState>> ExecuteWorkflowAsync(UnvalidatedCartState unvalidatedCart,
                                                                                    IEnumerable<CalculatedFinalPrice> existingOrderLines,
                                                                                    Func<ProductCode, Option<ProductCode>> checkProductExists)
        {

            ICartStates cartState = await ValidateProductCarts(checkProductExists, unvalidatedCart);
            cartState = CalculateFinalCartPrice(cartState);
            cartState = MergeOrders(cartState, existingOrderLines);
            cartState = PayFinalCartPrice(cartState);

            return cartState.Match<Either<ICartStates, PaidCartState>>(
                whenEmptyCartState: emptyCart => Left(emptyCart as ICartStates),
                whenUnvalidatedCartState: unvalidatedCart => Left(unvalidatedCart as ICartStates),
                whenInvalidatedCartState: invalidatedCart => Left(invalidatedCart as ICartStates),
                whenValidatedCartState: validatedCart => Left(validatedCart as ICartStates),
                whenFailedCartState: failedCart => Left(failedCart as ICartStates),
                whenCalculatedCartState: calculatedCart => Left(calculatedCart as ICartStates),
                whenPaidCartState: paidCart => Right(paidCart)
            );
        }
        private Option<ProductCode> CheckProductExists(IEnumerable<ProductCode> products, ProductCode productCode)
        {
            if (products.Any(p => p == productCode))
            {
                return Some(productCode);
            }
            else
            {
                return None;
            }
        }
        private PaidCartFailedEvent GenerateFailedEvent(ICartStates cartState) =>
            cartState.Match<PaidCartFailedEvent>(
                whenEmptyCartState: emptyCart => new($"Invalid state {nameof(EmptyCartState)}"),
                whenUnvalidatedCartState: unvalidatedCart => new($"Invalid state {nameof(UnvalidatedCartState)}"),
                whenInvalidatedCartState: invalidatedCart => new(invalidatedCart.Reason),
                whenValidatedCartState: validatedCart => new($"Invalid state {nameof(ValidatedCartState)}"),
                whenFailedCartState: failedCart =>
                {
                    logger.LogError(failedCart.Exception, failedCart.Exception.Message);
                    return new(failedCart.Exception.Message);
                },
                whenCalculatedCartState: calculatedCart => new($"Invalid state {nameof(CalculatedCartState)}"),
                whenPaidCartState: paidCart => new($"Invalid state {nameof(PaidCartState)}"));
    }





    // public async Task<IPaidCartEvent> ExecuteAsync(PayCartCommand command, Func<ProductCode, Option<ProductCode>> checkProductExists, Func<ProductCode, ProductQuantity, TryAsync<bool>> checkStock, Func<Client, TryAsync<bool>> checkAddress)
    //     {
    //         UnvalidatedCartState unvalidatedCart = new UnvalidatedCartState(command.InputCart);
    //         ICartStates cart = await ValidateProductCarts(checkProductExists, unvalidatedCart);
    //         cart = CalculateFinalCartPrice(cart);
    //         cart = PayFinalCartPrice(cart);

    //         return cart.Match(
    //             whenEmptyCartState: emptyCart => new PaidCartFailedEvent("Cart still empty"),
    //             whenUnvalidatedCartState: unvalidatedCart => new PaidCartFailedEvent("Unexpected unvalidated state") as IPaidCartEvent,
    //             whenInvalidatedCartState: invalidatedCart => new PaidCartFailedEvent(invalidatedCart.Reason),
    //             whenFailedCartState: failedCartState => new PaidCartFailedEvent("Failed"),
    //             whenValidatedCartState: validatedCart => new PaidCartFailedEvent("Unexpected validated state"),
    //             whenCalculatedCartState: calculatedCart => new PaidCartFailedEvent("Unexpected calculated state"),
    //             whenPaidCartState: paidCart => new PaidCartSuccedeedEvent(paidCart.Csv)
    //             );
    //     }
}
