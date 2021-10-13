using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator1.Domain
{
    public record Produs
    {
        public int Cantitate { get; }
        public string Id { get; }
        public string Denumire { get; }
        public string Pret { get; }

        public Produs(string cantitate, string id, string denumire, string pret)
        {
            // try parsing the quantity from string to Int32
            try
            {
                Cantitate = Int32.Parse(cantitate);
            }
            catch
            {
                Cantitate = 0;
                throw new CantitateInvalidaExceptie($"{cantitate:0.##} reprezinta o cantitate invalida.");
            }

            if (!string.IsNullOrEmpty(id))
            {
                Id = id;
            }
            else
            {
                throw new IdProdusInvalidExceptie($"{id:0.##} nu reprezinta o valoare valida.");
            }
            if (!string.IsNullOrEmpty(denumire))
            {
                Denumire = denumire;
            }
            else
            {
                throw new IdProdusInvalidExceptie($"{denumire:0.##} nu exista.");
            }

            Console.WriteLine($"\nProdus: \n\tcantitate: {Cantitate}\n\tID: {Id}\n\tdenumire: {Denumire}\n\tpret: {Pret}");
        }

        public override string ToString()
        {
            return $"{Cantitate:0.##}";
        }
    }

}
