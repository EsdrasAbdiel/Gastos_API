using Gastos_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gastos_API.Data.Configurations
{
    public class RegistroConfiguration : IEntityTypeConfiguration<Registro>
    {
        public void Configure(EntityTypeBuilder<Registro> builder)
        {
            builder.ToTable("usuario");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .HasColumnType("uuid");
            builder.Property(x => x.Nome)
                    .HasMaxLength(200);
            builder.Property(x => x.Email)
                    .HasMaxLength(200);
        }
    }
}
