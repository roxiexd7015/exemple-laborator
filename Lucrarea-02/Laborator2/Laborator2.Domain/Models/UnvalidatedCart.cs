using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator2.Domain
{
    public record UnvalidatedCart(string ProductCode, string ProductPrice, string ProductQuantity, string ClientAddress);
}