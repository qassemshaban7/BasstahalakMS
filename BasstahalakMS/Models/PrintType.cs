using System.ComponentModel.DataAnnotations;

namespace BasstahalakMS.Models
{
    public class PrintType  
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "يرجى إدخال نوع المطبوع")]
        public string Name { get; set; }

        public ICollection<Library>? Library { get; set; }
    }
}
