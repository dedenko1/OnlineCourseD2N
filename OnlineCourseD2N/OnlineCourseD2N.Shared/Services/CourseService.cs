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
