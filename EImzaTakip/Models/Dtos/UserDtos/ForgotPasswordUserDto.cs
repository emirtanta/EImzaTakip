using System.ComponentModel.DataAnnotations;

namespace EImzaTakip.Models.Dtos.UserDtos
{
    public class ForgotPasswordUserDto
    {
        [Required(ErrorMessage ="E-posta zorunludur!")]
        [Display(Name ="E-posta")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Geçerli bir e-posta adresi giriniz!")]
        public string Email { get; set; }
    }
}
