using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gastos_API.Models;

namespace Gastos_API.Data.Configurations
{
    public class EntradaItemConfiguration : IEntityTypeConfiguration<EntradaItem>
    {
        public void Configure(EntityTypeBuilder<EntradaItem> builder)
        {
            builder.ToTable("entradaitem");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                   .HasColumnName("id")
                   .ValueGeneratedOnAdd()
                   .UseIdentityByDefaultColumn();

            builder.Property(i => i.Entrada_Id)
                   .HasColumnName("entrada_id")
                   .IsRequired();

            builder.Property(i => i.EntradaDescricao)
                   .HasMaxLength(200)
                   .HasColumnName("entradadescricao");

            builder.Property(i => i.EntradaValor)
                   .HasColumnType("numeric(10,2)")
                   .HasColumnName("entradavalor");

            // 🔥 RELACIONAMENTO EXPLÍCITO (resolve o erro)
            builder.HasOne(i => i.Despesa)
                   .WithMany(d => d.ItensEntrada)
                   .HasForeignKey(i => i.Entrada_Id)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
