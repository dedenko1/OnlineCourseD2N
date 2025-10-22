using OnlineCourseD2N.Shared.Services;
using OnlineCourseD2N.Web.Components;
using OnlineCourseD2N.Web.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add device-specific services used by the OnlineCourseD2N.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5034/") });
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<TrainerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(OnlineCourseD2N.Shared._Imports).Assembly,
        typeof(OnlineCourseD2N.Web.Client._Imports).Assembly);

app.Run();
