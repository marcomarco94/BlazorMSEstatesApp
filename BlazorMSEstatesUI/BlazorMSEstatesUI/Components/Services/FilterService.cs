
namespace BlazorMSEstatesUI.Client.Services;

public class FilterService
{
    private readonly ILocalStorageService _localStorage;
    private int? _maxPriceInput;

    private int? _minPriceInput;
    private string? _searchInput;
    private string? _selectedAdvertisement;

    private string? _selectedCategory;
    private string? _selectedLocation;
    private string? _sortedBy;

    public FilterService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public string? SelectedAdvertisement
    {
        get => _selectedAdvertisement ?? "All";
        set
        {
            if (_selectedAdvertisement != value)
            {
                _selectedAdvertisement = value;
                _localStorage.SetItemAsync(nameof(SelectedAdvertisement), value);
                NotifySearchFilterChanged();
            }
        }
    }

    public string? SelectedCategory
    {
        get => _selectedCategory ?? "Category";
        set
        {
            if (_selectedCategory != value)
            {
                _selectedCategory = value;
                _localStorage.SetItemAsync(nameof(SelectedCategory), value);
                NotifySearchFilterChanged();
            }
        }
    }

    public string? SelectedLocation
    {
        get => _selectedLocation ?? "Location";
        set
        {
            if (_selectedLocation != value)
            {
                _selectedLocation = value;
                _localStorage.SetItemAsync(nameof(SelectedLocation), value);
                NotifySearchFilterChanged();
            }
        }
    }

    public string? SearchInput
    {
        get => _searchInput ?? "";
        set
        {
            if (_searchInput != value)
            {
                _searchInput = value;
                _localStorage.SetItemAsync(nameof(SearchInput), value);
                NotifySearchFilterChanged();
            }
        }
    }

    public string? SortedBy
    {
        get => _sortedBy ?? "Price - Low to High";

        set
        {
            if (_sortedBy != value)
            {
                _sortedBy = value;
                _localStorage.SetItemAsync(nameof(SortedBy), value);
                NotifySearchFilterChanged();
            }
        }
    }

    public int? MinPriceInput
    {
        get => _minPriceInput ?? 0;
        set
        {
            if (_minPriceInput != value)
            {
                _minPriceInput = value;
                _localStorage.SetItemAsync(nameof(MinPriceInput), value);
                NotifySearchFilterChanged();
            }
        }
    }

    public int? MaxPriceInput
    {
        get => _maxPriceInput ?? 0;
        set
        {
            if (_maxPriceInput != value)
            {
                _maxPriceInput = value;
                _localStorage.SetItemAsync(nameof(MaxPriceInput), value);
                NotifySearchFilterChanged();
            }
        }
    }

    public event Action OnSearchFilterChanged;

    public async Task LoadFromLocalStorage()
    {
        _selectedCategory = await _localStorage.GetItemAsync<string>(nameof(SelectedCategory)) ?? "Category";
        _selectedLocation = await _localStorage.GetItemAsync<string>(nameof(SelectedLocation)) ?? "Location";
        _selectedAdvertisement = await _localStorage.GetItemAsync<string>(nameof(SelectedAdvertisement)) ?? "All";
        _minPriceInput = await _localStorage.GetItemAsync<int>(nameof(MinPriceInput));
        _maxPriceInput = await _localStorage.GetItemAsync<int>(nameof(MaxPriceInput));
        _searchInput = await _localStorage.GetItemAsync<string>(nameof(SearchInput)) ?? "";
        _sortedBy = await _localStorage.GetItemAsync<string>(nameof(SortedBy)) ?? "Price - Low to High";
    }

    private void NotifySearchFilterChanged()
    {
        OnSearchFilterChanged?.Invoke();
    }
}