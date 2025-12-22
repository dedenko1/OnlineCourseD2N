using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using OnlineCourseD2N.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourseD2N.Shared.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient http, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<AuthResponse> LoginAsync(LoginDTO model)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/auth/login", model);
                var contentStr = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var statusCode = response.StatusCode;

                    return new AuthResponse
                    {
                        Success = false,
                        Message = contentStr
                    };
                }

                if (!response.IsSuccessStatusCode)
                {
                    if (contentStr.Trim().StartsWith("<"))
                    {
                        return new AuthResponse
                        {
                            Success = false,
                            Message = $"Server Error ({response.StatusCode}): Kemungkinan URL salah atau Server Crash. Cek Output."
                        };
                    }

                    // Jika errornya berupa JSON (misal dari BadRequest controller kita), coba parse
                    try
                    {
                        var errorResult = System.Text.Json.JsonSerializer.Deserialize<AuthResponse>(contentStr,
                            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        return errorResult ?? new AuthResponse { Success = false, Message = "Gagal login." };
                    }
                    catch
                    {
                        // Kalau gagal parse, berarti errornya teks biasa
                        return new AuthResponse { Success = false, Message = $"Error: {contentStr}" };
                    }
                }

                // 4. Jika Sukses (200 OK), baru Deserialize dengan aman
                var result = System.Text.Json.JsonSerializer.Deserialize<AuthResponse>(contentStr,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result != null && result.Success)
                {
                    await _localStorage.SetItemAsync("authToken", result.Token);
                    ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
                }

                return result!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CLIENT EXCEPTION]: {ex.Message}");
                return new AuthResponse { Success = false, Message = "Gagal terhubung ke server." };
            }
        }

        public async Task<AuthResponse> RegisterAsync(RegisterDTO model)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", model);
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");

            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
        }
    }
}
