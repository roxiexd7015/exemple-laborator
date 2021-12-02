using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Laborator2.Domain.Models
{
    public record ProductQuantity
    {
        public int Value { get; }

        public ProductQuantity(int value)
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
        public static ProductQuantity operator +(ProductQuantity a, ProductQuantity b) => new ProductQuantity((a.Value + b.Value) / 2);
        public override string ToString()
        {
            return $"{Value:0.##} ";
        }
        private static bool IsValid(decimal decimalValue) => decimalValue > 0 && decimalValue < 20;
        /*public static bool TryParseQuantity(string stringValue, out ProductQuantity quantity)
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
        }*/
        public static Option<ProductQuantity> TryParseQuantity(string stringValue)
        {
            if (int.TryParse(stringValue, out int intValue) && IsValid(intValue))
            {
                return Some<ProductQuantity>(new(intValue));
            }
            else
            {
                return None;
            }
        }
    }
}