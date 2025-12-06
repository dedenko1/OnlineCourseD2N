using Microsoft.JSInterop;
using OnlineCourseD2N.Shared.Services;

namespace OnlineCourseD2N.Web.Services
{
    public class TextToSpeechServiceWeb : ITextToSpeechService
    {
        private readonly IJSRuntime _js;

        // Inject IJSRuntime untuk komunikasi ke Browser
        public TextToSpeechServiceWeb(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SpeakAsync(string text)
        {
            // Panggil fungsi JS 'ttsInterop.speak'
            await _js.InvokeVoidAsync("ttsInterop.speak", text);
        }

        public async Task CancelAsync()
        {
            // Panggil fungsi JS 'ttsInterop.cancel'
            await _js.InvokeVoidAsync("ttsInterop.cancel");
        }
    }
}
