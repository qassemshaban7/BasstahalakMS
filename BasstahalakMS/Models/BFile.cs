using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasstahalakMS.Models
{
    public class BFile
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال اسم الملف")]
        public string Name { get; set; }

        [Required(ErrorMessage = "يرجى إدخال وصف الملف")]
        public string Description { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "يرجى اختيار ملف Word")]
        public IFormFile UploadedFile { get; set; }
        public string FilePath { get; set; }

        [Required(ErrorMessage = "يرجى إدخال اسم الكتاب")]
        public string BookName { get; set; }
        [Required(ErrorMessage = "يرجى إدخال اسم الفرع")]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "يرجى إدخال عدد الوحدات")]
        public int UnitsCount { get; set; }

        [Required(ErrorMessage = "يرجى إدخال عدد الدروس")]
        public int LessonsCount { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
