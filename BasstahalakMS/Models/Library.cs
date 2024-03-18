using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasstahalakMS.Models
{
    public class Library
    {
        [Key]
        public int LibraryId { get; set; }  
        public int Color { get; set; }
        public int Count { get; set; }
        public int PriceOfUnit { get; set; }
        public int Total { get; set; }

        public int PrintTypeId { get; set; }
        [ForeignKey("PrintTypeId")]
        public PrintType? PrintType { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
