using System.ComponentModel.DataAnnotations;

namespace EImzaTakip.Models.Dtos.UserDtos
{
    public class UserProfileDto
    {
        public int Id { get; set; }

        [Display(Name = "Ad")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Soyad")]
        public string Surname { get; set; } = string.Empty;

        [Display(Name = "Kullanıcı Adı")]
        public string NickName { get; set; } = string.Empty;

        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        public int RoleId { get; set; }

        [Display(Name = "Rolü")]
        public string RoleName { get; set; } = string.Empty;

        [Display(Name = "Ad Soyad")]
        public string FullName
        {
            get
            {
                return $"{Name} {Surname}";
            }
        }
    }
}
