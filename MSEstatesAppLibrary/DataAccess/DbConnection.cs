using System.Security.Authentication;
using Microsoft.Extensions.Configuration;
using MSEstatesAppLibrary.Models;

namespace MSEstatesAppLibrary.DataAccess;

public class DbConnection : IDbConnection
{
    private readonly string? _connectionId = "MongoDB";

    public DbConnection(IConfiguration config)
    {
        _connectionId = config.GetConnectionString(_connectionId);

        var settings = MongoClientSettings.FromUrl(
            new MongoUrl(_connectionId)
        );

        settings.SslSettings =
            new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };
        Client = new MongoClient(settings);

        DbName = config["DatabaseName"];

        var db = Client.GetDatabase(DbName);
        CategoryCollection = db.GetCollection<CategoryModel>(CategoryCollectionName);
        LocationCollection = db.GetCollection<LocationModel>(LocationCollectionName);
        ListingCollection = db.GetCollection<ListingModel>(ListingCollectionName);
        CompanyCollection = db.GetCollection<CompanyModel>(CompanyCollectionName);
        AgentCollection = db.GetCollection<AgentModel>(AgentCollectionName);
        AdvertisementTypeCollection = db.GetCollection<AdvertisementTypeModel>(AdvertisementTypeCollectionName);
        RealtorContactCollection = db.GetCollection<RealtorContactModel>(RealtorContactCollectionName);
        FacebookPostCollection = db.GetCollection<FacebookTemplateModel>(FacebookPostCollectionName);
        FacebookTemplateCollection = db.GetCollection<FacebookTemplateModel>(FacebookTemplateCollectionName);
        FacebookGroupCollection = db.GetCollection<FacebookGroupModel>(FacebookGroupCollectionName);
    }

    public string FacebookGroupCollectionName { get; } = "facebookGroups";
    public string? DbName { get; }
    public string CategoryCollectionName { get; } = "categories";
    public string LocationCollectionName { get; } = "locations";
    public string ListingCollectionName { get; } = "listings";
    public string CompanyCollectionName { get; } = "company";
    public string AgentCollectionName { get; } = "agents";
    public string AdvertisementTypeCollectionName { get; } = "advertisementTypes";
    public string RealtorContactCollectionName { get; } = "realtorContacts";
    public string FacebookPostCollectionName { get; } = "facebookPosts";
    public string FacebookTemplateCollectionName { get; } = "facebookTemplates";
    public MongoClient Client { get; }
    public IMongoCollection<CategoryModel> CategoryCollection { get; }
    public IMongoCollection<LocationModel> LocationCollection { get; }
    public IMongoCollection<ListingModel> ListingCollection { get; }
    public IMongoCollection<CompanyModel> CompanyCollection { get; }
    public IMongoCollection<AgentModel> AgentCollection { get; }
    public IMongoCollection<AdvertisementTypeModel> AdvertisementTypeCollection { get; }
    public IMongoCollection<RealtorContactModel> RealtorContactCollection { get; }
    public IMongoCollection<FacebookTemplateModel> FacebookPostCollection { get; }
    public IMongoCollection<FacebookTemplateModel> FacebookTemplateCollection { get; }
    public IMongoCollection<FacebookGroupModel> FacebookGroupCollection { get; }
}