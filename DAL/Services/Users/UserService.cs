using DAL.Context;
using DAL.Entities.User;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services.Users
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateUserAsync(UserEntity user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<UserEntity>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserEntity> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            return user;
        }

        public async Task<UserEntity> GetUserByEmail(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == id.ToString());
            if (user == null)
            {
                throw new KeyNotFoundException($"User with email {id} not found.");
            }

            return user;
        }

        public async Task UpdateUserAsync(UserEntity updatedUser)
        {
            var existingUser = await _context.Users.FindAsync(updatedUser.Id);
            if (existingUser != null)
            {
                existingUser.FirstName = updatedUser.FirstName;
                existingUser.LastName = updatedUser.LastName;
                existingUser.Login = updatedUser.Login;
                existingUser.Email = updatedUser.Email;
                existingUser.Phone = updatedUser.Phone;
                existingUser.PasswordHash = updatedUser.PasswordHash;

                await _context.SaveChangesAsync();
            }
        }
    }
}
