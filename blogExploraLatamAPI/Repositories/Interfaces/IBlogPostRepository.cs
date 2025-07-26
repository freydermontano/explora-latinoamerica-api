using CodeBlog.API.Models.Domain;

namespace blogExploraLatamAPI.Repositories.Interfaces
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost); 

    }
}
