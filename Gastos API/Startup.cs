using Gastos_API.Data;
using Gastos_API.Interfaces;
using Gastos_API.Repositorios;
using Gastos_API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Gastos_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // ========================================
        // 1. ADICIONE AQUI: ConfigureServices
        // ========================================
        public void ConfigureServices(IServiceCollection services)
        {
            // === REGISTRA A POLÍTICA CORS ===
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // ? ADICIONE ISSO AQUI ?
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler =
                        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    // Opcional: ajuda bastante em debug
                    // options.JsonSerializerOptions.WriteIndented = true;
                });

            services.AddScoped<IDespesaService, DespesaRepository>();
            services.AddScoped<IEntradaService, EntradaRepository>();
        }

        // ========================================
        // 2. ADICIONE AQUI: Configure (antes do UseRouting)
        // ========================================
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // === ATIVA O CORS ===
            app.UseRouting();
            app.UseCors("AllowReact");
            app.UseAuthorization();

            // Rota raiz (sua página de status)
            app.MapWhen(context => context.Request.Path == "/", appBranch =>
            {
                appBranch.Run(async context =>
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync("<h2>Rodando API Gastos</h2>");
                });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}