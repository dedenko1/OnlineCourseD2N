using Microsoft.AspNetCore.Components.Forms;
using OnlineCourseD2N.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;

namespace OnlineCourseD2N.Shared.Services
{
    public class TrainerService
    {
        private readonly HttpClient _http;

        public TrainerService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string?> UploadCoverAsync(IBrowserFile file, string? oldFileName = null)
        {
            try
            {
                var content = new MultipartFormDataContent();
                var stream = file.OpenReadStream(10 * 1024 * 1024);
                content.Add(new StreamContent(stream), "file", file.Name);

                if (!string.IsNullOrEmpty(oldFileName))
                {
                    content.Add(new StringContent(oldFileName), "oldFileName");
                }

                var response = await _http.PostAsync("api/uploads/uploadfile", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Upload error: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> UploadStreamAsync(Stream fileStream, string? oldFileName = null)
        {
            try
            {
                if (fileStream == null || fileStream.Length == 0)
                    return null;

                using var content = new MultipartFormDataContent();
                string fileName = $"{Guid.NewGuid()}.png";

                var streamContent = new StreamContent(fileStream);
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

                content.Add(streamContent, "file", fileName);

                if (!string.IsNullOrEmpty(oldFileName))
                {
                    content.Add(new StringContent(oldFileName), "oldFileName");
                }

                var response = await _http.PostAsync("api/uploads/uploadfile", content);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Upload failed: {response.StatusCode}");
                    return null;
                }

                string uploadedFileName = await response.Content.ReadAsStringAsync();
                return uploadedFileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Upload error: {ex.Message}");
                return null;
            }
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
