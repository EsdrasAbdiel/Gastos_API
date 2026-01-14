using Gastos_API.Models;

namespace Gastos_API.Services
{
    public interface IEntradaService
    {
        Task<List<EntradaItem>> BuscarItensEntradaPorIdAsync(Guid id);
    }
}
