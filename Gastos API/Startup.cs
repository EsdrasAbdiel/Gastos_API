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

        public void ConfigureServices(IServiceCollection services)
        {
            // === CORS - Crie uma política nomeada ===
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecific", policy =>
                {
                    policy.WithOrigins(
                            "http://localhost:4200",          // Angular local
                            "https://localhost:4200",
                            "https://esdrasabdiel.github.io/GerenciamentoDeGastos/" // adicione seu domínio real depois
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); // se precisar de cookies/auth com credenciais
                });

                // Para testes rápidos (NÃO use em produção!):
                // options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler =
                        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });

            services.AddScoped<IDespesaService, DespesaRepository>();
            services.AddScoped<IEntradaService, EntradaRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Ordem correta do middleware (importante!)
            app.UseRouting();

            // Ative o CORS AQUI ? depois de Routing e antes de Authorization/Endpoints
            app.UseCors("AllowSpecific");  // ? mude para o nome correto da política

            app.UseAuthorization();

            // Rota raiz (opcional - sua página de status)
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