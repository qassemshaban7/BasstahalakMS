using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasstahalakMS.Models
{
    public class PdfFile
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "يرجى إدخال اسم الملف")]
        public string Name { get; set; }
        public string? Description { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "يرجى اختيار ملف pdf")]
        public IFormFile pdfFile { get; set; }  
        public string? PDFPath { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public int status { get; set; }  //0-craete    //1-super_admin  //2-need to Edit   //3-send to review   //4-send to printing

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int BfileId { get; set; }
        [ForeignKey("BfileId")]
        public BFile BFile { get; set; }
    }
}
