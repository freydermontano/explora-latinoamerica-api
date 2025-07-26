using blogExploraLatamAPI.Repositories.Interfaces;
using CodeBlog.API.Data;
using CodeBlog.API.Models.Domain;

namespace blogExploraLatamAPI.Repositories.Implementations
{
    public class BlogPostRepository : IBlogPostRepository
    {


        private readonly ApplicationDbContext context;


        public BlogPostRepository(ApplicationDbContext context)
        {
            this.context = context;
        }



        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await context.AddAsync(blogPost);
            await context.SaveChangesAsync();
            return blogPost;
        }
    }
}
