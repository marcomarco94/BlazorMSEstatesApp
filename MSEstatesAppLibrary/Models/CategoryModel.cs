namespace MSEstatesAppLibrary.Models;

public class CategoryModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Category { get; set; }
    public string? Acronym { get; set; }
    public string? Image { get; set; }
}