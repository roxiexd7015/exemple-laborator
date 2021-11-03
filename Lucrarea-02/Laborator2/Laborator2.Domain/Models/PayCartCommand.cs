using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator2.Domain.Models
{
    public record PayCartCommand
    {
        public PayCartCommand(IReadOnlyCollection<UnvalidatedCart> inputCart)
        {
            InputCart = inputCart;
        }

        public IReadOnlyCollection<UnvalidatedCart> InputCart { get; }
    }
}
