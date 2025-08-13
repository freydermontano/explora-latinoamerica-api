using blogExploraLatamAPI.Models.DTO;
using blogExploraLatamAPI.Repositories.Interfaces;
using CodeBlog.API.Models.Domain;
using CodeBlog.API.Models.DTO;
using CodeBlog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace blogExploraLatamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {


        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;
        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }


        [HttpPost]
        public async Task<IActionResult> createBlogPost([FromBody] CreateBlogPostRequestDto request)
        {

            //Convertir  DTO a la entidad del dominio
            var blogPost = new BlogPost
            {
                Title = request.Title,
                Content = request.Content,
                ShortDescription = request.ShortDescription,
                PublishedDate = request.PublishedDate,
                Author = request.Author,
                urlHandle = request.urlHandle,
                FeatureImageUrl = request.FeatureImageUrl,
                IsVisible = request.IsVisible,
                //Inicializamos la categorias
                Categories = new List<Category>()
            };

            //Agregar la relacion de categoria en blogPost
            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);

                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            //Guardar  blogPost en la base de datos
            blogPost = await blogPostRepository.CreateAsync(blogPost);

            // Mapear la entidad de dominio a un DTO de respuesta para el cliente
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                urlHandle = blogPost.urlHandle,
                FeatureImageUrl = blogPost.FeatureImageUrl,
                IsVisible = blogPost.IsVisible,
                //lista de categorias dto
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList(),

            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();

            //Convertir Dominio a modelo Dto
            var response = new List<BlogPostDto>();
            foreach (var blogPost in blogPosts)
            {
                response.Add(new BlogPostDto
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    urlHandle = blogPost.urlHandle,
                    FeatureImageUrl = blogPost.FeatureImageUrl,
                    IsVisible = blogPost.IsVisible,
                    //lista de categorias dto
                    Categories = blogPost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle,
                    }).ToList(),
                });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{urlHadnle}")]
        public async Task<IActionResult> getBlogPostByUrlHandle([FromRoute] string urlHadnle)
        {
            var blogPost = await blogPostRepository.GetByIdUrlHandleAsync(urlHadnle);

            if (blogPost is null)
            {
                return NotFound();
            }

            //Convertir Modelo dominio a Dto
            var response = new BlogPostDto
            {

                Id = blogPost.Id,
                Title = blogPost.Title,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                urlHandle = blogPost.urlHandle,
                FeatureImageUrl = blogPost.FeatureImageUrl,
                IsVisible = blogPost.IsVisible,

                //Lista de categoria dto
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };

            return Ok(response);

        }


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {

            var blogPost = await blogPostRepository.GetByIdAsync(id);

            if (blogPost is null)
            {
                return NotFound();
            }

            //Convertir Modelo dominio a Dto
            var response = new BlogPostDto
            {

                Id = blogPost.Id,
                Title = blogPost.Title,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                urlHandle = blogPost.urlHandle,
                FeatureImageUrl = blogPost.FeatureImageUrl,
                IsVisible = blogPost.IsVisible,

                //Lista de categoria dto
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };

            return Ok(response);
        }


        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDto request)
        {
            var blogPost = new BlogPost
            {
                Id = id, // Necesario para la actualizacion
                Author = request.Author,
                Title = request.Title,
                Content = request.Content,
                ShortDescription = request.ShortDescription,
                PublishedDate = request.PublishedDate,
                urlHandle = request.urlHandle,
                FeatureImageUrl = request.FeatureImageUrl,
                IsVisible = request.IsVisible,
                Categories = new List<Category>()
            };

            // Validar y asignar categorias
            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);
                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            // Actualizar en base de datos
            var updatedBlogPost = await blogPostRepository.UpdateAsync(blogPost);

            if (updatedBlogPost == null)
            {
                return NotFound();
            }

            // Construir DTO de respuesta
            var response = new BlogPostDto
            {
                Id = updatedBlogPost.Id,
                Author = updatedBlogPost.Author,
                Title = updatedBlogPost.Title,
                Content = updatedBlogPost.Content,
                ShortDescription = updatedBlogPost.ShortDescription,
                PublishedDate = updatedBlogPost.PublishedDate,
                urlHandle = updatedBlogPost.urlHandle,
                FeatureImageUrl = updatedBlogPost.FeatureImageUrl,
                IsVisible = updatedBlogPost.IsVisible,
                Categories = updatedBlogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {

            var deleteBlogPost =  await blogPostRepository.DeleteAsync(id);
            if (deleteBlogPost == null)
            {
                return NotFound();
            }

            //Convertir modelo Dominio a Dto
            var response = new BlogPostDto
            {

                Id = deleteBlogPost.Id,
                Author = deleteBlogPost.Author,
                Title = deleteBlogPost.Title,
                Content = deleteBlogPost.Content,
                ShortDescription = deleteBlogPost.ShortDescription,
                urlHandle = deleteBlogPost.urlHandle,
                FeatureImageUrl = deleteBlogPost.FeatureImageUrl,
                IsVisible = deleteBlogPost.IsVisible,
                PublishedDate = deleteBlogPost.PublishedDate,
            };


            return Ok(response);
        }


    }
}
