using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasstahalakMS.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public string? Address { get; set; } // عنوان المطبعة

        public int? Type { get; set; } // نوع المطبعة 

        public double? TotalMoney { get; set; }  //الخزنة 

        public ICollection<BFile> BFiles { get; set; }
        public ICollection<Library> Libraries { get; set; }
    }
}
