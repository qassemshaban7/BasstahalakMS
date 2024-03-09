using Microsoft.AspNetCore.Identity;

namespace BasstahalakMS.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string ArabicRoleName { get; set; }
    }
}
