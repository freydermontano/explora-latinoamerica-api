using blogExploraLatamAPI.Models.Domain;
using blogExploraLatamAPI.Models.DTO;
using blogExploraLatamAPI.Repositories.Implementations;
using blogExploraLatamAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace blogExploraLatamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await imageRepository.GetAll();

            //Convertir Modelo dominio a Dto
            var response = new List<BlogImageDto>();
            foreach (var image in images)
            {
                response.Add(new BlogImageDto
                {
                    Id = image.Id,
                    FileExtension = image.FileExtension,
                    FileName = image.FileName,
                    Title = image.Title,
                    Url = image.Url,
                    DateCreated = image.DateCreated

                });
            }

            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {

            ValidateFileUpload(file);

            //Subir Imagen
            if (ModelState.IsValid)
            {
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now,
                };

                //Guardar en la base de datos
                blogImage = await imageRepository.Upload(file, blogImage);

                //Pasar de modelo dominio a modelop Dto
                var response = new BlogImageDto
                {
                    Id = blogImage.Id,
                    FileName = $"{blogImage.FileName}{blogImage.FileExtension}",
                    Title = blogImage.Title,
                    DateCreated = DateTime.Now,
                    Url = blogImage.Url,

                };
                return Ok(response);

            }

            return BadRequest(ModelState);
        }



        // Metodo para validar la extension y el tamaño de un archivo subido
        private void ValidateFileUpload(IFormFile file)
        {
            // Extensiones permitidas para las imagenes
            var allowedExtensions = new string[] { ".png", ".jpg", ".jpeg" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Formato no válido. Solo se permiten archivos .png, .jpg y .jpeg");
            }

            // Validar tamaño del archivo, maximo 10MB (10 * 1024 * 1024 bytes)
            const long maxFileSize = 10 * 1024 * 1024;
            if (file.Length > maxFileSize)
            {
                // Agrega un error si el archivo excede el tamaño permitido
                ModelState.AddModelError("file", "El archivo no puede ser mayor a 10MB.");
            }
        }

    }
}
