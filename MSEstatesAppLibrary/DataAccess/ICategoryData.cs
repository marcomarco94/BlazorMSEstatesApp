namespace MSEstatesAppLibrary.DataAccess;

public interface ICategoryData
{
    Task<List<CategoryModel>> GetAllCategories();
    Task CreateCategory(CategoryModel category);
}