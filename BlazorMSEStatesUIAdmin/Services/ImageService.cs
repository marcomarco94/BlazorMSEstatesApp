using System.IO;

namespace BlazorMSEstatesUIAdmin.Services;

public class ImageService
{
    private const string ImageBaseUrl = "https://msestatesblob.blob.core.windows.net/blobmsestates/";

    public string GetFullImagePath(string? relativePath)
    {
        return ImageBaseUrl + relativePath;
    }

    public string GetFullImagePathHq(string? relativePath)
    {
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(relativePath);
        var extension = Path.GetExtension(relativePath);
        var newFileName = $"{fileNameWithoutExtension}_org{extension}";
        return Path.Combine(ImageBaseUrl, newFileName);
    }



}