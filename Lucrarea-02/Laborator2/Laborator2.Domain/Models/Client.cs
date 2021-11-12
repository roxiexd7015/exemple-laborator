using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Laborator2.Domain.Models
{
    public record Client
    {
        private static readonly Regex ValidAddress = new("^RO ");

        public string Value { get; }
        public Client(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidClientAddressException("Client address is invalid.");
            }

        }
        public override string ToString()
        {
            return $"{Value}";
        }
        private static bool IsValid(string stringValue) => ValidAddress.IsMatch(stringValue);
        public static Option<Client> TryParseAddress(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<Client>(new(stringValue));
            }
            else
            {
                return None;
            }
        }
    }
}