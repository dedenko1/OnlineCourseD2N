using Microsoft.JSInterop;
using OnlineCourseD2N.Shared.Services;

namespace OnlineCourseD2N.Web.Services
{
    public class ScreenshotServiceWeb : IScreenshotService
    {
        private readonly IJSRuntime _js;

        public ScreenshotServiceWeb(IJSRuntime js)
        {
            _js = js;
        }

        public async Task CaptureAndShareAsync()
        {
            // Panggil fungsi JS yang kita buat di langkah 2
            await _js.InvokeVoidAsync("webScreenshot.takeAndDownload");
        }
    }
}
