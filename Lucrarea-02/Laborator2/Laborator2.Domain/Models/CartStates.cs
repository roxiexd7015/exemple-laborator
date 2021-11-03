using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator2.Domain.Models
{
    [AsChoice]

    public static partial class CartStates
    {
        public interface ICartStates { }
        public record EmptyCartState : ICartStates
        {

        }
        public record UnvalidatedCartState : ICartStates
        {
            public UnvalidatedCartState(IReadOnlyCollection<UnvalidatedCart> productsList)
            {
                ProductsList = productsList;
            }
            public IReadOnlyCollection<UnvalidatedCart> ProductsList { get; }
        }
        public record InvalidatedCartState : ICartStates
        {
            internal InvalidatedCartState(IReadOnlyCollection<UnvalidatedCart> productsList, string reason)
            {
                ProductsList = productsList;
                Reason = reason;
            }

            public IReadOnlyCollection<UnvalidatedCart> ProductsList { get; }
            public string Reason { get; }
        }

        public record ValidatedCartState : ICartStates
        {
            internal ValidatedCartState(IReadOnlyCollection<ValidatedCart> productsList)
            {
                ProductsList = productsList;
            }

            public IReadOnlyCollection<ValidatedCart> ProductsList { get; }
        }
        public record CalculatedCartState : ICartStates
        {
            internal CalculatedCartState(IReadOnlyCollection<CalculatedFinalPrice> productsList)
            {
                ProductsList = productsList;
            }

            public IReadOnlyCollection<CalculatedFinalPrice> ProductsList { get; }
        }

        public record PaidCartState : ICartStates
        {
            internal PaidCartState(IReadOnlyCollection<CalculatedFinalPrice> productsList, string csv)
            {
                ProductsList = productsList;
                Csv = csv;
            }

            public IReadOnlyCollection<CalculatedFinalPrice> ProductsList { get; }
            public string Csv { get; }
        }

    }
}

