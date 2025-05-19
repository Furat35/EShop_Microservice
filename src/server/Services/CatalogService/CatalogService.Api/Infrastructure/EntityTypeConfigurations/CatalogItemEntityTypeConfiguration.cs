using CatalogService.Api.Core.Domain;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Api.Infrastructure.EntityTypeConfigurations
{
    public class CatalogItemEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItem>
    {
        public void Configure(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.ToTable("Catalog", CatalogContext.DEFAULT_SCHEMA);

            builder.HasKey(ct => ct.Id);

            builder.Property(ct => ct.Id)
                .UseHiLo("catalog_hilo")
                .IsRequired();

            builder.Property(ct => ct.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(ct => ct.Price)
                .IsRequired();

            builder.Property(ct => ct.PictureFileName)
                .IsRequired(false);

            builder.Ignore(ct => ct.PictureUri);

            builder.HasOne(ct => ct.CatalogBrand)
                .WithMany()
                .HasForeignKey(ct => ct.CatalogBrandId);

            builder.HasOne(ct => ct.CatalogType)
               .WithMany()
               .HasForeignKey(ct => ct.CatalogTypeId);
        }
    }
}
