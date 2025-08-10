using blogExploraLatamAPI.Models.Domain;
using blogExploraLatamAPI.Repositories.Interfaces;
using CodeBlog.API.Data;
using Microsoft.EntityFrameworkCore;

namespace blogExploraLatamAPI.Repositories.Implementations
{
    public class ImageRepository : IImageRepository
    {

        private readonly IWebHostEnvironment webHostEnvironment; // Para obtener la ruta raiz del proyecto
        private readonly IHttpContextAccessor httpContextAccessor; // Para obtener información de la solicitud HTTP actual
        private readonly ApplicationDbContext applicationDbContext; // Para guardar los datos en la base de datos

        public ImageRepository(
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext applicationDbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<BlogImage>> GetAll()
        {
            return await applicationDbContext.BlogImages.ToListAsync();
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            // Guardar la imagen en disco
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);


            //Construir la URL accesible desde el navegador
            // Obtenemos el contexto HTTP actual para construir la URL, https://localhost:7601/Images/imagen.jpg
            var httpRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;

            //Guardar los datos en la base de datos
            await applicationDbContext.BlogImages.AddAsync(blogImage);
            await applicationDbContext.SaveChangesAsync();
            return blogImage;
        }
    }
}
