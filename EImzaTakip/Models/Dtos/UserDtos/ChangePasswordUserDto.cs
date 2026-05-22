using System.ComponentModel.DataAnnotations;

namespace EImzaTakip.Models.Dtos.UserDtos
{
    public class ChangePasswordUserDto
    {
        [Required(ErrorMessage ="Mevcut şifre zorunludur!")]
        [Display(Name ="Mevcut Şifre")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre zorunludur!")]
        [Display(Name = "Yeni Şifre")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="Yeni şifre en az 6 karakter uzunluğunda olmak zorundadır!")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage ="Şifre tekrar zorunludur!")]
        [Display(Name ="Yeni Şifre Tekrar")]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage ="Şifreler uyuşmuyor")]
        public string ConfirmNewPassword { get; set; }
    }
}
