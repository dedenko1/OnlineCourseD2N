using OnlineCourseD2N.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourseD2N.Services
{
    public class TextToSpeechService : ITextToSpeechService
    {
        public async Task SpeakAsync(string text)
        {
            var settings = new SpeechOptions()
            {
                Volume = 1.0f,
                Pitch = 1.0f
            };

            // TextToSpeech ini berasal dari namespace Microsoft.Maui.Media
            await TextToSpeech.Default.SpeakAsync(text, settings);
        }
    }
}
