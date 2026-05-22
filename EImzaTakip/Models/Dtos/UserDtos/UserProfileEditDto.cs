using System.ComponentModel.DataAnnotations;

namespace EImzaTakip.Models.Dtos.UserDtos
{
    public class UserProfileEditDto
    {
        [Required(ErrorMessage = "Ad zorunludur!")]
        [Display(Name = "Ad")]
        [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter uzunluğunda olmalıdır!")]
        [MinLength(2, ErrorMessage = "Ad en az 2 karakter uzunluğunda olmalıdır!")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad zorunludur!")]
        [Display(Name = "Soyad")]
        [StringLength(100, ErrorMessage = "Soyad en fazla 100 karakter uzunluğunda olmalıdır!")]
        [MinLength(2, ErrorMessage = "Soyad en az 2 karakter uzunluğunda olmalıdır!")]
        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kullanıcı adı zorunludur!")]
        [Display(Name = "Kullanıcı Adı")]
        [StringLength(100, ErrorMessage = "Kullanıcı adı en fazla 100 karakter uzunluğunda olmalıdır!")]
        [MinLength(3, ErrorMessage = "Kullanıcı adı en az 3 karakter uzunluğunda olmalıdır!")]
        public string NickName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta adresi zorunludur!")]
        [Display(Name = "E-posta")]
        [StringLength(100, ErrorMessage = "E-posta adresi en fazla 100 karakter uzunluğunda olmalıdır!")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz!")]
        public string Email { get; set; } = string.Empty;
    }
}
