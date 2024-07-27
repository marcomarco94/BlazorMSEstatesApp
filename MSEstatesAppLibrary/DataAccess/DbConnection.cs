using System.Security.Authentication;
using Microsoft.Extensions.Configuration;
using MSEstatesAppLibrary.Models;

namespace MSEstatesAppLibrary.DataAccess;

public class DbConnection : IDbConnection
{
    private readonly IConfiguration _config;
    private readonly string _connectionId = "MongoDB";
    private readonly IMongoDatabase _db;

    public DbConnection(IConfiguration config)
    {
        _config = config;
        _connectionId= _config.GetConnectionString(_connectionId);
        
        MongoClientSettings settings = MongoClientSettings.FromUrl(
            new MongoUrl(_connectionId)
        );
        
        settings.SslSettings = 
            new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
        Client = new MongoClient(settings);
        
        
        DbName = _config["DatabaseName"];
        _db = Client.GetDatabase(DbName);

        CategoryCollection = _db.GetCollection<CategoryModel>(CategoryCollectionName);
        LocationCollection = _db.GetCollection<LocationModel>(LocationCollectionName);
        ListingCollection = _db.GetCollection<ListingModel>(ListingCollectionName);
        CompanyCollection = _db.GetCollection<CompanyModel>(CompanyCollectionName);
        AgentCollection = _db.GetCollection<AgentModel>(AgentCollectionName);
        AdvertisementTypeCollection = _db.GetCollection<AdvertisementTypeModel>(AdvertisementTypeCollectionName);
        RealtorContactCollection = _db.GetCollection<RealtorContactModel>(RealtorContactCollectionName);
        FacebookPostCollection = _db.GetCollection<FacebookPostModel>(FacebookPostCollectionName);
        FacebookTemplateCollection = _db.GetCollection<FacebookTemplateModel>(FacebookTemplateCollectionName);
        FacebookGroupCollection = _db.GetCollection<FacebookGroupModel>(FacebookGroupCollectionName);
    }

    public string DbName { get; }
    public string CategoryCollectionName { get; } = "categories";
    public string LocationCollectionName { get; } = "locations";
    public string ListingCollectionName { get; } = "listings";
    public string CompanyCollectionName { get; } = "company";
    public string AgentCollectionName { get; } = "agents";
    public string AdvertisementTypeCollectionName { get; } = "advertisementTypes";
    public string RealtorContactCollectionName { get; } = "realtorContacts";
    public string FacebookPostCollectionName { get; } = "facebookPosts";
    public string FacebookTemplateCollectionName { get; } = "facebookTemplates";
    public string FacebookGroupCollectionName { get; } = "facebookGroups";
    public MongoClient Client { get; }
    public IMongoCollection<CategoryModel> CategoryCollection { get; }
    public IMongoCollection<LocationModel> LocationCollection { get; }
    public IMongoCollection<ListingModel> ListingCollection { get; }
    public IMongoCollection<CompanyModel> CompanyCollection { get; }
    public IMongoCollection<AgentModel> AgentCollection { get; }
    public IMongoCollection<AdvertisementTypeModel> AdvertisementTypeCollection { get; }
    public IMongoCollection<RealtorContactModel> RealtorContactCollection { get; }
    public IMongoCollection<FacebookPostModel> FacebookPostCollection { get; }
    public IMongoCollection<FacebookTemplateModel> FacebookTemplateCollection { get; }
    public IMongoCollection<FacebookGroupModel> FacebookGroupCollection { get; }
}