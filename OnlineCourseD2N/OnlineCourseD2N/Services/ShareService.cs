using OnlineCourseD2N.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourseD2N.Services
{
    public class ShareService : IShareService
    {
        public async Task ShareTextAsync(string title, string text, string url)
        {
            if (string.IsNullOrEmpty(text)) return;

            // Membungkus data yang akan dibagikan
            var request = new ShareTextRequest
            {
                Title = title,       // Judul (muncul di beberapa OS)
                Text = text,         // Pesan utama
                Uri = url,           // Link (otomatis jadi preview di WA)
                Subject = title      // Subjek (khusus Email)
            };

            // Panggil UI Native Share
            await Share.Default.RequestAsync(request);
        }
    }
}
