namespace MSEstatesAppLibrary.Models;

public class LocationModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set;}

    public string? Location { get; set; }
    
    public string? Acronym { get; set; }
}