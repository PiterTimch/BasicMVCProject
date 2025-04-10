using AutoMapper;
using BasicMVCProject.Models.Category;
using DAL.Context;
using Microsoft.AspNetCore.Mvc;

namespace BasicMVCProject.Controllers
{
    public class CategoriesController(AppDbContext context, IMapper mapper) : Controller
    {
        public IActionResult Index()
        {
            var model = mapper.Map<List<CategoryItemViewModel>>(context.Categories.ToList());

            return View(model);
        }
    }
}
