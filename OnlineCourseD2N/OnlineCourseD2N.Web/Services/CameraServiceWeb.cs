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

        // ⭐ Fungsi untuk diekspos ke komponen UI agar dapat memulai JS
        public DotNetObjectReference<CameraServiceWeb> GetDotNetRef()
        {
            // Pastikan DotNetRef dibuat saat TakePhotoAsync dipanggil
            return _dotNetRef!;
        }

        public async Task<Stream?> TakePhotoAsync()
        {
            // 1. Inisialisasi sesi baru
            if (_tcs != null) { _tcs.SetCanceled(); }
            _tcs = new TaskCompletionSource<string?>();
            _dotNetRef = DotNetObjectReference.Create(this);

            try
            {
                // 2. Service HANYA MENUNGGU HASIL. UI yang akan memanggil JS startCapture.
                var base64 = await _tcs.Task;

                if (string.IsNullOrEmpty(base64) || base64 == "Cancelled")
                    return null;

                // 3. Konversi Base64 ke Stream
                var base64Data = base64.Split(',').Length > 1 ? base64.Split(',')[1] : base64;
                var bytes = Convert.FromBase64String(base64Data);
                return new MemoryStream(bytes);
            }
            catch (TaskCanceledException)
            {
                return null;
            }
            finally
            {
                // 4. Cleanup (panggilan stopCamera di Service dilakukan untuk berjaga-jaga)
                await _js.InvokeVoidAsync("cameraInterop.stopCamera");
                _dotNetRef?.Dispose();
                _tcs = null;
            }
        }

        // ⭐ CALLBACK DARI JAVASCRIPT: Foto berhasil diambil
        [JSInvokable]
        public void PhotoCaptured(string base64Image)
        {
            _tcs?.SetResult(base64Image);
        }

        // ⭐ CALLBACK DARI JAVASCRIPT: Proses dibatalkan/gagal
        [JSInvokable]
        public void CaptureCancelled()
        {
            _tcs?.SetResult("Cancelled");
        }

        public void Dispose()
        {
            _dotNetRef?.Dispose();
        }
    }
}