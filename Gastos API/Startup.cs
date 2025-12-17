using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Gastos_API.Data;

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
                options.AddPolicy("AllowReact", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();
        }

        // ========================================
        // 2. ADICIONE AQUI: Configure (antes do UseRouting)
        // ========================================
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // === ATIVA O CORS ===
            app.UseCors("AllowReact");  // <--- COLOQUE AQUI

            app.UseHttpsRedirection();
            app.UseRouting();
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