using Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Toolbelt.Blazor;

namespace Client.Services
{
    public class HttpInterceptorService : IHttpInterceptorService
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly IAuthService _authService;
        private readonly NavigationManager _navigationManager;
        public HttpInterceptorService(HttpClientInterceptor interceptor, IAuthService authService, NavigationManager navigationManager)
        {
            _interceptor = interceptor;
            _authService = authService;
            _navigationManager = navigationManager;
        }
        public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;
        public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absPath = e.Request.RequestUri.AbsolutePath;

            // if the http request is for login or to get current user, return from the method
            if (absPath.Contains("login") || absPath.Contains("currentuser")) return;

            try
            {
                var token = await _authService.TryRefreshToken();
                if (!string.IsNullOrEmpty(token))
                {
                    //_snackBar.Add("Refreshed Token.", Severity.Success);
                    e.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _authService.Logout();
                _navigationManager.NavigateTo("/");
            }
        }
        public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
    }
}
