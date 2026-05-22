using System.ComponentModel.DataAnnotations;

namespace EImzaTakip.Models.Dtos.UserDtos
{
    public class GetUserDto
    {
        public int Id { get; set; }

        [Display(Name = "Ad")]
        public string Name { get; set; } 

        [Display(Name = "Soyad")]
        public string Surname { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string NickName { get; set; }

        [Display(Name = "E-posta")]
        public string Email { get; set; } 
    }
}
