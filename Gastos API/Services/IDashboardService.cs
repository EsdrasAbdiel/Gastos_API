using Gastos_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gastos_API.Services
{
    public interface IDashboardService
    {
        Task<Dashboard> ListarAsync(Guid id);
    }
}
