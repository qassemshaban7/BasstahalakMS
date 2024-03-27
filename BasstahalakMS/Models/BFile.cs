using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasstahalakMS.Models
{
    public class BFile
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال اسم الملف")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "يرجى إدخال وصف الملف")]
        public string? Description { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "يرجى اختيار ملف Word")]
        public IFormFile UploadedFile { get; set; }
        public string? FilePath { get; set; }

        public string? fileContent  { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public ICollection<FileBranch> FileBranches { get; set; }
        public int status { get; set; } = 0;

        public int? TeamStatus { get; set; }  // -1 For Supervisor   -2 For Member of Team   
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
