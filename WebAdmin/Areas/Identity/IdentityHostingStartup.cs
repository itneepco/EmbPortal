using Domian.Identity;
using Microsoft.AspNetCore.Hosting;
using Persistence.Identity;
using Microsoft.Extensions.DependencyInjection;


[assembly: HostingStartup(typeof(WebAdmin.Areas.Identity.IdentityHostingStartup))]
namespace WebAdmin.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDefaultIdentity<AppUser>()
                    .AddRoles<AppRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>();
            });
        }
    }
}