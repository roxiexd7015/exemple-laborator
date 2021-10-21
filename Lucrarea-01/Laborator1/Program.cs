using Laborator1.Domain;
using System;
using System.Collections.Generic;
using static Laborator1.Domain.CartStates;

namespace Laborator1
{
    class Program
    {
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            var productsList = ReadProductsList().ToArray();
            UnvalidateCart unvalidatedCart = new(productsList);
            ICartStates result = ValidateCart(unvalidatedCart);
            result.Match(
                whenEmptyCart: emptyResult => emptyCartProducts,
                whenUnvalidatedCart: unvalidatedResult => unvalidatedCart,
                whenPublishedCart: publishedResult => publishedResult,
                whenInvalidatedCart: invalidResult => invalidResult,
                whenValidatedCart: validatedResult => PayingCart(validatedResult)
            );

            var name = ReadValue("Type your name: ");
            var address = ReadValue("Type your address: ");
            Console.WriteLine("The products for " + name.ToString() + " will be shipped to address: " + address.ToString());
        }

        private static List<UnvalidateCart> ReadProductsList()
        {
            List<UnvalidateCart> productsList = new();
            do
            {
                var name = ReadValue("Product name: ");
                if (string.IsNullOrEmpty(name))
                {
                    break;
                }

                var quantity = ReadValue("Quantity: ");
                if (string.IsNullOrEmpty(quantity))
                {
                    break;
                }

                try
                {
                    productsList.Add(new(name, quantity));
                }
                catch
                {
                    throw new InvalidProductIdException("Input error at creating a new Product");
                }
            } while (true);
            return productsList;
        }

        private static ICartStates ValidateCart(UnvalidatedCart unvalidatedProducts) =>
            random.Next(100) > 50 ?
            new InvalidatedCart(new List<UnvalidateCart>(), "Random error")
            : new ValidatedCart(new List<ValidateCart>());

        private static ICartStates ValidateCart2(UnvalidatedCart unvalidatedProducts) =>
            random.Next(100) > 50 ?
            new InvalidatedCart(new List<UnvalidateCart>(), "Random error")
            : new ValidatedCart(new List<ValidateCart>());

        private static ICartStates PayingCart(ValidatedCart validCartProducts) =>
            new PublishedCart(new List<ValidateCart>(), DateTime.Now);


        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}