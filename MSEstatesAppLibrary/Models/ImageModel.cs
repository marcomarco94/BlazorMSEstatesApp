namespace MSEstatesAppLibrary.Models;

public class ImageModel
{
    public ImageModel()
    {
        string guid = Guid.NewGuid().ToString();
        NameOriginal = guid + "_org.jpeg";
        NameResizedImageData = guid + ".jpeg";
        NameNoLogoImageData = guid + "_noLogo.jpeg";
    }

    public string? NameOriginal { get; set; }
    public string? NameResizedImageData { get; set; }
    public string? NameNoLogoImageData { get; set; }
    public byte[]? NoLogoImageData { get; set; }
    public byte[]? ResizedImageData { get; set; }
    public byte[]? OriginalImageData { get; set; }
}