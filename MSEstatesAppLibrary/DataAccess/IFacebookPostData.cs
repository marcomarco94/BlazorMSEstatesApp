using MSEstatesAppLibrary.Models;

namespace MSEstatesAppLibrary.DataAccess;

public interface IFacebookPostData
{
    Task<List<FacebookTemplateModel>> GetAllFacebookPosts();
    Task<List<FacebookTemplateModel>> GetFacebookPostsByListingId(string id);
    Task CreateFacebookPost(FacebookTemplateModel facebookPost);
}