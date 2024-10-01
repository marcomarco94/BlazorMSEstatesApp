namespace MSEstatesAppLibrary.Models;

public class CompanyModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? ContactName { get; set; }
    public string? MailAddress { get; set; }
    public string? PhoneNumber { get; set; }
    public string? WhatsApp { get; set; }
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
    public string? Address { get; set; }
}