namespace blogExploraLatamAPI.Models.Domain
{
    public class BlogImage
    {

        public Guid Id { get; set; }
        public string FileName { get; set; }              // Nombre del archivo (sin extension)
        public string FileExtension { get; set; }         // Extensión del archivo (.jpg, .png)
        public string Title { get; set; }
        public string Url { get; set; }                   // URL completa para acceder a la imagen
        public DateTime DateCreated { get; set; }


    }
}
