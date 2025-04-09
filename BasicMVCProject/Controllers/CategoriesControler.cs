using BasicMVCProject.Models.Category;
using Microsoft.AspNetCore.Mvc;

namespace BasicMVCProject.Controllers
{
    public class CategoriesController : Controller
    {
        List<CategoryItemViewModel> categories = new List<CategoryItemViewModel>
        {
            new CategoryItemViewModel
            {
                Id = 1,
                Name = "Пригодницькі",
                Description = "Мультфільми, сповнені захопливих пригод та подорожей.",
                ImageUrl = "https://lux.fm/uploads/media_news/2022/06/62b5ba40b796c685027414.jpg?w=400&fit=cover&output=webp&q=85"
            },
            new CategoryItemViewModel
            {
                Id = 2,
                Name = "Комедійні",
                Description = "Мультфільми, що піднімуть настрій та розсмішать.",
                ImageUrl = "https://112.ua/uploads/950x550_1733234222_.jpg.png"
            },
            new CategoryItemViewModel
            {
                Id = 3,
                Name = "Фентезі",
                Description = "Магічні світи та чарівні істоти чекають на вас.",
                ImageUrl = "https://static.kinoafisha.info/k/articles/1200/upload/articles/697475977550.jpg"
            },
            new CategoryItemViewModel
            {
                Id = 4,
                Name = "Наукова фантастика",
                Description = "Подорожі в космос та футуристичні технології.",
                ImageUrl = "https://itc.ua/wp-content/uploads/2024/10/sci-fi-8281527_1280.jpg"
            },
            new CategoryItemViewModel
            {
                Id = 5,
                Name = "Класика Disney",
                Description = "Найкращі класичні мультфільми від студії Disney.",
                ImageUrl = "https://www.ranok.com.ua/storage/img/product/0b4e3d34b64fa64f.jpg"
            }
        };

        public IActionResult Index()
        {
            return View(categories);
        }
    }
}
