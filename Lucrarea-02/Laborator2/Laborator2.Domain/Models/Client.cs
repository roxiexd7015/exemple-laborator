using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cart.Domain
{
    public record Client
    {
        private static readonly Regex ValidName = new("^client[0-9]{3}$");

        public string Name { get; }
        public string Address { get; }

        public Client(string name, string address)
        {
            if (ValidPattern.IsMatch(value))
            {
                Name = name;
            }
            else
            {
                throw new InvalidClientNameException("Client name is invalid.");
            }

            if (!string.IsNullOrEmpty(address)) // add some regex maybe
            {
                Address = address;
            }
            else
            {
                throw new InvalidClientAddressException($"{address:0.##} is an invalid address value.");
            }
        }

        public override string ToString()
        {
            return $"{Name:0.##}, {Address:0.##}";
        }

        private static bool IsValid(string stringValue) => ValidName.IsMatch(stringValue);

        public static bool TryParse(string stringValue, out Client name)
        {
            bool isValid = false;
            name = null;

            if (IsValid(stringValue))
            {
                isValid = true;
                name = new(stringValue);
            }

            return isValid;
        }
    }
}