using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CatalogService.Api.Infrastructure.Context
{
    public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<CatalogContext>
    {
        public CatalogContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>()
               .UseSqlServer("Server=.;Database=CatalogDb;Trusted_Connection=True;TrustServerCertificate=True");

            return new CatalogContext(optionsBuilder.Options);
        }
    }
}
