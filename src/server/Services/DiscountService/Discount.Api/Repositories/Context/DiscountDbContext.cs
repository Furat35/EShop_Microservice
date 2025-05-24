using Microsoft.EntityFrameworkCore;

namespace Discount.Api.Repositories.Context
{
    public class DiscountDbContext(DbContextOptions<DiscountDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Discount>().HasKey(cb => cb.Id);

            modelBuilder.Entity<Models.CatalogItem>().Property(_ => _.Id).ValueGeneratedNever();
            modelBuilder.Entity<Models.CatalogItem>().HasKey(cb => cb.Id);
            modelBuilder.Entity<Models.Discount>()
                .HasMany(_ => _.Items)
                .WithOne(_ => _.Discount)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Models.Discount> Discounts { get; set; }
        public DbSet<Models.CatalogItem> Products { get; set; }
    }
}
