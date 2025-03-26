using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class EventConfiguration : IEntityTypeConfiguration<EventMessage>
    {
        public void Configure(EntityTypeBuilder<EventMessage> builder)
        {
            builder.ToTable("Events");
            builder.HasKey(s => s.Id);
            builder.Property(u => u.Id).HasColumnType("uuid").ValueGeneratedNever();

        }
    }
}
