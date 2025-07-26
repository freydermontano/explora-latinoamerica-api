using blogExploraLatamAPI.Models.Domain;
using blogExploraLatamAPI.Models.DTO;
using blogExploraLatamAPI.Repositories.Interfaces;
using CodeBlog.API.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace blogExploraLatamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {


        private readonly IBlogPostRepository blogPostRepository;
        public BlogPostsController(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
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
            };

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

            };

            return Ok( response );
        }
    }
}
