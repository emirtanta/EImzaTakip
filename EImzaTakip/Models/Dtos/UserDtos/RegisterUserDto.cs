using System.ComponentModel.DataAnnotations;

namespace EImzaTakip.Models.Dtos.UserDtos
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage ="Ad zorunludur!")]
        [Display(Name ="Ad")]
        [StringLength(100,ErrorMessage ="Ad en fazla 100 karakter uzunluğunda olmalıdır!")]
        [MinLength(3,ErrorMessage ="Ad en az 3 karakter uzunluğunda olmalıdır!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur!")]
        [Display(Name = "Soyad")]
        [StringLength(100, ErrorMessage = "Soyad en fazla 100 karakter uzunluğunda olmalıdır!")]
        [MinLength(2, ErrorMessage = "Soyad en az 2 karakter uzunluğunda olmalıdır!")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur!")]
        [Display(Name = "E-posta")]
        [DataType(DataType.EmailAddress,ErrorMessage ="E-posta uygun formatta değil!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur!")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter uzunluğunda olmalıdır!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrar zorunludur!")]
        [Display(Name = "Şifre Tekrar")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor!")]
        public string ConfirmPassword { get; set; }
    }
}
