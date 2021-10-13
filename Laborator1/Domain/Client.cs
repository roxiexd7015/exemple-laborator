using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator1.Domain
{
    public record Client(string Nume, string Parola);
    
       /*private string nume;
        private string parola;
        private string adresa;

        public Client(string nume, string parola, string adresa)
        {
            this.nume = nume;
            this.parola = parola;
            this.adresa = adresa;
        }
       
        public string Nume { get => nume; set => nume = value; }
        public string Parola { get => parola; set => parola = value; }
        public string Adresa { get => adresa; set => adresa = value; }*/
    
}
