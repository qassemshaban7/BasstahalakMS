using System.ComponentModel.DataAnnotations;

namespace BasstahalakMS.Models
{
    public class Branch
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "يرجى إدخال اسم الفرع")]
        [MaxLength(200)]
        public string Name { get; set; }
    }
}
