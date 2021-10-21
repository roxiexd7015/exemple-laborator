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

        public record EmptyCart() : ICartStates;
        public record UnvalidatedCart(IReadOnlyCollection<UnvalidateCart> ProductsList) : ICartStates;

        public record InvalidatedCart(IReadOnlyCollection<UnvalidateCart> ProductsList, string reason) : ICartStates;

        public record ValidatedCart(IReadOnlyCollection<ValidateCart> ProductsList) : ICartStates;

        public record PublishedCart(IReadOnlyCollection<ValidateCart> ProductsList) : ICartStates;

    }
}

