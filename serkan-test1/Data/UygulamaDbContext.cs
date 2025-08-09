using Microsoft.EntityFrameworkCore;
using serkan_test1.Models;
using System.Collections.Generic;

namespace serkan_test1.Data
{
    public class UygulamaDbContext : DbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options)
            : base(options)
        {
        }

        public DbSet<DegisiklikTalebi> DegisiklikTalepleri { get; set; }
        public DbSet<FirmaBilgileriViewModel> Firmalar { get; set; }
        public DbSet<Customer> Customers { get; set; }

    }
}
