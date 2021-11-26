using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator2.Data.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public decimal? Price { get; set; }
        public int Stock { get; set; }
    }

}