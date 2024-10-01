using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;
using SkiaSharp;

namespace MSEstatesAppLibrary.Services;

public class ListingService
{
    private readonly AzureBlobService _azureBlobService;
    private readonly string? _imageBasePath;
    private readonly IListingData _listingData;

    public ListingService(IConfiguration config, AzureBlobService azureBlobService, IListingData listingData)
    {
        _azureBlobService = azureBlobService;
        _imageBasePath = config.GetSection("ImgBasePath").Value;
        _listingData = listingData;
    }

    public async Task<List<string>> GetFullUrlsByListingId(string? id)
    {
        var listing = await _listingData.GetListingById(id);
        var fullUrls = new List<string>();

        if (listing?.ImageUrls != null)
            foreach (var imageUrl in listing.ImageUrls)
            {
                var imageUrlExtension = Path.GetExtension(imageUrl);
                var imageUrlWithoutExtension = Path.GetFileNameWithoutExtension(imageUrl);
                var orgImageUrl = $"{_imageBasePath}{imageUrlWithoutExtension}_org{imageUrlExtension}";
                fullUrls.Add(orgImageUrl);
            }

        return fullUrls;
    }

    public async Task UpdateImagesOnCloud(List<string?>? oldImageUrls, List<string?>? newImageUrls)
    {
        if (oldImageUrls != null && newImageUrls != null)
        {
            var deletedImageUrls = oldImageUrls.Except(newImageUrls).ToList();

            foreach (var imageUrl in deletedImageUrls)
            {
                if (imageUrl == null) continue;
                await _azureBlobService.DeleteFileBlobAsync(imageUrl);
                var imageUrlWithoutExtension = Path.GetFileNameWithoutExtension(imageUrl);

                var imageUrlExtension = Path.GetExtension(imageUrl);
                var orgImageUrl = $"{imageUrlWithoutExtension}_org{imageUrlExtension}";
                await _azureBlobService.DeleteFileBlobAsync(orgImageUrl);

                var noLogoImageUrl = $"{imageUrlWithoutExtension}_noLogo{imageUrlExtension}";
                await _azureBlobService.DeleteFileBlobAsync(noLogoImageUrl);
            }
        }
    }

    public async Task<List<string?>> ProcessAndUploadImages(IFormFileCollection files, ListingModel listing)
    {
        var tasks = files.Select(async file =>
        {
            if (file.Length <= 0) return null;
            await using var stream = file.OpenReadStream();
            var imageUrl = await ProcessAndUploadImage(stream);
            return imageUrl;
        }).ToList();

        var imageUrls = await Task.WhenAll(tasks);
        return imageUrls.ToList();
    }

    public async Task<string?> ProcessAndUploadImage(Stream imageStream)
    {
        var imageWithNoLogoData = ConvertStreamToByteArray(imageStream);
        imageStream.Position = 0;
        var originalImageData = AddLogoToImage(imageStream);
        var resizedImageData = ResizeImage(originalImageData);

        var processedImage = new ImageModel
        {
            NoLogoImageData = imageWithNoLogoData,
            OriginalImageData = originalImageData,
            ResizedImageData = resizedImageData
        };

        await _azureBlobService.UploadFileToBlobAsync(processedImage.NameNoLogoImageData,
            processedImage.NoLogoImageData);
        await _azureBlobService.UploadFileToBlobAsync(processedImage.NameOriginal, processedImage.OriginalImageData);
        await _azureBlobService.UploadFileToBlobAsync(processedImage.NameResizedImageData,
            processedImage.ResizedImageData);

        return processedImage.NameResizedImageData;
    }

    private byte[] ConvertStreamToByteArray(Stream stream)
    {
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            stream.Position = 0;
            return memoryStream.ToArray();
        }
    }

    private byte[] AddLogoToImage(Stream imageStream)
    {
        var logoResourcePath = "MSEstatesAppLibrary.Resources.Images.logo_white.png";

        var assembly = typeof(ListingService).Assembly;
        using var originalBitmap = SKBitmap.Decode(imageStream);
        using var logoStream = assembly.GetManifestResourceStream(logoResourcePath);
        using var logoBitmap = SKBitmap.Decode(logoStream);
        using var surface = SKSurface.Create(originalBitmap.Info);
        var canvas = surface.Canvas;
        canvas.DrawBitmap(originalBitmap, SKPoint.Empty);
        var logoSize = originalBitmap.Width * 0.08f;
        var ratio = originalBitmap.Width / (float)originalBitmap.Height;
        var x = originalBitmap.Width * 0.04f;
        var y = originalBitmap.Height * (0.04f * ratio);

        using (var paint = new SKPaint())
        {
            paint.FilterQuality = SKFilterQuality.High;
            canvas.DrawBitmap(logoBitmap, SKRect.Create(x, y, logoSize, logoSize), paint);

            var watermarkSize = originalBitmap.Height * 0.5f;
            var watermarkX = (originalBitmap.Width - watermarkSize) / 2.0f;
            var watermarkY = (originalBitmap.Height - watermarkSize) / 2.0f;

            paint.ColorFilter = SKColorFilter.CreateBlendMode(SKColors.White.WithAlpha(40), SKBlendMode.SrcIn);
            canvas.DrawBitmap(logoBitmap,
                SKRect.Create(watermarkX, watermarkY, watermarkSize, watermarkSize), paint);
        }

        using (var image = surface.Snapshot())
        using (var data = image.Encode(SKEncodedImageFormat.Jpeg, 100))
        {
            var byteArray = data.ToArray();
            return byteArray;
        }
    }

    private byte[] ResizeImage(byte[] originalImageData)
    {
        using (var originalBitmap = SKBitmap.Decode(originalImageData))
        {
            var newWidth = 800;
            var newHeight = originalBitmap.Height * newWidth / originalBitmap.Width;

            using (var resizedBitmap =
                   originalBitmap.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.High))
            {
                using (var resizedImage = SKImage.FromBitmap(resizedBitmap))
                using (var resizedData = resizedImage.Encode(SKEncodedImageFormat.Jpeg, 95))
                {
                    var resizedByteArray = resizedData.ToArray();
                    return resizedByteArray;
                }
            }
        }
    }
}