﻿using AutoMapper;
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUsers = await service.GetAllUsersAsync();

            bool isEmailExist = existingUsers.Any(u => u.Email == model.Email);
            bool isLoginExist = existingUsers.Any(u => u.Login == model.Login);
            bool isPhoneExist = existingUsers.Any(u => u.Phone == model.Phone);

            if (isEmailExist)
                ModelState.AddModelError(nameof(model.Email), "Користувач з такою поштою уже існує");

            if (isLoginExist)
                ModelState.AddModelError(nameof(model.Login), "Користувач з таким логіном уже існує");

            if (isPhoneExist)
                ModelState.AddModelError(nameof(model.Phone), "Користувач з таким телефоном уже існує");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var entity = mapper.Map<UserEntity>(model);

            if (model.ImageFile != null)
            {
                string imageName = await imageService.SaveImageAsync(model.ImageFile);
                entity.ImageName = imageName;
            }

            await service.CreateUserAsync(entity);

            return RedirectToAction("Index", "Categories");
        }
    }
}
