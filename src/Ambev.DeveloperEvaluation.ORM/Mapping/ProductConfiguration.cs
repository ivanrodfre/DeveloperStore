using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(s => s.Id);
            builder.Property(u => u.Id).HasColumnType("uuid").ValueGeneratedNever();
            builder.Property(s => s.ProductId).IsRequired();
            builder.Property(s => s.SaleId).IsRequired();
            builder.Property(s => s.Quantity).HasColumnType("decimal(5,2)").IsRequired();
            builder.Property(s => s.UnitPrice).IsRequired();
            builder.Property(s => s.Discount).IsRequired();
            builder.Property(s => s.TotalPrice).HasColumnType("decimal(18,2)").IsRequired();


            builder.HasOne<Sale>()
                   .WithMany(sale => sale.Products)
                   .HasForeignKey(product => product.SaleId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
