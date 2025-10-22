using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OnlineCourseD2N.Shared.Services;
using OnlineCourseD2N.Web.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add device-specific services used by the OnlineCourseD2N.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

await builder.Build().RunAsync();
