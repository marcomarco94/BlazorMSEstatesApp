namespace MSEstatesAppLibrary.Models;

public class FacebookTemplateModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set;}
    public string? ListingId { get; set; }
    public string? Name { get; set; }
    public string? Template { get; set; }
    public DateTime? DateCreated { get; set; } = DateTime.UtcNow;
}
