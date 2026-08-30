using Gastos_API.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Gastos_API.BackgroundServices
{
    public class CompetenciaBackgroundService : BackgroundService
    {
        private readonly ILogger<CompetenciaBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public CompetenciaBackgroundService(
            ILogger<CompetenciaBackgroundService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var agora = DateTime.Now;

                if (agora.Day == 1 &&
                    agora.Hour == 00 &&
                    agora.Minute == 00)
                {
                    using var scope = _scopeFactory.CreateScope();

                    var calendarioService =
                        scope.ServiceProvider.GetRequiredService<ICalendarioService>();

                    var statusMes = calendarioService.VerificarCompetenciaMesPeloAno(
                        agora.Year,
                        agora.Month);

                    _logger.LogInformation("Status: {Status}", statusMes);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}