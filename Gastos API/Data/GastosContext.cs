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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.HasDefaultSchema("public");
        }
    }
}
