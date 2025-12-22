using Microsoft.JSInterop; // Tambahkan ini
using OnlineCourseD2N.Shared.Services;

namespace OnlineCourseD2N.Web.Services
{
    public class WebBatteryService : IBatteryService
    {
        private readonly IJSRuntime _js;

        // Inject IJSRuntime
        public WebBatteryService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<double> CheckLevel()
        {
            try
            {
                // Panggil fungsi JS yang kita buat tadi
                return await _js.InvokeAsync<double>("getBatteryLevel");
            }
            catch
            {
                return 1.0; // Kalau error, anggap 100%
            }
        }

        // Sisanya biarkan dummy (karena deteksi charging/state di web lebih ribet)
        public string CheckState() => "Unknown";
        public string CheckPowerSource() => "AC";
    }
}