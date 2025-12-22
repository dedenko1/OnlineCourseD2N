using OnlineCourseD2N.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourseD2N.Services
{
    public class ScreenshorService : IScreenshotService
    {
        public async Task CaptureAndShareAsync()
        {
            if (Screenshot.Default.IsCaptureSupported)
            {
                // 1. Ambil Screenshot
                IScreenshotResult screen = await Screenshot.Default.CaptureAsync();

                if (screen != null)
                {
                    // 2. Baca stream gambar
                    Stream stream = await screen.OpenReadAsync();

                    // 3. Simpan ke File Sementara (Cache) agar bisa di-share
                    // Kita tidak bisa share Stream langsung ke WA, harus jadi file dulu.
                    var fileName = $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

                    using (var fileStream = File.Create(filePath))
                    {
                        await stream.CopyToAsync(fileStream);
                    }

                    // 4. Buka Native Share Sheet
                    await Share.Default.RequestAsync(new ShareFileRequest
                    {
                        Title = "Bagikan Screenshot",
                        File = new ShareFile(filePath)
                    });
                }
            }
        }
    }
}
