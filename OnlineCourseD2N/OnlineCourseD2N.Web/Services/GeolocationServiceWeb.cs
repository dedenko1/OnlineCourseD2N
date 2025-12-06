using Microsoft.JSInterop;
using OnlineCourseD2N.Shared.Services;

namespace OnlineCourseD2N.Web.Services
{
    public class GeolocationServiceWeb : IGeolocationService
    {
        private readonly IJSRuntime _js;

        public GeolocationServiceWeb(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<(double Latitude, double Longitude)?> GetCurrentLocationAsync()
        {
            try
            {
                // Panggil fungsi JS yang kita buat tadi
                var result = await _js.InvokeAsync<GeolocationResult?>("geolocationInterop.getCurrentPosition");

                if (result != null)
                {
                    return (result.Latitude, result.Longitude);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Geolocation: {ex.Message}");
            }

            return null; // Return null jika gagal/ditolak user
        }

        // Class kecil untuk menampung hasil JSON dari JS
        private class GeolocationResult
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}
