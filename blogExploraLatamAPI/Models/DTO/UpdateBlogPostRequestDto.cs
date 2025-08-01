namespace blogExploraLatamAPI.Models.DTO
{
    public class UpdateBlogPostRequestDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public string urlHandle { get; set; }
        public string FeatureImageUrl { get; set; }
        public bool IsVisible { get; set; }

        //Relacion con categoria, para mostrar categorias
        public List<Guid> Categories { get; set; } = new List<Guid>();   
    }
}
