using AutoMapper;
using BasicMVCProject.Interfaces;
using BasicMVCProject.Models.User;
using BasicMVCProject.Services;
using DAL.Entities.User;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BasicMVCProject.Controllers
{
    public class UsersController(IMapper mapper, IUserService service, IImageService imageService) : Controller()
    {
        public IActionResult Index()
        {
            return View("Create");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            var entity = mapper.Map<UserEntity>(model);

            string imageName = await imageService.SaveImageAsync(model.ImageFile);

            entity.ImageName = imageName;
            await service.CreateUserAsync(entity);
            return RedirectToAction("Index", "Category");
        }
    }
}
