using Microsoft.JSInterop;
using OnlineCourseD2N.Shared.Services;

namespace OnlineCourseD2N.Web.Services
{
    public class ShareServiceWeb : IShareService
    {
        private readonly IJSRuntime _js;

        public ShareServiceWeb(IJSRuntime js)
        {
            _js = js;
        }

        public async Task ShareTextAsync(string title, string text, string url)
        {
            // Panggil JS shareInterop.share
            await _js.InvokeVoidAsync("shareInterop.share", title, text, url);
        }
    }
}
