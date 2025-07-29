namespace CodeBlog.API.Models.Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UrlHandle { get; set; }

        // Relacion muchos a muchos
        // Una categoria puede estar asociada a muchos posts
        public ICollection<BlogPost> blogPosts { get; set; }

    }
}
