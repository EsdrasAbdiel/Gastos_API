using Gastos_API.Models;

namespace Gastos_API.Services
{
    public interface IAuthService
    {
        Task<Registro> AdicionarRegistroAsync(RegistroRequest registro);
        Task<Registro?> BuscarUsuarioPeloEmailAsync(RegistroRequest registro);
        string GerarToken(Registro registro);
    }
}
