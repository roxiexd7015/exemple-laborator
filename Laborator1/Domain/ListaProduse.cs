using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator1.Domain
{
    public static class ListaProduse
    {
        public static List<Produs> Produse()
        {
            return new List<Produs>
            {
                new Produs("50","001","Paine", "1.50"),
                new Produs("5","002", "Carne de pui", "16.99"),
                new Produs("23","003","Inghetata", "2.30"),
            };
        }

    }
}
