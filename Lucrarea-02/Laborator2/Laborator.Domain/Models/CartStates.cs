using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator1.Domain
{
    [AsChoice]

    public static partial class CartStates
    {
        public interface ICartStates { }
        public record UnvalidatedCart : ICartStates
        {
            public UnvalidatedCart(IReadOnlyCollection<UnvalidateCart> productsList)
            {
                ProductsList = productsList;
            }
            public IReadOnlyCollection<UnvalidateCart> ProductsList { get; }
        }

        public record InvalidatedCart : ICartStates
        {
            internal InvalidatedCart(IReadOnlyCollection<UnvalidateCart> productsList, string reason)
            {
                ProductsList = productsList;
                Reason = reason;
            }

            public IReadOnlyCollection<UnvalidateCart> GradeList { get; }
            public string Reason { get; }
        }

        public record ValidatedCart : ICartStates
        {
            internal ValidatedCart(IReadOnlyCollection<ValidateCart> productsList)
            {
                ProductsList = productsList;
            }

            public IReadOnlyCollection<ValidateCart> ProductsList { get; }
        }
        public record CalculatedCart : ICartStates
        {
            internal CalculatedCart(IReadOnlyCollection<CalculatedFinalPrice> productsList)
            {
                ProductsList = productsList;
            }

            public IReadOnlyCollection<CalculatedFinalPrice> ProductsList { get; }
        }

        public record PublishedCart : ICartStates
        {
            internal PublishedCart(IReadOnlyCollection<CalculatedFinalPrice> productsList, string csv)
            {
                ProductsList = productsList;
                Csv = csv;
            }

            public IReadOnlyCollection<CalculatedFinalPrice> ProductsList { get; }
            public string Csv { get; }
        }

    }
}

