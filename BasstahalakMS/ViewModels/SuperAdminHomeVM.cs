using BasstahalakMS.Models;

namespace BasstahalakMS.ViewModels
{
    public class SuperAdminHomeVM
    {
        public  IEnumerable<ApplicationUser>? Users { get; set; }
        public  IEnumerable<Book>? Books { get; set; }
        public  IEnumerable<Branch>? Branches { get; set; }
        public  IEnumerable<PrintType>? PrintTypes { get; set; }
        public  IEnumerable<BFile>? BFiles { get; set; }
    }
}
