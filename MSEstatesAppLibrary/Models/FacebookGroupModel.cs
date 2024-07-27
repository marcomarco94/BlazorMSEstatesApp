namespace MSEstatesAppLibrary.Models;

public class FacebookGroupModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get;  set;}
    public string? GroupName { get; set; }
}