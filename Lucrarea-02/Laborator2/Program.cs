using Laborator2.Domain.Models;
using Laborator2.Domain;
using System;
using System.Collections.Generic;
using static Laborator2.Domain.Models.CartStates;
using static Laborator2.Domain.Models.PaidCartEvent;

namespace Laborator2
{
    class Program
    {
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            var productsList = ReadProductsList().ToArray();
            PayCartCommand command = new(productsList);
            PaidCartWorkflow workflow = new PaidCartWorkflow();
            var result = workflow.Execute(command, (code) => true);

            result.Match(
                    whenPaidCartFailedEvent: @event =>
                    {
                        Console.WriteLine($"Payment failed: {@event.Reason}");
                        return @event;
                    },
                    whenPaidCartSuccedeedEvent: @event =>
                    {
                        Console.WriteLine($"Payment succeeded.");
                        Console.WriteLine(@event.Csv);
                        return @event;
                    }
                );

            var name = ReadValue("Client name: ");
            Console.WriteLine("Welcome, " + name.ToString());
            var address = ReadValue("Client address: ");
            Console.WriteLine("Delivery will be made at: " + address.ToString());
        }

        private static List<UnvalidatedCart> ReadProductsList()
        {
            List<UnvalidatedCart> productsList = new();
            bool state = false;
            do
            {
                var code = ReadValue("Product code: ");
                if (string.IsNullOrEmpty(code))
                {
                    break;
                }
                var price = ReadValue("Product price: ");
                if (string.IsNullOrEmpty(price))
                {
                    break;
                }
                var quantity = ReadValue("Product quantity: ");
                if (string.IsNullOrEmpty(quantity))
                {
                    break;
                }
                productsList.Add(new(code, price, quantity));

                var finishShopping = ReadValue("Finish shopping (Y/N): ");
                if (string.IsNullOrEmpty(finishShopping))
                {
                    break;
                }
                if (string.Compare(finishShopping, "Y") == 0)
                {
                    state = true;
                }
                else if (string.Compare(finishShopping, "N") == 0)
                {
                    state = false;
                }
            } while (state);
            return productsList;
        }

        private static string ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
