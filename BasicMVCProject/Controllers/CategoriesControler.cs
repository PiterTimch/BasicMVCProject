using AutoMapper;
using BasicMVCProject.Models.Category;
using DAL.Context;
using DAL.Entities.Category;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BasicMVCProject.Controllers
{
    public class CategoriesController(ICategoryService service, IMapper mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var categoiesEntity = await service.GetAllAsync();
            var model = mapper.Map<List<CategoryItemViewModel>>(categoiesEntity);

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
            var items = await service.GetAllAsync();
            var item = items.FirstOrDefault(x => x.Name == model.Name);

            if (item != null)
            {
                ModelState.AddModelError("Name", "Категорія з такою назвою вже існує.");
                return View(model);
            }
            item = mapper.Map<CategoryEntity>(model);
            await service.AddAsync(item);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await service.GetByIdAsync(id);
            if (entity == null) return NotFound();

            var model = mapper.Map<CategoryEditViewModel>(entity);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditViewModel model)
        {
            var entities = await service.GetAllAsync();
            
            bool isAny = entities.Any(x => x.Name == model.Name && x.Id != model.Id);

            if (isAny)
            {
                ModelState.AddModelError("Name", "Категорія з такою назвою вже існує.");
                return View(model);
            }

            var item = mapper.Map<CategoryEntity>(model);
            await service.UpdateAsync(item);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await service.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
