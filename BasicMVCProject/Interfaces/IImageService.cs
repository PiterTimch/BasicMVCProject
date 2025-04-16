namespace BasicMVCProject.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile file);
        Task DeleteImage(string name);
    }
}
