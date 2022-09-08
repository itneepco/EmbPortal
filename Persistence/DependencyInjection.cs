using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,
                                                           IConfiguration config)
        {
          services.AddDbContext<AppDbContext>(x =>
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                //x.UseSqlite(config.GetConnectionString("DefaultConnection"));
               var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
                x.UseMySql(connectionString, serverVersion);

            });
          services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

          return services;
        }
    }
}