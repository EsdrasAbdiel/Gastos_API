using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gastos_API.Models;

namespace Gastos_API.Data.Configurations
{
    public class ResumoFinanceiroMensalConfiguration : IEntityTypeConfiguration<ResumoFinanceiroMensal>
    {
        public void Configure(EntityTypeBuilder<ResumoFinanceiroMensal> builder)
        {
            builder.ToTable("resumoFinanceiro");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("id")
                   .HasColumnType("uuid");

            builder.Property(x => x.UsuarioId)
                   .HasColumnName("usuarioId")
                   .HasColumnType("uuid");

            builder.Property(x => x.ValorDespesaTotal)
                   .HasColumnName("valordespesatotal");

            builder.Property(x => x.ValorEntradaTotal)
                   .HasColumnName("valorentradatotal");

            builder.Property(x => x.DataInclusao)
                   .HasColumnName("datainclusao")
                   .HasColumnType("date");

            builder.Property(x => x.Mes)
                   .HasColumnName("mes");

            builder.Property(x => x.Ano)
                   .HasColumnName("ano");

            builder.Ignore(x => x.StatusCompetenciaMes);

            builder.HasMany(x => x.ItensDespesa)
                   .WithOne()
                   .HasForeignKey(x => x.DespesaId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Usuario)
                   .WithMany(x => x.ResumosFinanceiros)
                   .HasForeignKey(x => x.UsuarioId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}