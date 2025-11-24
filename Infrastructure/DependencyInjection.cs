using ApiWebBase.Factories;
using Application.Commands;
using Application.Interfaces;
using Application.Observers;
using Application.Services;
using Application.Strategies;
using Domain.Configurations;
using Infrastructure.Adapters;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            // 6. OBSERVER (Notificação de Auditoria)
            services.AddScoped<IAuditObserver, SecurityAlertObserver>();

            // 7. ADAPTER (Registra o Adapter como um Observer também)
            // O AuditService vai receber agora dois observadores: o de Alerta e o Adapter do Legado.
            services.AddScoped<IAuditObserver, LegacyLoggerAdapter>();

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
