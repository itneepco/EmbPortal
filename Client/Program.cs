using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Client.Services;
using Client.Services.Interfaces;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddAntDesign()
                .AddAuthorizationCore()
                .AddBlazoredLocalStorage()
                .AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<IAuthService, AuthService>()
                .AddScoped<EMBStateProvider>()
                .AddScoped<AuthenticationStateProvider, EMBStateProvider>();

            builder.Services.AddScoped<IProjectService, ProjectService>()
                .AddScoped<IContractorService, ContractorService>()
                .AddScoped<IWorkOrderService, WorkOrderService>()
                .AddScoped<IUomService, UomService>()
                .AddScoped<IMBookService, MBookService>()
                .AddScoped<IUserService, UserService>();

            builder.Services.AddHttpClientInterceptor();
            builder.Services.AddScoped<IHttpInterceptorService, HttpInterceptorService>();

            await builder.Build().RunAsync();
        }
    }
}
