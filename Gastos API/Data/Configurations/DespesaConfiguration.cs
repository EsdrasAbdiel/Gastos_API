using Gastos_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Gastos_API.Data.Configurations
{
    public class DespesaConfiguration : IEntityTypeConfiguration<Despesa>
    {
        public void Configure(EntityTypeBuilder<Despesa> builder)
        {
            builder.ToTable("despesa");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Descricao).HasColumnName("descricao");
            builder.Property(x => x.CategoriaId).HasColumnName("categoria_id");
            builder.HasOne(x => x.Categoria)
       .WithMany(x => x.Despesas)
       .HasForeignKey(x => x.CategoriaId)
       .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
