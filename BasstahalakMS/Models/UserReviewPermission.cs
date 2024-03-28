using System.ComponentModel.DataAnnotations.Schema;

namespace BasstahalakMS.Models
{
    public class UserReviewPermission
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int PermissionId { get; set; }
        [ForeignKey("PermissionId")]
        public ReviewPermission ReviewPermission { get; set; }
    }
}
