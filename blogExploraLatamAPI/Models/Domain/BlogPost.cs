namespace CodeBlog.API.Models.Domain
{
    public class BlogPost
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


        // Relacion muchos a muchos
        // Un post puede estar asociado a muchas categorias
        public ICollection<Category> Categories { get; set; }

    }
}
