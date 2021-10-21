using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator1.Domain
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
            try
            {
                Price = Convert.ToDecimal(price);
            }
            catch
            {
                Price = 0;
                throw new InvalidProductPriceException($"{price:0.##} is an invalid quantity value.");
            }

            if (!string.IsNullOrEmpty(name)) // add some regex maybe
            {
                Name = name;
            }
            else
            {
                throw new InvalidProductNameException($"{name:0.##} is an invalid code value.");
            }

            Console.WriteLine($"\nProduct: \n\tquantity: {Quantity}\n\tcode: {Name}");
        }

        public override string ToString()
        {
            return $"{Quantity:0.##} buc., {Name:0.##}, {Price:0.##} lei. ";
        }
    }
}