using CodeBlog.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodeBlog.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions <ApplicationDbContext> options) : base(options)
        {
        }

        // Define las entidades que se mapearan como tablas en la base de datos mediante Entity Framework Core
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
