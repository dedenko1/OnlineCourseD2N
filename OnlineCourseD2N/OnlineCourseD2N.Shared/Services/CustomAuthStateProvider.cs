using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace OnlineCourseD2N.Shared.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;

        public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient http)
        {
            _localStorage = localStorage;
            _http = http;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Default: User dianggap Belum Login (Anonymous)
            var identity = new ClaimsIdentity();
            var anonymousState = new AuthenticationState(new ClaimsPrincipal(identity));

            try
            {
                // 👇👇 PASANG JEBAKAN TRY-CATCH DISINI 👇👇

                // Coba akses LocalStorage
                string token = await _localStorage.GetItemAsStringAsync("authToken");

                if (!string.IsNullOrEmpty(token))
                {
                    // Bersihkan tanda kutip jika ada (bug umum blazored)
                    token = token.Replace("\"", "");

                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);

                    // Cek apakah token expired?
                    if (jwtToken.ValidTo < DateTime.UtcNow)
                    {
                        await _localStorage.RemoveItemAsync("authToken");
                        return anonymousState;
                    }

                    var claims = jwtToken.Claims;
                    identity = new ClaimsIdentity(claims, "JwtAuth");

                    _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch (InvalidOperationException)
            {
                // INI KUNCINYA:
                // Jika error "JavaScript interop calls cannot be issued...",
                // berarti kita sedang di Server (Prerendering).
                // Abaikan saja, kembalikan status "Belum Login".
            }
            catch (Exception)
            {
                // Error lain (misal token rusak), abaikan juga.
            }

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        public void NotifyUserAuthentication(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims;
            var identity = new ClaimsIdentity(claims, "JwtAuth");
            var user = new ClaimsPrincipal(identity);

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            _http.DefaultRequestHeaders.Authorization = null;
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}