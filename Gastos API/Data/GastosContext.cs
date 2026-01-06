using Microsoft.EntityFrameworkCore;
using Gastos_API.Models;

namespace Gastos_API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Despesa> Despesas { get; set; }
        public DbSet<Entrada> Entradas{ get; set; }
        public DbSet<EntradaItem> EntradaItens { get; set; }
        public DbSet<DespesaItem> DespesaItens { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Despesa>(entity =>
            {
                entity.ToTable("despesas");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                      .HasColumnType("uuid");

                entity.HasMany(x => x.ItensDespesa)
                      .WithOne()                    // sem navegação
                      .HasForeignKey(x => x.DespesaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DespesaItem>(entity =>
            {
                entity.ToTable("despesasitem");

                entity.HasKey(x => x.Id);

                // Id agora é int
                entity.Property(x => x.Id)
                      .ValueGeneratedOnAdd();     // autoincremento

                entity.Property(x => x.Descricao)
                      .HasMaxLength(200);

                entity.Property(x => x.Valor)
                      .HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<Entrada>(entity =>
            {
                entity.ToTable("entradas");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id)
                      .HasColumnType("uuid");
                entity.HasMany(x => x.EntradaItens)
                      .WithOne()                    // sem navegação
                      .HasForeignKey(x => x.Id)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EntradaItem>(entity =>
            {
                entity.ToTable("entradasitem");
                entity.HasKey(x => x.Id);
                // Id agora é int
                entity.Property(x => x.Id)
                      .ValueGeneratedOnAdd();     // autoincremento
                entity.Property(x => x.EntradaDescricao)
                      .HasMaxLength(200);
                entity.Property(x => x.EntradaValor)
                      .HasColumnType("decimal(10,2)");
            });
        }
    }
}
