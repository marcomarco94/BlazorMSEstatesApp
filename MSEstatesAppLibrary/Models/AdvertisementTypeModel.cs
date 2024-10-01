namespace MSEstatesAppLibrary.Models;

public class AdvertisementTypeModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Acronym { get; set; }
    public string? AdvertisementType { get; set; }
}