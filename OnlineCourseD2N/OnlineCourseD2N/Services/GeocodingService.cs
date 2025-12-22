using OnlineCourseD2N.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourseD2N.Services
{
    public class GeocodingService : IGeocodingService
    {
        public async Task<string?> GetAddressFromCoordinatesAsync(double latitude, double longitude)
        {
            try
            {
                // Menggunakan fitur bawaan MAUI
                var placemarks = await Geocoding.Default.GetPlacemarksAsync(latitude, longitude);

                var place = placemarks?.FirstOrDefault();

                if (place != null)
                {
                    var parts = new List<string?>
                    {
                        place.Thoroughfare,       // Nama Jalan
                        place.SubLocality,        // Kelurahan/Desa
                        place.Locality,           // Kota/Kabupaten
                        place.AdminArea,          // Provinsi
                        place.CountryName         // Negara
                    };

                    // Gabungkan koma, dan buang yang kosong (null)
                    return string.Join(", ", parts.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
            }
            catch (Exception ex)
            {
                // Geocoding butuh internet, kalau gagal return null aja
                Console.WriteLine($"Geocoding Error: {ex.Message}");
            }

            return null;
        }
    }
}
