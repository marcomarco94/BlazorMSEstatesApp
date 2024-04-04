using Blazored.LocalStorage;
using BlazorMSEstatesUI.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiUrl")),
    DefaultRequestHeaders =
    {
        { "Origin", "https://as-ms-estates-webapp.azurewebsites.net" }
    }
});




await builder.Build().RunAsync();

