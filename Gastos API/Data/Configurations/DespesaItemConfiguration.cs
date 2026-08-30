using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gastos_API.Models;

namespace Gastos_API.Data.Configurations
{
    public class DespesaItemConfiguration : IEntityTypeConfiguration<DespesaItem>
    {
        public void Configure(EntityTypeBuilder<DespesaItem> builder)
        {
            builder.ToTable("despesasitem");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();     // autoincremento
            builder.Property(x => x.Descricao)
                   .HasMaxLength(200);
            builder.Property(x => x.Valor)
                   .HasColumnType("decimal(10,2)");
            builder.Property(x => x.Pago)
                   .HasColumnType("boolean");
            builder.Property(x => x.DataInclusao)
                    .HasColumnType("date")
                    .HasColumnName("datainclusao");
        }
    }
}
