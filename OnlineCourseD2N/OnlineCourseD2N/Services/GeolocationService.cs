using OnlineCourseD2N.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace OnlineCourseD2N.Services
{
    public class GeolocationService : IGeolocationService
    {
        public async Task<(double Latitude, double Longitude)?> GetCurrentLocationAsync() {
            try
            {
                // Cek Izin
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }

                if (status == PermissionStatus.Granted)
                {
                    // Ambil Lokasi
                    var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                    var location = await Geolocation.Default.GetLocationAsync(request);

                    if (location != null)
                        return (location.Latitude, location.Longitude);

                    // Coba ambil lokasi terakhir (Cached)
                    location = await Geolocation.Default.GetLastKnownLocationAsync();
                    if (location != null)
                        return (location.Latitude, location.Longitude);
                }
            }
            catch (Exception ex)
            {
                // Bisa log error di sini jika perlu
                Console.WriteLine($"Error GPS: {ex.Message}");
            }

            return null; // Gagal mendapatkan lokasi
        }
    }
}
