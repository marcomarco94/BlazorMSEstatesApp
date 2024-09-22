namespace BlazorMSEstatesUIAdmin.Models;

public class FacebookPostModel
{
    public string? Id { get; set;}
    public string? ListingId { get; set; }
    public FacebookTemplateModel? Template { get; set; }
    public DateTime? DateCreated { get; set; } = DateTime.UtcNow;
}
