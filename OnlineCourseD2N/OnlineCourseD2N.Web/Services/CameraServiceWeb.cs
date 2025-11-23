using Microsoft.JSInterop;
using OnlineCourseD2N.Shared.Services;

namespace OnlineCourseD2N.Web.Services
{
    public class CameraServiceWeb : ICameraService
    {
        private readonly IJSRuntime _js;

        public CameraServiceWeb(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<Stream?> TakePhotoAsync()
        {
            var base64 = await _js.InvokeAsync<string>("camera.capturePhoto");

            if (string.IsNullOrEmpty(base64))
                return null;

            var bytes = Convert.FromBase64String(base64.Split(',')[1]);
            return new MemoryStream(bytes);
        }
    }
}
