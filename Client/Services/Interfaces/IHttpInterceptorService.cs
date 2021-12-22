using System.Threading.Tasks;
using Toolbelt.Blazor;

namespace Client.Services.Interfaces
{
    public interface IHttpInterceptorService
    {
        void RegisterEvent();
        Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e);
        void DisposeEvent();
    }
}
