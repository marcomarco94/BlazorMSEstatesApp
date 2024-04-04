namespace MSEstatesAppLibrary.Models;

public class AgentModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; private set;}

    public string? AgentId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MailAddress { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Facebook { get; set; }
    public string? Address { get; set; }
    public string? Image { get; set; }
    public bool? Active { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}