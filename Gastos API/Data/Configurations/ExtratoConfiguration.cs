using Gastos_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gastos_API.Data.Configurations
{
    public class ExtratoConfiguration : IEntityTypeConfiguration<ImportacaoExtrato>
    {
        public void Configure(EntityTypeBuilder<ImportacaoExtrato> builder)
        {
            builder.ToTable("importacao_extrato");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                   .HasColumnName("id")
                   .ValueGeneratedOnAdd()
                   .UseIdentityByDefaultColumn();

            builder.Property(i => i.UsuarioId)
                   .HasColumnName("usuario_id")
                   .IsRequired();

            builder.Property(i => i.DataImportacao)
                .HasColumnType("date")
                    .HasColumnName("data_importacao")
                    .IsRequired();


            builder.Property(i => i.Status)
        .HasColumnName("status")
        .HasColumnType("integer")
        .IsRequired();

            builder.Property(i => i.QuantidadeLancamentos)
.HasColumnName("quantidade_lancamentos")
.HasColumnType("integer")
.IsRequired();

            builder.Property(i => i.ReferenciaMes)
.HasColumnName("referencia_mes")
.HasColumnType("integer")
.IsRequired();

            builder.Property(i => i.IdResumoFinanceiro).HasColumnName("id_resumo_financeiro");
        }
    }
}
