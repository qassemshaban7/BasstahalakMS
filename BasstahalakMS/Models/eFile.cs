using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BasstahalakMS.Models
{
    public class eFile
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

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }   
    }
}