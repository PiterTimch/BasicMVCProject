using AutoMapper;
using BasicMVCProject.Interfaces;
using BasicMVCProject.Models.Category;
using DAL.Context;
using DAL.Entities.Category;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace BasicMVCProject.Controllers
{
    public class CategoriesController(
        ICategoryService service, IMapper mapper, IConfiguration configuration, IImageService imageService
        ) : Controller
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
            var entitys = await service.GetAllAsync();
            var entity = entitys.FirstOrDefault(x => x.Name == model.Name);

            if (entity != null)
            {
                ModelState.AddModelError("Name", "Категорія з такою назвою вже існує.");
                return View(model);
            }

            string imageName = await imageService.SaveImageAsync(model.ImageFile);

            entity = mapper.Map<CategoryEntity>(model);
            entity.ImageUrl = imageName;
            await service.AddAsync(entity);

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

            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                await imageService.DeleteImage(category.ImageUrl);
            }

            await service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
