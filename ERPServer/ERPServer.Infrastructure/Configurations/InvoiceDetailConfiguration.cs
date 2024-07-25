using ERPServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERPServer.Infrastructure.Configurations
{
    internal sealed class InvoiceDetailConfiguration : IEntityTypeConfiguration<InvoiceDetail>
    {
        public void Configure(EntityTypeBuilder<InvoiceDetail> builder)
        {
            builder.HasOne(x => x.Product)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(x => x.Price).HasColumnType("money");     
            builder.Property(x => x.Quantity).HasColumnType("decimal(7,2)");
        }
    }
}
