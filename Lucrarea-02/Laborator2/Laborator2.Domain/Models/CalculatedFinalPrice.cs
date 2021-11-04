using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator2.Domain.Models
{
    public record CalculatedFinalPrice(ProductCode code, ProductPrice price, ProductQuantity quantity, ProductPrice totalPrice);
}