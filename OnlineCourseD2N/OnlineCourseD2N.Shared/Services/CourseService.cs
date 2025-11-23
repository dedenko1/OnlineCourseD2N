using Microsoft.AspNetCore.Components.Forms;
using OnlineCourseD2N.Shared.Models;
using System.Net.Http.Json;

namespace OnlineCourseD2N.Shared.Services
{
    public class CourseService
    {
        private readonly HttpClient _http;

        public CourseService(HttpClient http)
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
                    return await response.Content.ReadAsStringAsync(); // nama file
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
                using var content = new MultipartFormDataContent();

                var streamContent = new StreamContent(fileStream);
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                content.Add(streamContent, "file", $"{Guid.NewGuid()}.png");

                if (!string.IsNullOrEmpty(oldFileName))
                {
                    content.Add(new StringContent(oldFileName), "oldFileName");
                }

                var response = await _http.PostAsync("api/uploads/uploadfile", content);

                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Upload error: {ex.Message}");
                return null;
            }
        }



        public async Task<List<Course>> GetAllAsync()
            => await _http.GetFromJsonAsync<List<Course>>("api/courses") ?? [];

        public async Task<Course?> GetByIdAsync(int id)
            => await _http.GetFromJsonAsync<Course>($"api/courses/{id}");

        public async Task<bool> AddAsync(Course course)
        {
            var response = await _http.PostAsJsonAsync("api/courses", course);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, Course course)
        {
            var response = await _http.PutAsJsonAsync($"api/courses/{id}", course);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/courses/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
