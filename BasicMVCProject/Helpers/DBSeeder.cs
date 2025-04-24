using AutoMapper;
using BasicMVCProject.Interfaces;
using BasicMVCProject.Models.Seeder;
using DAL.Context;
using DAL.Entities.Category;
using DAL.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace BasicMVCProject.Helpers
{
    public static class DBSeeder
    {
        public static async Task SeedData(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();

            await context.Database.MigrateAsync();

            await SeedCategories(context, mapper, imageService);
            await SeedUsers(context, mapper);
        }

        private static async Task SeedCategories(AppDbContext context, IMapper mapper, IImageService imageService)
        {
            if (context.Categories.Any()) return;

            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Categories.json");

            if (!File.Exists(jsonFile))
                throw new FileNotFoundException("Json file not found", jsonFile);

            var jsonData = await File.ReadAllTextAsync(jsonFile);
            var categories = System.Text.Json.JsonSerializer.Deserialize<List<SeederCategoryModel>>(jsonData);

            if (categories != null)
            {
                var entities = new List<CategoryEntity>();
                foreach (var category in categories)
                {
                    var entity = mapper.Map<CategoryEntity>(category);

                    if (!string.IsNullOrEmpty(category.ImagePath))
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "SeederImages", category.ImagePath);
                        if (File.Exists(filePath))
                        {
                            var formFile = ConvertFileToIFormFile(filePath);
                            entity.ImageUrl = await imageService.SaveImageAsync(formFile);
                        }
                    }

                    entities.Add(entity);
                }

                await context.Categories.AddRangeAsync(entities);
                await context.SaveChangesAsync();
            }
        }


        private static IFormFile ConvertFileToIFormFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return new FormFile(stream, 0, stream.Length, "ImageFile", fileInfo.Name)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/" + fileInfo.Extension.Trim('.')
            };
        }


        private static async Task SeedUsers(AppDbContext context, IMapper mapper)
        {
            if (context.Users.Any()) return;

            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Users.json");

            if (!File.Exists(jsonFile))
                throw new FileNotFoundException("Json file not found", jsonFile);

            var jsonData = await File.ReadAllTextAsync(jsonFile);
            var categories = System.Text.Json.JsonSerializer.Deserialize<List<SeederUserModel>>(jsonData);

            if (categories != null)
            {
                var ent = mapper.Map<List<UserEntity>>(categories);
                await context.Users.AddRangeAsync(ent);
                await context.SaveChangesAsync();
            }
        }
    }
}
