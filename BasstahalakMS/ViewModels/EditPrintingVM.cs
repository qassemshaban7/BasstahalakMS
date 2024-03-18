using System.ComponentModel.DataAnnotations;

namespace BasstahalakMS.ViewModels
{
    public class EditPrintingVM
    {
        public string Id { get; set; }  
        [Required]
        public string? FullName { get; set; }
        [Required]
        public string? UserName { get; set; }

        //[Required(ErrorMessage = "كلمة المرور  مطلوبه")]
        //[StringLength(100, ErrorMessage = "الرقم السري يجب ان لا يقل عن 6 خانات", MinimumLength = 6)]
        //[DataType(DataType.Password, ErrorMessage = "الرقم السري يجب ان يتكون من ارقام وحروف")]
        //public string? Password { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Address { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "رقم الموبايل يجب أن يحتوي على أرقام فقط")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        [Required]
        public int? Type { get; set; }
    }
}
