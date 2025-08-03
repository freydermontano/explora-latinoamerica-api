using blogExploraLatamAPI.Models.Domain;

namespace blogExploraLatamAPI.Repositories.Interfaces
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);

        Task<IEnumerable<BlogImage>> GetAll();
    }
}
