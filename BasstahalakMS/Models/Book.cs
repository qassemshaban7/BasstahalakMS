using System.ComponentModel.DataAnnotations;

namespace BasstahalakMS.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "يرجى إدخال اسم الكتاب")]
        [MaxLength(200)]
        public string Name { get; set; }    
    }
}
