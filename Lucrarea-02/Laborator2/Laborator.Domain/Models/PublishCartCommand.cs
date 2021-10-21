using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Laborator2.Domain.Models.CartStates;

namespace Laborator2.Domain.Models
{
    public record PublishCartCommand
    {
        public PublishCartCommand(IReadOnlyCollection<UnvalidateCart> inputCart)
        {
            InputCart = inputCart;
        }

        public IReadOnlyCollection<UnvalidateCart> InputCart { get; }
    }
}
