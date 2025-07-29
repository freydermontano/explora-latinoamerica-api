using blogExploraLatamAPI.Repositories.Interfaces;
using CodeBlog.API.Data;
using CodeBlog.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await context.BlogPosts.Include(x => x.Categories).ToListAsync();
        }


        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await context.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
