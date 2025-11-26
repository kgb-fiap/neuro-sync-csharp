using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NeuroSync.Domain.Repositories;
using NeuroSync.Infrastructure.Persistence;
using NeuroSync.Infrastructure.Repositories;
using Oracle.EntityFrameworkCore;

namespace NeuroSync.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NeuroSyncDbContext>(options =>
            {
                options.UseOracle(configuration.GetConnectionString("DefaultConnection") ?? "User Id=NEURO_SYNC;Password=senha;Data Source=localhost:1521/XEPDB1");
            });

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IEstacaoTrabalhoRepository, EstacaoTrabalhoRepository>();
            services.AddScoped<IReservaEstacaoRepository, ReservaEstacaoRepository>();

            return services;
        }
    }
}
