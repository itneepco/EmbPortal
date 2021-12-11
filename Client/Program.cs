using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Client.Services;
using Client.Services.Interfaces;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddAntDesign();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IContractorService, ContractorService>();
            builder.Services.AddScoped<IWorkOrderService, WorkOrderService>();
            builder.Services.AddScoped<IUomService, UomService>();
            builder.Services.AddScoped<IMBookService, MBookService>();

            await builder.Build().RunAsync();
        }
    }
}
