using Microsoft.Extensions.DependencyInjection;
using NeuroSync.Application.Common;
using NeuroSync.Application.Services;

namespace NeuroSync.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IEstacaoService, EstacaoService>();
            services.AddScoped<IReservaService, ReservaService>();
            services.AddScoped<IPreferenciaSensorialService, PreferenciaSensorialService>();

            services.AddOptions<JwtSettings>().BindConfiguration("Jwt");

            return services;
        }
    }
}
