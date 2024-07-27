using MSEstatesAppLibrary.Models;

namespace MSEstatesAppLibrary.DataAccess;

public interface IFacebookPostData
{
    Task<List<FacebookPostModel>> GetAllFacebookPosts();
    Task CreateFacebookPost(FacebookPostModel facebookPost);
}