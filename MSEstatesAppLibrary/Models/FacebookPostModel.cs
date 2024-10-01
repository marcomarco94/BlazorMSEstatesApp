namespace MSEstatesAppLibrary.Models;

public class FacebookPostModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? ListingId { get; set; }
    public FacebookTemplateModel? Template { get; set; }
    public DateTime? DateCreated { get; set; } = DateTime.UtcNow;
}