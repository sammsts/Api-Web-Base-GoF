using Application.Interfaces;
using Application.Services;
using Infrastructure.Services;
using Infrastructure.Persistence;
using Domain.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Application.Strategies;
using Application.Commands;
using ApiWebBase.Factories;
using Microsoft.Extensions.Caching.Memory;

namespace ApiWebBase.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtOptions>(config.GetSection("Jwt"));

            // Habilita Cache em Memória (Necessário para o Padrão Decorator)
            services.AddMemoryCache();

            #region Padrões GoF - Registros

            // 1. SINGLETON (Banco em Memória)
            // Registra a classe concreta como Singleton para manter os dados vivos
            services.AddSingleton<AuthRepository>();

            // 2. DECORATOR (Cache no Repositório)
            // Quando alguém pedir IAuthRepository, entrega o CachedAuthRepository que envolve o AuthRepository real.
            services.AddScoped<IAuthRepository>(provider =>
                new CachedAuthRepository(
                    provider.GetRequiredService<AuthRepository>(), // Injeta o Singleton real
                    provider.GetRequiredService<IMemoryCache>()    // Injeta o Cache
                ));

            // 3. STRATEGY (Geração de Token)
            // Facilita a troca da estratégia de token (JWT, Reference, etc)
            services.AddScoped<ITokenStrategy, JwtTokenStrategy>();

            // 4. COMMAND (Handler de Login)
            // Registra o manipulador do comando de login
            services.AddScoped<LoginCommandHandler>();

            // 5. FACTORY METHOD (Criação de Logs)
            services.AddScoped<IAuditLogFactory, AuditLogFactory>();

            #endregion

            #region Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            #endregion

            return services;
        }
    }
}
