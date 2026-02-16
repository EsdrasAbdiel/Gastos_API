using Gastos_API.Models;

namespace Gastos_API.Services
{
    public interface IAuthService
    {
        Task<Registro> AdicionarRegistroAsync(Registro registro);
        Task<Registro?> BuscarUsuarioPeloEmailAsync(string email);
    }
}
