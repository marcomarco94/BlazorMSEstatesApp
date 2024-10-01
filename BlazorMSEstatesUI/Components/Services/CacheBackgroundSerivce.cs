namespace BlazorMSEstatesUI.Components.Services;

public class CacheBackgroundSerivce : BackgroundService
{
    private readonly ApiService _apiService;
    private readonly TimeSpan _cacheDuration;
    private readonly CacheService _cacheService;
    private readonly ILogger<CacheBackgroundSerivce> _logger;

    public CacheBackgroundSerivce(ApiService apiService, CacheService cacheService,
        ILogger<CacheBackgroundSerivce> logger)
    {
        _apiService = apiService;
        _cacheDuration = TimeSpan.FromMinutes(10);
        _cacheService = cacheService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var listings = await _apiService.GetListings();
                _cacheService.Set(nameof(listings), listings);

                var categories = await _apiService.GetCategories();
                _cacheService.Set(nameof(categories), categories);

                var company = await _apiService.GetCompany();
                _cacheService.Set(nameof(company), company);

                var locations = await _apiService.GetLocations();
                _cacheService.Set(nameof(locations), locations);
            }
            catch (HttpRequestException ex)
            {
                // Log HTTP request exceptions
                _logger.LogError($"An error occurred while making an HTTP request: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log other exceptions
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
            }

            await Task.Delay(_cacheDuration, stoppingToken);
        }
    }
}