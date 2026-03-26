using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gastos_API.Models;

namespace Gastos_API.Data.Configurations
{
    public class ResumoFinanceiroMensalConfiguration : IEntityTypeConfiguration<ResumoFinanceiroMensal>
    {
        public void Configure(EntityTypeBuilder<ResumoFinanceiroMensal> builder)
        {
            builder.ToTable("despesas");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .HasColumnType("uuid");
            builder.HasMany(x => x.ItensDespesa)
                   .WithOne()
                   .HasForeignKey(x => x.DespesaId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
