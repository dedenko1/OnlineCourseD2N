using OnlineCourseD2N.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourseD2N.Services
{
    public class MapService : IMapService
    {
        public async Task OpenMapAsync(double latitude, double longitude, string name)
        {
            // --- LOGIKA CABANG (HYBRID) ---

            // 1. Jika Windows, buka Browser Google Maps
            if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
            {
                // Format URL Google Maps
                // https://www.google.com/maps/search/?api=1&query=lat,long
                var url = $"https://www.google.com/maps/search/?api=1&query={latitude.ToString().Replace(",", ".")},{longitude.ToString().Replace(",", ".")}";

                await Launcher.Default.OpenAsync(url);
            }
            // 2. Jika Android/iOS, buka Aplikasi Native
            else
            {
                var location = new Location(latitude, longitude);
                var options = new MapLaunchOptions
                {
                    Name = name,
                    NavigationMode = NavigationMode.None
                };

                try
                {
                    await Map.Default.OpenAsync(location, options);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Gagal buka peta native: {ex.Message}");
                }
            }
        }
    }
}
