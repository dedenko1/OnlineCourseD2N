using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using OnlineCourseD2N.Shared.Models;

namespace OnlineCourseD2N.Shared.Services
{
    public class TrainerService
    {
        private readonly HttpClient _http;

        public TrainerService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Trainer>> GetAllAsync()
            => await _http.GetFromJsonAsync<List<Trainer>>("api/trainers") ?? [];

        public async Task<Trainer?> GetByIdAsync(int id)
            => await _http.GetFromJsonAsync<Trainer>($"api/trainers/{id}");

        public async Task<bool> AddAsync(Trainer trainer)
        {
            Console.WriteLine($"DEBUG: Sending POST to api/trainers with data {trainer.Name}");
            var response = await _http.PostAsJsonAsync("api/trainers", trainer);
            Console.WriteLine($"DEBUG: Response status = {response.StatusCode}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, Trainer trainer)
        {
            var response = await _http.PutAsJsonAsync($"api/trainers/{id}", trainer);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/trainers/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
