using AutoMapper;
using BasicMVCProject.Interfaces;
using BasicMVCProject.Models.User;
using BasicMVCProject.Services;
using BCrypt.Net;
using DAL.Entities.User;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet]
        public IActionResult Login()
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

            entity.Role = "User";

            await service.CreateUserAsync(entity);

            await SignInWithCookiesAsync(entity);

            return RedirectToAction("Index", "Categories");
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel model) 
        {
            var user = await service.GetUserByEmail(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(nameof(model.Email), "Неправильна пошта");
                return View(model);
            }

            bool result = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

            if (!result)
            {
                ModelState.AddModelError(nameof(model.Password), "Неправильний пароль");
                return View(model);
            }

            await SignInWithCookiesAsync(user);

            if (user.Role == "Admin")
            {
                return RedirectToAction("AdminIndex", "Categories");
            }

            return RedirectToAction("Index", "Categories");
        }

        private async Task SignInWithCookiesAsync(UserEntity user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("Cookies", principal);
        }

    }
}
