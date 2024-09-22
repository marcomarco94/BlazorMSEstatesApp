namespace BlazorMSEstatesUI.Components.Models;

public class ListingModel
{
    public string? Id { get; set; } 
    public string? Token { get; set; }
    public string? ListingName { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public int? Bathrooms { get; set; }
    public int? Bedrooms { get; set; }
    public int? Price { get; set; }
    public int? Size { get; set; }
    public string? DateCreated { get; set; }
    public LocationModel? Location { get; set; } = new();
    public CategoryModel? Category { get; set; } = new();
    public AdvertisementTypeModel? AdvertisementType { get; set; }
    public List<string>? Features { get; set; } = new();
    public List<string> ImageUrls { get; set; } = new();
    public List<string> ImageUrlsOrg { get; set; } = new();
    public bool? Archived { get; set; }
}