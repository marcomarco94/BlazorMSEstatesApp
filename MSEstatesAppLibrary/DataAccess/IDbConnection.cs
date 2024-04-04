namespace MSEstatesAppLibrary.DataAccess;

public interface IDbConnection
{
    string DbName { get; }
    string CategoryCollectionName { get; }
    string LocationCollectionName { get; }
    string ListingCollectionName { get; }
    string CompanyCollectionName { get; }
    string AgentCollectionName { get; }
    string RealtorContactCollectionName { get; }
    string AdvertisementTypeCollectionName { get; }
    string FacebookPostCollectionName { get; }
    MongoClient Client { get; }
    IMongoCollection<CategoryModel> CategoryCollection { get; }
    IMongoCollection<LocationModel> LocationCollection { get; }
    IMongoCollection<ListingModel> ListingCollection { get; }
    IMongoCollection<CompanyModel> CompanyCollection { get; }
    IMongoCollection<AgentModel> AgentCollection { get; }
    IMongoCollection<RealtorContactModel> RealtorContactCollection { get; }
    IMongoCollection<AdvertisementTypeModel> AdvertisementTypeCollection { get; }
    IMongoCollection<FacebookPostModel> FacebookPostCollection { get; }
}