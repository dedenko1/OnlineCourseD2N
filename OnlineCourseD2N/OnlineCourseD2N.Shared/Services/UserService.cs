using OnlineCourseD2N.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourseD2N.Shared.Services
{
    public class UserService
    {
        private readonly HttpClient _http;

        public UserService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<UsersDTO>> GetAllUsersAsync()
        {
            try
            {
                // [KHUSUS MAUI/WINDOWS] 
                // Kita perlu tempel token manual jika HttpClient tidak otomatis membawanya.
                // Jika kamu sudah pakai DelegatingHandler di MauiProgram, bagian ini bisa dihapus.
                /*
                var token = await SecureStorage.GetAsync("auth_token");
                if (!string.IsNullOrEmpty(token))
                {
                    _http.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("Bearer", token);
                }
                */

                // Panggil API
                return await _http.GetFromJsonAsync<List<UsersDTO>>("api/users") ?? new List<UsersDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService Error] Gagal ambil data: {ex.Message}");
                return new List<UsersDTO>(); // Return list kosong biar gak crash
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                // Tempel token juga disini jika perlu (sama seperti di atas)

                var result = await _http.DeleteAsync($"api/users/{id}");
                return result.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService Error] Gagal hapus: {ex.Message}");
                return false;
            }
        }
    }
}
