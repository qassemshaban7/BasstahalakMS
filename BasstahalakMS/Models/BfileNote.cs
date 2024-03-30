using System.ComponentModel.DataAnnotations.Schema;

namespace BasstahalakMS.Models
{
    public class BfileNote
    {
        public int Id { get; set; }
        public int BfileId { get; set; }
        [ForeignKey("BfileId")]
        public BFile BFile { get; set; }

        public string? ReciveUserId { get; set; }   
        [ForeignKey("ReciveUserId")]
        public ApplicationUser User { get; set; }
        public string? SendUserId { get; set; }
        //[ForeignKey("SendUserId")]
        //public ApplicationUser User { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public int status { get; set; }

        public string? CurrentFileContent { get; set; }
        public string? Notes { get; set; }
    }
}
