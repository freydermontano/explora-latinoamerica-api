using blogExploraLatamAPI.Models.DTO;
using CodeBlog.API.Models.Domain;
using CodeBlog.API.Models.DTO;
using CodeBlog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {

            // Mapear el DTO a la entidad del dominio
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };

            // Guardar la nueva categoria en la base de datos que se encuentra en nuestro repositorio
            await categoryRepository.CreateAsync(category);

            // Mapear la entidad de dominio a un DTO de respuesta para el cliente
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            //Obtebner tidas las categorias de la base de datos
           var categories =  await categoryRepository.GetAllAsync();

            //Mapear Modelo Dominio a Dto

            //Lista vacia para almacenar los objetos que se enviaran como respuesta(DTOs)
            var response = new List<CategoryDto>();

            //Recorrer cada categoria
            foreach (var category in categories)
            {
                //Convierte cada objeto de dominio Category a un DTO (CategoryDto)
                response.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var existingCategory = await categoryRepository.GetById(id);

            if (existingCategory is null)
            {
                return NotFound();
            }

            //Mapear Modelo Dominio a Dto
            var response = new CategoryDto
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequestDto request)
        {
            // Mapear el DTO recibido desde el cliente al modelo de dominio categoria
            //Seconstruye un Objeto de tipo categoria con los datos que bienen del cleinte, ademas se le asigna el id desde la ruta y no del cuerpo para evita inconsistencia
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            var updateCategory = await categoryRepository.UpdateAsync(category);
            if (updateCategory == null)
            {
               return  NotFound(); 
            }

            //Mapear el objeto de dominio actualizado al DTO que sera devuelto al cliente
            var response = new CategoryDto
            {
                Id = updateCategory.Id,
                Name = updateCategory.Name,
                UrlHandle = updateCategory.UrlHandle
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await categoryRepository.DeleteAsync(id);
            if (category is null)
            {
                return NotFound();
            }
            // Mapear el objeto de dominio eliminado al DTO que sera devuelto al cliente
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok();
        }


    }


    

    
}
