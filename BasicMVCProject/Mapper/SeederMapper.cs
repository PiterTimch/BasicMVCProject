using AutoMapper;
using BasicMVCProject.Models.Seeder;
using DAL.Entities.Category;

namespace BasicMVCProject.Mapper
{
    public class SeederMapper : Profile
    {
        public SeederMapper()
        {
            CreateMap<SeederCategoryModel, CategoryEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
