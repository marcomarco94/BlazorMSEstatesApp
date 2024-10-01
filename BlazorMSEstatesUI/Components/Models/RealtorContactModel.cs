using System.ComponentModel.DataAnnotations;

namespace BlazorMSEstatesUI.Components.Models;

public class RealtorContactModel
{
    public string? Id { get; set; }
    [Required] public string? Name { get; set; }

    public string? Number { get; set; }

    [Required] [EmailAddress] public string? Email { get; set; }

    [Required] public string? Message { get; set; }

    public string? Token { get; set; } = "ContactForm";
}