using Microsoft.Extensions.Logging;
using OnlineCourseD2N.Services;
using OnlineCourseD2N.Shared.Services;

namespace OnlineCourseD2N
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddSingleton<IFormFactor, FormFactor>();
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<ICameraService, CameraService>();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            builder.Services.AddScoped<CourseService>();
            builder.Services.AddScoped<TrainerService>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5034/") });
            return builder.Build();
        }
    }
}