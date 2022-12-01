using GameStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(Product));
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Developer).WithMany(d => d.Products).HasForeignKey(e => e.DeveloperId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
