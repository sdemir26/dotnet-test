using Microsoft.EntityFrameworkCore;
using TalepOnayEkrani.Models;
using System.Collections.Generic;

namespace TalepOnayEkrani.Data
{
    public class UygulamaDbContext : DbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options)
            : base(options)
        {
        }

        public DbSet<DegisiklikTalebi> DegisiklikTalepleri { get; set; }
    }
}
