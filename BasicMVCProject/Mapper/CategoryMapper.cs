using AutoMapper;
using BasicMVCProject.Models.Category;
using BasicMVCProject.Models.Seeder;
using DAL.Entities.Category;

namespace BasicMVCProject.Mapper
{
    public class CategryMapper : Profile
    {
        public CategryMapper()
        {
            CreateMap<CategoryEntity, CategoryItemViewModel>();
            CreateMap<CategoryEntity, CategoryEditViewModel>();
            CreateMap<CategoryEditViewModel, CategoryEntity>();
            CreateMap<CategoryCreateViewModel, CategoryEntity>()
                .ForMember(x => x.ImageUrl, opt => opt.Ignore());
            CreateMap<CategoryItemViewModel, CategoryEditViewModel>();
        }
    }
}
