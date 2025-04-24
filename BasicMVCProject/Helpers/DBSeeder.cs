using AutoMapper;
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

            await context.Database.MigrateAsync();

            await SeedCategories(context, mapper);
            await SeedUsers(context, mapper);
        }

        private static async Task SeedCategories(AppDbContext context, IMapper mapper) 
        {
            if (context.Categories.Any()) return;

            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Categories.json");

            if (!File.Exists(jsonFile))
                throw new FileNotFoundException("Json file not found", jsonFile);

            var jsonData = await File.ReadAllTextAsync(jsonFile);
            var categories = System.Text.Json.JsonSerializer.Deserialize<List<SeederCategoryModel>>(jsonData);

            if (categories != null)
            {
                var ent = mapper.Map<List<CategoryEntity>>(categories);
                await context.Categories.AddRangeAsync(ent);
                await context.SaveChangesAsync();
            }
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
