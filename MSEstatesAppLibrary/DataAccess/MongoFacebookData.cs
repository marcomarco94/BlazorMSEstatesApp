namespace MSEstatesAppLibrary.DataAccess;

public class MongoFacebookData : IFacebookData
{
    private readonly IMongoCollection<FacebookPostModel> _facebookPosts;
    
    public MongoFacebookData(IDbConnection db)
    {
        _facebookPosts = db.FacebookPostCollection;
    }

    public async Task<List<FacebookPostModel>> GetAllFacebookPosts()
    {
        var results = await _facebookPosts.FindAsync(_ => true);
        return results.ToList();
    } 

    public Task CreateFacebookPost(FacebookPostModel facebookPost)
    {
        if (string.IsNullOrEmpty(facebookPost.Id))
        {
            facebookPost.Id = ObjectId.GenerateNewId().ToString();
        }
        
        var filter = Builders<FacebookPostModel>.Filter.Eq(post => post.ListingId, facebookPost.ListingId);
        var options = new ReplaceOptions { IsUpsert = true };
        return _facebookPosts.ReplaceOneAsync(filter, facebookPost, options);
    }
}