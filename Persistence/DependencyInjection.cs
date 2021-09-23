using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,
                                                           IConfiguration config)
        {
          services.AddDbContext<AppDbContext>(x =>
            {
                x.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
          services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

          return services;
        }
    }
}