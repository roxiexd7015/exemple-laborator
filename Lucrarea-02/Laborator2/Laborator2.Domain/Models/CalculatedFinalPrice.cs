using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator2.Domain.Models
{
    public record CalculatedFinalPrice(ProductCode ProductCode, ProductPrice ProductPrice, ProductQuantity ProductQuantity, ProductPrice TotalPrice)
    {
        public int OrderId { get; set; } //ProductId
        public bool IsUpdated { get; set; }
    }
}