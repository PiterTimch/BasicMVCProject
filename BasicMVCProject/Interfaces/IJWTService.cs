using DAL.Entities.User;

namespace BasicMVCProject.Interfaces
{
    public interface IJWTService
    {
        public string GenerateToken(UserEntity user);
    }
}
