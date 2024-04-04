namespace MSEstatesAppLibrary.Models;

public class RealtorContactModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; private set;}

    public string? Name { get; set; }
    public string? Number { get; set; }
    public string? Email { get; set; }
    public string? Message { get; set; }
    public string? Token { get; set; } = "ContactForm";
    public bool? Archived { get; set; } = false;
    public DateTime? DateCreated { get; set; } = DateTime.UtcNow;
}