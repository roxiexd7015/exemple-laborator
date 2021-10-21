using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator2.Domain
{
    public record Product
    {
        public int Quantity { get; }
        public decimal Price { get; }
        public string Name { get; }

        public Product(string quantity, decimal price, string name)
        {
            try
            {
                Quantity = Int32.Parse(quantity);
            }
            catch
            {
                Quantity = 0;
                throw new InvalidProductQuantityException($"{quantity:0.##} is an invalid quantity value.");
            }
            if (IsValid(price))
            {
                Price = price;
            }
            else
            {
                throw new InvalidProductPriceException($"{price:0.##} is an invalid quantity value.");
            }

            if (!string.IsNullOrEmpty(name))
            {
                Name = name;
            }
            else
            {
                throw new InvalidProductNameException($"{name:0.##} is an invalid code value.");
            }

            Console.WriteLine($"\nProduct: \n\tquantity: {Quantity}\n\tcode: {Name}");
        }
        public static Price operator +(Price a, Price b) => new Price((a.Value + b.Value) / 2m);

        public Price Round()
        {
            var roundedValue = Math.Round(Value);
            return new Price(roundedValue);
        }
        public override string ToString()
        {
            return $"{Quantity:0.##} buc., {Name:0.##}, {Price:0.##} lei. ";
        }
        public static bool TryParsePrice(string priceString, out Price price)
        {
            bool isValid = false;
            price = null;
            if (decimal.TryParse(priceString, out decimal numericPrice))
            {
                if (IsValid(numericPrice))
                {
                    isValid = true;
                    price = new(numericPrice);
                }
            }

            return isValid;
        }
        private static bool IsValid(decimal productPrice) => productPrice > 0;
    }
}