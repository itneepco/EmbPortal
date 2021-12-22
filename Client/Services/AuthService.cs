using Blazored.LocalStorage;
using Client.Extensions;
using Client.Models;
using Client.Services.Interfaces;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly NavigationManager _navManager;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider, NavigationManager navManager)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
            _navManager = navManager;
        }

        public async Task<IResult> Login(LoginRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Identity/Login", model);
            var result = await response.ToResult<AuthUserResponse>();

            if (result.Succeeded)
            {
                var token = result.Data.Token;
                await _localStorage.SetItemAsync("authToken", token);
                ((EMBStateProvider) _authStateProvider).MarkUserAsAuthenticated(token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return Result.Success("Logged In successfully");
            }
            else
            {
                return Result.Fail(result.Message);
            }
        }

        public async Task<IResult> Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((EMBStateProvider) _authStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            _navManager.NavigateTo("/");

            return Result.Success("Logged out successfully");
        }

        public async Task<string> RefreshToken()
        {
            var response = await _httpClient.GetAsync("/api/Identity/Currentuser");
            var result = await response.ToResult<AuthUserResponse>();

            if (!result.Succeeded)
            {
                throw new ApplicationException("Something went wrong during the refresh token action");
            }

            var token = result.Data.Token;
            await _localStorage.SetItemAsync("authToken", token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return token;
        }

        public async Task<string> TryRefreshToken()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals("exp")).Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            var timeUTC = DateTime.UtcNow;
            var diff = expTime - timeUTC;

            //if token is going to expire within 5 mins, refresh the token if user make http call in that period
            if (diff.TotalMinutes <= 5)
                return await RefreshToken();

            return string.Empty;
        }
    }
}
