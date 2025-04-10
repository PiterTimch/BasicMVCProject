using DAL.Entities.Category;

namespace DAL.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryEntity>> GetAllAsync();
        Task<CategoryEntity?> GetByIdAsync(int id);
        Task AddAsync(CategoryEntity category);
        Task UpdateAsync(CategoryEntity category);
        Task DeleteAsync(int id);
    }
}
