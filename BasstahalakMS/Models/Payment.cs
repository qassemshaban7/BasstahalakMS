using System.ComponentModel.DataAnnotations.Schema;

namespace BasstahalakMS.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }      
        public double Money { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
        public string? PhotoPath { get; set; }
        public int Type { get; set; }
        public DateTime PaymentTime { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
