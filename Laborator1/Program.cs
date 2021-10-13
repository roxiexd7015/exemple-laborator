using Laborator1.Domain;
using System;
using System.Collections.Generic;
using static Laborator1.Domain.StareCos;
using System.Linq;

namespace Laborator1
{
    class Program
    {
        private static readonly Random random = new Random();
        public static string VerificareNume()
        {
            Console.Write("Introdu nume de client: ");
            return Console.ReadLine();
        }
        public static string VerificareParola()
        {
            Console.Write("Introdu parola: ");
            return Console.ReadLine();
        }

        private static List<CosClientNevalidat> IncarcaProduse()
        {
            List<CosClientNevalidat> listaProduse = new();
            do
            {
                //read client registration number and products and create a list of products
                var client = new Client(VerificareNume(), VerificareParola());
                if (client.Nume != "Client" || client.Parola != "1234")
                {
                    Console.WriteLine("Date eronate!");
                    break;
                }
                Console.WriteLine("\nCUMPARATURI PLACUTE!:)");
                List<Produs> listaProduse2 = new List<Produs>();

                var cantIntrodusa = ReadValue("Cantitate: ");
                if (string.IsNullOrEmpty(cantIntrodusa))
                {
                    break;
                }

                var idProd = ReadValue("ID: ");
                if (string.IsNullOrEmpty(idProd))
                {
                    break;
                }

                var denProd = ReadValue("Denumire: ");
                if (string.IsNullOrEmpty(denProd))
                {
                    break;
                }

                try
                {
                    listaProduse.Add(new(client.Nume, cantIntrodusa, idProd, denProd));
                }
                catch
                {
                    throw new IdProdusInvalidExceptie("Produs invalid. ERROR");
                }
            } while (true);
            return listaProduse;
        }
        private static IStareCos CosValidat(CosNevalid produseNevalidate) =>
          random.Next(100) > 50 ?
          new CosInvalid(new List<CosClientNevalidat>(), "Eroare random")
          : new CosValid(new List<CosClientValidat>());
        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
        static void Main(string[] args)
        {
            CosGol cosGol = new();
            var produseIncarcate = IncarcaProduse().ToArray();
            CosNevalid cosNevalid = new(produseIncarcate);
            IStareCos result = CosValidat(cosNevalid);
            result.Match(
                whenCosNevalid: cosNevalid => cosNevalid,
                whenCosPlatit: cosPlatit => cosPlatit,
                whenCosInvalid: cosInvalid => cosInvalid,
                whenCosGol: cosGol => cosGol,
                whenCosValid: cosValid => cosValid
            );

        }
    }
}
