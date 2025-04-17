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

            if (model.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Зображення обов'язкове");
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

            var entity = mapper.Map<CategoryEntity>(model);
            string oldImage = entities.FirstOrDefault(x => x.Id == model.Id)!.ImageUrl;

            if (model.IsImageChanged)
            {
                if (String.IsNullOrEmpty(model.ImageFile.FileName.ToLower()))
                {
                    return NotFound();
                }

                await imageService.DeleteImageAsync(oldImage);

                string imageName = await imageService.SaveImageAsync(model.ImageFile);
                entity.ImageUrl = imageName;
            }
            else 
            {
                entity.ImageUrl = oldImage;
            }

            await service.UpdateAsync(entity);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await service.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                await imageService.DeleteImageAsync(category.ImageUrl);
            }

            await service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
