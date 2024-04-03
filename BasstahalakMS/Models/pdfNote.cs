using System.ComponentModel.DataAnnotations.Schema;

namespace BasstahalakMS.Models
{
    public class pdfNote
    {
        public int Id { get; set; }
        public string? Description { get; set; } 
        public DateTime SentDate { get; set; } = DateTime.Now;
        public string? ReciveUserId { get; set; }
        [ForeignKey("ReciveUserId")]
        public ApplicationUser? User { get; set; }
        public int PdfId { get; set; }
        [ForeignKey("PdfId")]
        public PdfFile? PdfFile { get; set; }
    }
}
