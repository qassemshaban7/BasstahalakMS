using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasstahalakMS.Models
{
    public class FileBranch
    {
        public int Id { get; set; }
        public int BFileId { get; set; }
        [ForeignKey("BFileId")]
        public BFile BFile { get; set; }
        public int BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }

        [Required(ErrorMessage = "يرجى إدخال عدد الوحدات")]
        public int UnitsCount { get; set; }

        [Required(ErrorMessage = "يرجى إدخال عدد الدروس")]
        public int LessonsCount { get; set; }
    }
}
