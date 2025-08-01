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


        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
           var existingBlogPost = await  context.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);

            if (existingBlogPost != null)
            {
                context.Remove(existingBlogPost);
                await context.SaveChangesAsync();
                return existingBlogPost;
            }

            return null;
        }


        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await context.BlogPosts.Include(x => x.Categories).ToListAsync();
        }


        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await context.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await context.BlogPosts.Include(x =>x.Categories).FirstOrDefaultAsync(x =>x.Id == blogPost.Id);
            if (existingBlogPost == null)
            {
                return null;
            }

            //Actualizar BlogPost
            context.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);
            //Actualizar Categoria
            existingBlogPost.Categories = blogPost.Categories;
            await context.SaveChangesAsync();
            return blogPost;
        }
    }
}
