namespace BlazorMSEstatesUIAdmin.Models;

public class FacebookTemplateModel
{
    public string? Id { get; set; }
    public string? ListingId { get; set; }
    public string? Name { get; set; }
    public string? Template { get; set; }
    public DateTime? DateCreated { get; set; } = DateTime.UtcNow;
}