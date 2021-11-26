using Laborator2.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator2.Data
{
    public class CartsContext : DbContext
    {
        public CartsContext(DbContextOptions<CartsContext> options) : base(options)
        {
        }

        public DbSet<ProductDto> Products { get; set; }
        public DbSet<OrderHeaderDto> OrderHeaders { get; set; }
        public DbSet<OrderLineDto> OrderLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDto>().ToTable("Product").HasKey(s => s.ProductId);
            modelBuilder.Entity<OrderHeaderDto>().ToTable("OrderHeader").HasKey(s => s.OrderId);
            modelBuilder.Entity<OrderLineDto>().ToTable("OrderLine").HasKey(s => s.OrderLineId);
        }
    }
}
