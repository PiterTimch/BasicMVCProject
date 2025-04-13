using AutoMapper;
using BasicMVCProject.Models.Category;
using DAL.Context;
using DAL.Entities.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BasicMVCProject.Controllers
{
    public class CategoriesController(AppDbContext context, IMapper mapper) : Controller
    {
        public IActionResult Index()
        {
            var model = mapper.Map<List<CategoryItemViewModel>>(context.Categories.ToList());

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            var item = await context.Categories.SingleOrDefaultAsync(x => x.Name == model.Name);
            if (item != null)
            {
                ModelState.AddModelError("Name", "Категорія з такою назвою вже існує.");
                return View(model);
            }
            item = mapper.Map<CategoryEntity>(model);
            await context.Categories.AddAsync(item);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await context.Categories.FindAsync(id);
            if (entity == null) return NotFound();

            var model = mapper.Map<CategoryEditViewModel>(entity);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditViewModel model)
        {
            var exists = await context.Categories
                .AnyAsync(x => x.Name == model.Name && x.Id != model.Id);

            if (exists)
            {
                ModelState.AddModelError("Name", "Категорія з такою назвою вже існує.");
                return View(model);
            }

            var item = mapper.Map<CategoryEntity>(model);
            context.Categories.Update(item);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
