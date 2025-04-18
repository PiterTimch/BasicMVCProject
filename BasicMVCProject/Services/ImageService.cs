using BasicMVCProject.Interfaces;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace BasicMVCProject.Services
{
    public class ImageService(IConfiguration configuration) : IImageService
    {
        public async Task DeleteImageAsync(string name)
        {
            var sizes = configuration.GetRequiredSection("ImageSizes").Get<List<int>>();
            var dir = Path.Combine(Directory.GetCurrentDirectory(), configuration["ImageDir"]!);

            Task[] tasks = sizes
                .AsParallel()
                .Select(size =>
                {
                    return Task.Run(() =>
                    {
                        var path = Path.Combine(dir, $"{size}_{name}");
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    });
                })
                .ToArray();

            await Task.WhenAll(tasks);
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            using MemoryStream ms = new();
            await file.CopyToAsync(ms);
            var bytes = ms.ToArray();

            string imageName = await saveImageAsync(bytes);
            return imageName;
        }

        private async Task<string> saveImageAsync(byte[] bytes)
        {
            string imageName = $"{Guid.NewGuid()}.webp";

            var sizes = configuration.GetRequiredSection("ImageSizes").Get<List<int>>();

            var tasks = sizes.AsParallel().Select(s => saveImageAsync(bytes, imageName, s)).ToArray();

            await Task.WhenAll(tasks);

            return imageName;
        }

        private async Task saveImageAsync(byte[] bytes, string name, int size)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), configuration["ImageDir"]!, $"{size}_{name}");
            using var image = Image.Load(bytes);
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(size, size),
                Mode = ResizeMode.Max
            }));

            await image.SaveAsync(path, new WebpEncoder());
        }
    }
}
