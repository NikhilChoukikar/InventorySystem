using InventorySystem.Application.Interfaces.Repositories;
using InventorySystem.Application.Interfaces.Security;
using InventorySystem.Infrastructure.Data;
using InventorySystem.Infrastructure.Repositories;
using InventorySystem.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;



namespace InventorySystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Dapper context
            services.AddScoped<DapperContext>(_ =>
                new DapperContext(
                    configuration.GetConnectionString("DefaultConnection")!
                )
            );

            // Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>(); // 🔥 THIS LINE WAS MISSING
            services.AddScoped<JwtTokenGenerator>();

            services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();

            return services;
        }
    }
}

