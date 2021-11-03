using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator2.Domain
{
    public record ProductQuantity
    {
        public decimal Value { get; }

        public ProductQuantity(decimal value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductQuantityException("");
            }
        }
        public static ProductQuantity operator +(ProductQuantity a, ProductQuantity b) => new ProductQuantity((a.Value + b.Value) / 2m);
        public ProductQuantity Round()
        {
            var roundedValue = Math.Round(Value);
            return new ProductQuantity(roundedValue);
        }
        public override string ToString()
        {
            return $"{Value:0.##} ";
        }
        private static bool IsValid(decimal decimalValue) => decimalValue > 0 && decimalValue < 10;
        public static bool TryParseQuantity(string stringValue, out ProductQuantity quantity)
        {
            bool isValid = false;
            quantity = null;
            if (decimal.TryParse(stringValue, out decimal decimalValue))
            {
                if (IsValid(decimalValue))
                {
                    isValid = true;
                    quantity = new(decimalValue);
                }
            }

            return isValid;
        }

    }
}