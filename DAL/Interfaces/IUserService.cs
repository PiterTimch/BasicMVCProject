using DAL.Entities.User;

namespace DAL.Interfaces
{
    public interface IUserService
    {
        Task<UserEntity?> GetUserByIdAsync(int id);
        Task<UserEntity?> GetUserByEmail(string email);
        Task<IEnumerable<UserEntity>> GetAllUsersAsync();
        Task CreateUserAsync(UserEntity user);
        Task UpdateUserAsync(UserEntity updatedUser);
        Task DeleteUserAsync(int id);
    }
}
