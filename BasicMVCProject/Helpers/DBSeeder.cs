using AutoMapper;
using BasicMVCProject.Models.Seeder;
using DAL.Context;
using DAL.Entities.Category;
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

            if (!context.Categories.Any())
            {
                var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Categories.json");

                if (File.Exists(jsonFile))
                {
                    var jsonData = await File.ReadAllTextAsync(jsonFile);
                    var categories = System.Text.Json.JsonSerializer.Deserialize<List<SeederCategoryModel>>(jsonData);
                    if (categories != null)
                    {
                        var ent = mapper.Map<List<CategoryEntity>>(categories);
                        await context.Categories.AddRangeAsync(ent);
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    throw new FileNotFoundException("Json file not found", jsonFile);
                }
            }
        }
    }
}
