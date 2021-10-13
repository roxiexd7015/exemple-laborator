using Laborator1.Domain;
using System;
using System.Collections.Generic;
using static Laborator1.Domain.StareCos;
using System.Linq;

namespace Laborator1
{
    class Program
    {
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
        private static List<CosNevalid> IncarcaProduse()
        {
            List<CosNevalid> produseIncarcate = new();
            return produseIncarcate;
        }
        static void Main(string[] args)
        {
            var client = new Client(VerificareNume(), VerificareParola());
            if(client.Nume!="Client"||client.Parola!="1234")
            {
                Console.WriteLine("Date eronate!");
                return;
            }
            Console.WriteLine("\nCUMPARATURI PLACUTE!:)");
            List<Produs> listaProduse2 = new List<Produs>();

            while (true)
            {
                var listaProduse = ListaProduse.Produse();
                Console.WriteLine();
                listaProduse.ForEach(Console.WriteLine);
                Console.WriteLine("\nIntroduceti ID-ul produsul dorit sau tastati X daca ati terminat cumparaturile:");
                
                var introdus1 = Console.ReadLine();
                if (introdus1.Equals("X"))
                {
                    Console.WriteLine("\nMultumim pentru cumparaturi, la revedere!");
                    break;
                }

                var idIntrodus = int.Parse(introdus1.Trim());
                if (!listaProduse.Exists(x => x.Id == introdus1))
                {
                    Console.WriteLine("\n!!!ID-ul introdus nu exista!!!");
                    continue;
                }

                Console.WriteLine("Introduceti cantitatea dorita: ");
                var introdus2 = Console.ReadLine();
                Console.WriteLine("\nAm adaugat produsul in cos!");
                Console.WriteLine("COSUL CURENT: \n");
                listaProduse2.Add(listaProduse.Find(x => x.Id.Contains(introdus1)));
                listaProduse2.ForEach(Console.WriteLine);
            }


            /*var produseIncarcate = IncarcaProduse().ToArray();
            CosNevalid cosNevalid = new(produseIncarcate);
            IStareCos result = CosValid(cosNevalid);
            result.Match(
                whenCosNevalid: cosNevalid => cosNevalid,
                whenCosPlatit: cosPlatit => cosPlatit,
                whenCosGol: cosGol => cosGol,
                whenCosValid: cosValid => cosValid
            );*/

        }
    }
}
