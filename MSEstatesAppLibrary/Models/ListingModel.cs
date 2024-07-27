namespace MSEstatesAppLibrary.Models;
public class ListingModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set;}
    public string? Token { get; set; }
    public string? ListingName { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public int? Bathrooms { get; set; }
    public int? Bedrooms { get; set; }
    public int? Price { get; set; }
    public int? Size { get; set; }
    public List<string>? Features { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public LocationModel? Location { get; set; }
    public CategoryModel? Category { get; set; }
    public AdvertisementTypeModel? AdvertisementType { get; set; }
    public List<string?>? ImageUrls { get; set; }
    public bool Archived { get; set; } = false;
}