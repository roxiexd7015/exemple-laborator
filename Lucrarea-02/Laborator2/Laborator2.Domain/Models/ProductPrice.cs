using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Laborator2.Domain.Models
{
    public record ProductPrice
    {
        public decimal Value { get; }
        public ProductPrice(decimal value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductPriceException("");
            }
        }
        public override string ToString()
        {
            return $"{Value:0.##}";
        }
        private static bool IsValid(decimal decimalValue) => decimalValue > 0 && decimalValue < 500;
        public static bool TryParsePrice(string stringValue, out ProductPrice price)
        {
            bool isValid = false;
            price = null;
            if (decimal.TryParse(stringValue, out decimal decimalValue))
            {
                if (IsValid(decimalValue))
                {
                    isValid = true;
                    price = new(decimalValue);
                }
            }

            return isValid;
        }
    }
}