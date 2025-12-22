using OnlineCourseD2N.Shared.Services;

namespace OnlineCourseD2N.Web.Services
{
    public class GeocodingServiceWeb : IGeocodingService
    {
        public Task<string?> GetAddressFromCoordinatesAsync(double latitude, double longitude)
        {
            // Karena di Web reverse geocoding itu bayar/ribet,
            // Kita return koordinatnya aja sebagai fallback biar gak error.
            return Task.FromResult<string?>($"Lokasi Web: {latitude:0.####}, {longitude:0.####}");
        }
    }
}
