namespace blogExploraLatamAPI.Models.DTO
{
    public class CreateBlogPostRequestDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public string urlHandle { get; set; }
        public string FeatureImageUrl { get; set; }
        public bool IsVisible { get; set; }

    }
}
