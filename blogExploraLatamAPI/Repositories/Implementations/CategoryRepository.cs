using CodeBlog.API.Data;
using CodeBlog.API.Models.Domain;
using CodeBlog.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodeBlog.API.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {

        //Inyeccion del contexto de la base de datos
        private readonly ApplicationDbContext dbContext;
        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }
        public async Task<Category?> DeleteAsync(Guid id)
        {
            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (existingCategory is null)
            {
                return null;
            }

            dbContext.Categories.Remove(existingCategory);
            await dbContext.SaveChangesAsync();
            return existingCategory;
        }
        public async Task<IEnumerable<Category>> GetAllAsync(string? query = null)
        {
            //Query 
            var categories = dbContext.Categories.AsQueryable();

            //Filtrando 
            if (string.IsNullOrWhiteSpace(query) == false)
            {
                categories = categories.Where(x => x.Name.Contains(query));
            }


            //Ordenar 


            //Paginacion 

            return await categories.ToListAsync();

            //return await dbContext.Categories.ToListAsync();
        }
        public async Task<Category?> GetById(Guid id)
        {
           return await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                existingCategory.UrlHandle = category.UrlHandle;

                await dbContext.SaveChangesAsync();
                return existingCategory;
            }

            return null;
        }
    }
}
