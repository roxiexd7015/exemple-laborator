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
                new Produs("001","Paine",50, 1.50f),
                new Produs("002", "Carne de pui", 10, 16.99f),
                new Produs("003","Inghetata",23, 2.30f),
            };
        }
        
    }
}
