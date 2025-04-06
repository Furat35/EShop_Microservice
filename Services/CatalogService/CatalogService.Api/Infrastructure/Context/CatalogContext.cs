using CatalogService.Api.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CatalogService.Api.Infrastructure.Context
{
    public class CatalogContext(DbContextOptions<CatalogContext> options) : DbContext(options)
    {
        public const string DEFAULT_SCHEMA = "catalog";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogType> CatalogTypes { get; set; }
    }
}
