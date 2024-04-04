using Microsoft.Extensions.Caching.Memory;

namespace MSEstatesAppLibrary.DataAccess;

public class MongoCompanyData : ICompanyData
{
    private const string CacheName = "Company";
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<CompanyModel> _company;

    public MongoCompanyData(IDbConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _company = db.CompanyCollection;
    }

    public async Task<CompanyModel> GetCompany()
    {
        var output = _cache.Get<CompanyModel>(CacheName);
        if (output is null)
        {
            var result = await _company.FindAsync(_ => true);
            output = result.FirstOrDefault();
            _cache.Set(CacheName, output, TimeSpan.FromDays(7));
        }

        return output;
    }

    public async Task UpdateCompany(CompanyModel? updatedCompany)
    {
        var isEmptyCollectionFilter = Builders<CompanyModel>.Filter.Empty;

        var updateBuilder = Builders<CompanyModel>.Update;
        var update = updateBuilder.Combine(
            typeof(CompanyModel)
                .GetProperties()
                .Where(prop => prop.GetValue(updatedCompany) != null)
                .Select(prop => updateBuilder.Set(prop.Name, prop.GetValue(updatedCompany)))
                .ToList()
        );
        var updateResult = await _company.UpdateOneAsync(isEmptyCollectionFilter, update);
    }

    public async Task CreateCompany(CompanyModel company)
    {
        await _company.InsertOneAsync(company);
    }
}