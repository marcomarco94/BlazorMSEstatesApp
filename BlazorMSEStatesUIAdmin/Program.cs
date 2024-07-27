using System;
using System.Net.Http;
using BlazorMSEStatesUIAdmin;
using BlazorMSEstatesUIAdmin.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("WebAPI",
        client => client.BaseAddress = new Uri(builder.Configuration["WebAPI:BaseAddress"]))
    .AddHttpMessageHandler(sp =>
    {
        var handler = sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(
                authorizedUrls: new[] { builder.Configuration["WebAPI:BaseAddress"] },
                scopes: new[] { builder.Configuration["WebAPI:Scope"] });
        return handler;
    });

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("WebAPI"));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add(builder.Configuration["WebAPI:Scope"]);
});

builder.Services.AddFluentUIComponents();
builder.Services.AddSingleton<ImageService>();
builder.Services.AddSingleton<ListingStateService>();

await builder.Build().RunAsync();