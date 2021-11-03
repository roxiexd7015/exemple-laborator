using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Laborator2.Domain.Models
{
    public record ProductCode
    {
        private static readonly Regex ValidPattern = new("^ID[0-9]{3}$");
        public string Value { get; }

        private ProductCode(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductCodeException("");
            }
        }
        public override string ToString()
        {
            return Value;
        }
        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);
        public static bool TryParseCode(string stringValue, out ProductCode code)
        {
            bool isValid = false;
            code = null;
            if (IsValid(stringValue))
            {
                isValid = true;
                code = new(stringValue);
            }

            return isValid;
        }
    }
}