using BancaLafise.Application.Interfaces;
using BancaLafise.Infrastructure.Auth;
using BancaLafise.Infrastructure.Context;
using BancaLafise.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<LafiseDbContext>((sp, options) => {
            var connectionStrings = sp
                .GetRequiredService<IOptions<ConnectionOptions>>().Value;

            options.UseSqlite(connectionStrings.SQLite);
        });
        
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ISexoRepository, SexoRepository>();
        services.AddScoped<ITipoCuentaRepository, TipoCuentaRepository>();
        services.AddScoped<IEstadoUsuarioRepository, EstadoUsuarioRepository>();
        services.AddScoped<ITipoMovimientoRepository, TipoMovimientoRepository>();
        services.AddScoped<ITipoTransaccionRepository, TipoTransaccionRepository>();
        services.AddScoped<IReglaCumplimientoRepository, ReglaCumplimientoRepository>();
        services.AddScoped<ICuentaBancariaRepository, CuentaBancariaRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}