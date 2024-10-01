using MSEstatesAppLibrary.Models;

namespace MSEstatesAppLibrary.DataAccess;

public class MongoFacebookPostData : IFacebookPostData
{
    private readonly IMongoCollection<FacebookTemplateModel> _facebookPosts;

    public MongoFacebookPostData(IDbConnection db)
    {
        _facebookPosts = db.FacebookPostCollection;
    }

    public async Task<List<FacebookTemplateModel>> GetAllFacebookPosts()
    {
        var results = await _facebookPosts.FindAsync(_ => true);
        return results.ToList();
    }

    public async Task<List<FacebookTemplateModel>> GetFacebookPostsByListingId(string id)
    {
        var filter = Builders<FacebookTemplateModel>.Filter.Eq(post => post.ListingId, id);
        return await _facebookPosts.Find(filter).ToListAsync();
    }

    public async Task CreateFacebookPost(FacebookTemplateModel facebookPost)
    {
        facebookPost.DateCreated = DateTime.Now;
        var existingPost = await _facebookPosts
            .Find(Builders<FacebookTemplateModel>.Filter.Eq(post => post.ListingId, facebookPost.ListingId))
            .FirstOrDefaultAsync();
        if (existingPost != null && existingPost.Id != facebookPost.Id)
        {
            facebookPost.Id = existingPost.Id;
            var filter = Builders<FacebookTemplateModel>.Filter.Eq(post => post.ListingId, facebookPost.ListingId);
            await _facebookPosts.ReplaceOneAsync(filter, facebookPost);
        }
        else
        {
            facebookPost.Id = null;
            await _facebookPosts.InsertOneAsync(facebookPost);
        }
    }
}