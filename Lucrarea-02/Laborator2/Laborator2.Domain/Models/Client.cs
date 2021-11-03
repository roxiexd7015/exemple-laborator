using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Laborator2.Domain.Models
{
    public record Client
    {
        private static readonly Regex ValidAddress = new("^RO ");
        public string Address { get; }

        public Client(string address)
        {
            if (IsValid(address))
            {
                Address = address;
            }
            else
            {
                throw new InvalidClientAddressException("Client address is invalid.");
            }
        }

        public override string ToString()
        {
            return $"{Address:0.##}";
        }

        private static bool IsValid(string stringValue) => ValidAddress.IsMatch(stringValue);

        public static bool TryParseAddress(string stringValue, out Client address)
        {
            bool isValid = false;
            address = null;

            if (IsValid(stringValue))
            {
                isValid = true;
                address = new(stringValue);
            }

            return isValid;
        }
    }
}