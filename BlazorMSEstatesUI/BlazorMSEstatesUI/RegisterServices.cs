namespace BlazorMSEstatesUI;

public static class RegisterServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        builder.Services.AddMemoryCache();
        builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });
        
        builder.Services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiUrl")),
            DefaultRequestHeaders =
            {
                { "Origin", "https://as-ms-estates-webapp.azurewebsites.net" }
            }
        });
    
        builder.Services.AddSingleton<StateService>();
        builder.Services.AddScoped<FilterService>();
        builder.Services.AddScoped<ApiService>();
        builder.Services.AddBlazoredLocalStorage();

    }
}