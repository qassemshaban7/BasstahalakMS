using System.ComponentModel.DataAnnotations;

namespace BasstahalakMS.ViewModels
{
    public class EditUserVM
    {
        public string Id { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public string? UserName { get; set; }

        

        [RegularExpression("^[0-9]*$", ErrorMessage = "رقم الموبايل يجب أن يحتوي على أرقام فقط")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        [Required]
        public int? IsAdmin { get; set; }
    }
}
