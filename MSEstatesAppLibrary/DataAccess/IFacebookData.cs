namespace MSEstatesAppLibrary.DataAccess;

public interface IFacebookData
{
    Task<List<FacebookPostModel>> GetAllFacebookPosts();
    Task CreateFacebookPost(FacebookPostModel facebookPost);
}