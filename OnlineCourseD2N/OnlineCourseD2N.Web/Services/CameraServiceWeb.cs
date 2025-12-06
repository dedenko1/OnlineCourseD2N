using Microsoft.JSInterop;
using OnlineCourseD2N.Shared.Services;

namespace OnlineCourseD2N.Web.Services
{
    public class CameraServiceWeb : ICameraService, IDisposable
    {
        private readonly IJSRuntime _js;
        private DotNetObjectReference<CameraServiceWeb>? _dotNetRef;
        private TaskCompletionSource<string?>? _tcs;

        public CameraServiceWeb(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<Stream?> TakePhotoAsync()
        {
            // 1. Reset state lama
            if (_tcs != null) _tcs.TrySetCanceled();

            _tcs = new TaskCompletionSource<string?>();
            _dotNetRef = DotNetObjectReference.Create(this);

            try
            {
                // 2. PERUBAHAN UTAMA: 
                // Service ini yang menyuruh JS mulai, bukan UI.
                // Kita asumsikan ada elemen video dengan ID 'cameraVideoElement' di HTML
                bool started = await _js.InvokeAsync<bool>("cameraInterop.startCapture", _dotNetRef, "cameraVideoElement");

                if (!started)
                {
                    return null; // Gagal start (misal izin ditolak)
                }

                // 3. Tunggu sampai user klik tombol 'Capture' di UI (lewat JS Callback)
                var base64 = await _tcs.Task;

                if (string.IsNullOrEmpty(base64) || base64 == "Cancelled")
                    return null;

                // 4. Konversi ke Stream
                var base64Data = base64.Split(',').Length > 1 ? base64.Split(',')[1] : base64;
                var bytes = Convert.FromBase64String(base64Data);
                return new MemoryStream(bytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Camera Error: {ex.Message}");
                return null;
            }
            finally
            {
                // 5. Cleanup
                await _js.InvokeVoidAsync("cameraInterop.stopCamera", "cameraVideoElement");
                Dispose();
            }
        }

        // ... Callback methods tetap sama ...
        [JSInvokable]
        public void PhotoCaptured(string base64Image) => _tcs?.TrySetResult(base64Image);

        [JSInvokable]
        public void CaptureCancelled() => _tcs?.TrySetResult("Cancelled");

        public void Dispose()
        {
            _dotNetRef?.Dispose();
            _dotNetRef = null;
        }
    }
}