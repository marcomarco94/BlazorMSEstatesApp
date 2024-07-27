using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace MarketPlaceHelper.Services;

public class AzureBlobService
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;
    private readonly string _containerName;

    public AzureBlobService(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetSection("AzureBlob")["ConnectionString"];
        _containerName = _config.GetSection("AzureBlob")["ContainerName"];
    }
    
    public async Task UploadFileToBlobAsync(string? blobName, byte[] fileData)
    {
        try
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            using var uploadFileStream = new MemoryStream(fileData);
            await blobClient.UploadAsync(uploadFileStream, true);
        }
        catch (Exception e)
        {        
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteFileBlobAsync(string filePath)
    {
        try
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            var blobName = Path.GetFileName(filePath);
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<byte[]> DownloadFileBlobAsync(string filePath)
    {
        try
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            var blobName = $"{Path.GetFileNameWithoutExtension(filePath)}_org{Path.GetExtension(filePath)}";
            var blobClient = containerClient.GetBlobClient(blobName);

            BlobDownloadInfo download = await blobClient.DownloadAsync();
            using var ms = new MemoryStream();
            await download.Content.CopyToAsync(ms);
            return ms.ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}