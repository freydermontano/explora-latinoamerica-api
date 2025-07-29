using CodeBlog.API.Models.DTO;

namespace blogExploraLatamAPI.Models.DTO
{
    public class BlogPostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public string urlHandle { get; set; }
        public string FeatureImageUrl { get; set; }
        public bool IsVisible { get; set; }

        //Devolver respuesta a la vista de categorias con clientes
        public List<CategoryDto> Categories  { get; set; } = new List<CategoryDto>();
    }
}
