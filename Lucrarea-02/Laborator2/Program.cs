using Laborator2.Domain.Models;
using Laborator2.Domain;
using System;
using System.Collections.Generic;
using static Laborator2.Domain.Models.CartStates;
using static Laborator2.Domain.Models.PaidCartEvent;
using LanguageExt;
using static LanguageExt.Prelude;
using System.Threading.Tasks;

namespace Laborator2
{
    class Program
    {
        private static readonly Random random = new Random();

        static async Task Main(string[] args)
        {
            /*
            var testCode = ProductCode.TryParseCode("ID123");
            var productExists = await testCode.Match(
                Some: testCode => CheckProductExists(testCode).Match(Succ: value => value, exception => false),
                None: () => Task.FromResult(false)
            );

            var result = from productCode in ProductCode.TryParseCode("ID123")
                                                    .ToEitherAsync(() => "Invlid product code.")
                         from exists in CheckProductExists(productCode)
                                                    .ToEither(ex =>
                                                    {
                                                        Console.Error.WriteLine(ex.ToString());
                                                        return "Could not validate product code";
                                                    })
                         select exists;

            await result.Match(
                 Left: message => Console.WriteLine(message),
                 Right: flag => Console.WriteLine(flag));
            //*/

            ///*
            var productsList = ReadProductsList().ToArray();
            PayCartCommand command = new(productsList);
            PaidCartWorkflow workflow = new PaidCartWorkflow();
            var result = await workflow.ExecuteAsync(command, CheckProductExists);

            result.Match(
                    whenPaidCartFailedEvent: @event =>
                    {
                        Console.WriteLine($"Payment failed: {@event.Reason}");
                        Console.WriteLine("We are sorry to announce you that the delivery could not be proccesed any further away.");
                        return @event;
                    },
                    whenPaidCartSuccedeedEvent: @event =>
                    {
                        Console.WriteLine($"Payment succeeded.");
                        var name = ReadValue("Client name: ");
                        Console.WriteLine("Thank you for shopping, " + name.ToString());
                        var address = ReadValue("Client address: ");
                        Console.WriteLine("Delivery will be made at: " + address.ToString());
                        Console.WriteLine(@event.Csv);
                        return @event;
                    }
                );
            //*/
        }

        private static int Sum()
        {
            Option<int> two = Some(2);
            Option<int> four = Some(4);
            Option<int> six = Some(6);
            Option<int> none = None;

            var result = from x in two
                         from y in four
                         from z in six
                         from n in none
                         select x + y + z + n;
            // This expression succeeds because all items to the right of 'in' are Some of int
            // and therefore it lands in the Some lambda.
            int r = match(result,
                           Some: v => v * 2,
                           None: () => 0);     // r == 24

            return r;
        }
        private static List<UnvalidatedCart> ReadProductsList()
        {
            List<UnvalidatedCart> productsList = new();
            bool state = false;
            Console.WriteLine("Welcome! Please enjoy your shopping!");
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
        private static TryAsync<bool> CheckProductExists(ProductCode code)
        {
            Func<Task<bool>> func = async () =>
                 {
                     //HttpClient client = new HttpClient();

                     //var response = await client.PostAsync($"www.university.com/checkRegistrationNumber?number={student.Value}", new StringContent(""));

                     //response.EnsureSuccessStatusCode(); //200

                     return true;
                 };
            return TryAsync(func);
        }
    }
}
