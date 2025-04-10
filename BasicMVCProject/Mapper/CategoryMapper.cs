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
        }
    }
}
