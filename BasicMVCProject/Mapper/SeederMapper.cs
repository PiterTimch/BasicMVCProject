using AutoMapper;
using BasicMVCProject.Models.Seeder;
using DAL.Entities.Category;
using DAL.Entities.User;

namespace BasicMVCProject.Mapper
{
    public class SeederMapper : Profile
    {
        public SeederMapper()
        {
            CreateMap<SeederCategoryModel, CategoryEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<SeederUserModel, UserEntity>()
                .ForMember(x => x.PasswordHash, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));
        }
    }
}
