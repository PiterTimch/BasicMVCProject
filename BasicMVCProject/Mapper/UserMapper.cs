using AutoMapper;
using BasicMVCProject.Models.User;
using DAL.Entities.User;

namespace BasicMVCProject.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserCreateViewModel, UserEntity>()
                .ForMember(x => x.PasswordHash, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));
        }
    }
}
