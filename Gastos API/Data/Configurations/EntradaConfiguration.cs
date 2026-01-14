using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gastos_API.Models;

namespace Gastos_API.Data.Configurations
{
    public class EntradaConfiguration : IEntityTypeConfiguration<Entrada>
    {
        public void Configure(EntityTypeBuilder<Entrada> builder)
        {
            builder.ToTable("entradas");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .HasColumnType("uuid");
            builder.Property(x => x.Despesa_Id)
                   .HasColumnName("despesa_id");
            builder.HasMany(x => x.EntradaItens)
                   .WithOne()
                   .HasForeignKey(x => x.Entrada_Id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
