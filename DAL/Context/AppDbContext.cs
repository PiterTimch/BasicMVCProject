using DAL.Entities.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DAL.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<CategoryEntity> Categories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public AppDbContext()
        {
        }

        public static void Seed(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                try
                {
                    if (context.Categories.Any()) return;

                    context.Categories.AddRange(
                        new CategoryEntity
                        {
                            Name = "Пригодницькі",
                            Description = "Мультфільми, сповнені захопливих пригод та подорожей.",
                            ImageUrl = "https://lux.fm/uploads/media_news/2022/06/62b5ba40b796c685027414.jpg?w=400&fit=cover&output=webp&q=85"
                        },
                        new CategoryEntity
                        {
                            Name = "Комедійні",
                            Description = "Мультфільми, що піднімуть настрій та розсмішать.",
                            ImageUrl = "https://112.ua/uploads/950x550_1733234222_.jpg.png"
                        },
                        new CategoryEntity
                        {
                            Name = "Фентезі",
                            Description = "Магічні світи та чарівні істоти чекають на вас.",
                            ImageUrl = "https://static.kinoafisha.info/k/articles/1200/upload/articles/697475977550.jpg"
                        },
                        new CategoryEntity
                        {
                            Name = "Наукова фантастика",
                            Description = "Подорожі в космос та футуристичні технології.",
                            ImageUrl = "https://itc.ua/wp-content/uploads/2024/10/sci-fi-8281527_1280.jpg"
                        },
                        new CategoryEntity
                        {
                            Name = "Класика Disney",
                            Description = "Найкращі класичні мультфільми від студії Disney.",
                            ImageUrl = "https://www.ranok.com.ua/storage/img/product/0b4e3d34b64fa64f.jpg"
                        }
                    );

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<AppDbContext>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}
