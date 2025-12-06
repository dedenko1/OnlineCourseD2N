using Microsoft.JSInterop;
using OnlineCourseD2N.Shared.Services;

namespace OnlineCourseD2N.Web.Services
{
    public class MapServiceWeb : IMapService
    {
        private readonly IJSRuntime _js;

        // Kita butuh JS Runtime untuk membuka tab baru
        public MapServiceWeb(IJSRuntime js)
        {
            _js = js;
        }

        public async Task OpenMapAsync(double latitude, double longitude, string name)
        {
            // Format URL Google Maps
            var url = $"https://www.google.com/maps/search/?api=1&query={latitude.ToString().Replace(",", ".")},{longitude.ToString().Replace(",", ".")}";

            // Buka di tab baru ("_blank")
            await _js.InvokeVoidAsync("open", url, "_blank");
        }
    }
}
