using System.ComponentModel.DataAnnotations;

namespace EImzaTakip.Models.Dtos.UserDtos
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "E-posta zorunludur!")]
        [Display(Name = "E-posta")]
        [DataType(DataType.EmailAddress,ErrorMessage ="E-posta uygun formatta değil!")]
        [MaxLength(100, ErrorMessage = "E-posta en fazla 100 karakter uzunluğunda olmalıdır!")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Şifre zorunludur!")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
