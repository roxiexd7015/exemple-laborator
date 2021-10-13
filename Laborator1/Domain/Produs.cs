using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator1.Domain
{
    public record Produs
    {
        private string id;
        private string denumire;
        private Int16 cantitate;
        private float pret;

        public Produs(string id,string denumire, short cantitate, float pret)
        {
            this.denumire = denumire;
            this.id = id;
            this.cantitate = cantitate;
            this.pret = pret;
        }
        public string Id { get => id; set => id = value; }
        public string Denumire { get => denumire; set => denumire = value; }
        public short Cantitate { get => cantitate; set => cantitate = value; }
        public float Pret { get => pret; set => pret = value; }

        public override string ToString()
        {
            return "ID: " + id + ", denumire: " + denumire + ", cantitate: " + cantitate + " PRET: " + pret;
        }
    }

}
