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
        public async Task<IActionResult> createBlogPost([FromBody] CreateBlogPostRequestDto request )
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
            blogPost =  await blogPostRepository.CreateAsync( blogPost );

            // Mapear la entidad de dominio a un DTO de respuesta para el cliente
            var response = new BlogPostDto
            {
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

            return Ok( response );
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
                    Title= blogPost.Title,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    urlHandle= blogPost.urlHandle,
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
                return Ok( response );
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

            return Ok( response );
        }

    }
}
