using MSEstatesAppLibrary.Models;

namespace MSEstatesAppLibrary.DataAccess;

public class MongoFacebookGroupData : IFacebookGroupData
{
    private readonly IMongoCollection<FacebookGroupModel> _facebookGroups;

    public MongoFacebookGroupData(IDbConnection db)
    {
        _facebookGroups = db.FacebookGroupCollection;
    }

    public async Task<List<FacebookGroupModel>> GetAllGroups()
    {
        var results = await _facebookGroups.FindAsync(_ => true);
        return results.ToList();
    }

    public async Task CreateGroup(FacebookGroupModel facebookGroup)
    {
        facebookGroup.Id = null;
        await _facebookGroups.InsertOneAsync(facebookGroup);
    }

    public async Task UpdateGroup(FacebookGroupModel facebookGroup)
    {
        if (facebookGroup == null) throw new ArgumentNullException(nameof(facebookGroup));

        var filter = Builders<FacebookGroupModel>.Filter.Eq(f => f.Id, facebookGroup.Id);
        await _facebookGroups.ReplaceOneAsync(filter, facebookGroup);
    }

    public async Task<FacebookGroupModel> GetTemplateById(string? groupId)
    {
        if (string.IsNullOrEmpty(groupId)) throw new ArgumentNullException(nameof(groupId));
        var filter = Builders<FacebookGroupModel>.Filter.Eq(f => f.Id, groupId);
        var group = await _facebookGroups.Find(filter).FirstOrDefaultAsync();
        return group;
    }

    public async Task DeleteGroup(string groupId)
    {
        if (string.IsNullOrEmpty(groupId)) throw new ArgumentNullException(nameof(groupId));
        var filter = Builders<FacebookGroupModel>.Filter.Eq(f => f.Id, groupId);
        await _facebookGroups.DeleteOneAsync(filter);
    }
}