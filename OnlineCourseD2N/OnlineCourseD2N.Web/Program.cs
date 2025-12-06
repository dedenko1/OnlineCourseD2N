using OnlineCourseD2N.Shared.Services;
using OnlineCourseD2N.Web.Components;
using OnlineCourseD2N.Web.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddHubOptions(options =>
    {
        // Set batas maksimal pesan jadi 10MB (Defaultnya kecil banget)
        options.MaximumReceiveMessageSize = 10 * 1024 * 1024;
    })
    .AddInteractiveWebAssemblyComponents();

// Add device-specific services used by the OnlineCourseD2N.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5034/") });
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<TrainerService>();
builder.Services.AddScoped<ICameraService, CameraServiceWeb>();
builder.Services.AddScoped<IShareService, ShareServiceWeb>();
builder.Services.AddScoped<IMapService, MapServiceWeb>();
builder.Services.AddScoped<IGeolocationService, GeolocationServiceWeb>();
builder.Services.AddScoped<IConnectivityService, ConnectivityServiceWeb>();
builder.Services.AddScoped<ITextToSpeechService, TextToSpeechServiceWeb>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.MapStaticAssets();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(OnlineCourseD2N.Shared._Imports).Assembly,
        typeof(OnlineCourseD2N.Web.Client._Imports).Assembly);

app.Run();
