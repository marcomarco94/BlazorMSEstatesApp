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
        
        builder.Services.AddSingleton(sp => new HttpClient
        {
            BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiUrl")),
            DefaultRequestHeaders =
            {
                { "Origin", "https://as-ms-estates-webapp.azurewebsites.net" }
            }
        });
        
        builder.Services.AddHostedService<CacheBackgroundSerivce>();
        builder.Services.AddScoped<FilterService>();
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<CacheService>(); 
    }
}