using EImzaTakip.Models.Entities;
using EImzaTakip.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace EImzaTakip.Models.Dtos.PersonDtos
{
    public class PersonCreateUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tc kimlik no zorunludur")]
        [Display(Name = "Tc Kimlik No")]
        [StringLength(11)]
        [MinLength(11)]
        public string IdentityNumber { get; set; }

        [Required(ErrorMessage = "Ad zorunludur!")]
        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur!")]
        [Display(Name = "Soyad")]
        public string Surname { get; set; }

        [Display(Name = "Doğum Tarihi")]
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur!")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Display(Name = "Birim")]
        public int DepartmentId { get; set; }

        [Display(Name = "Başvuru Tipi")]
        public RecourseTypeEnum RecourseType { get; set; }

        [Display(Name = "Yedek mi?")]
        public bool YedekMi { get; set; }

        [Display(Name = "Akıllı Kart Okuyucu Türü")]
        public SmartCardReaderTypeEnum SmartCardReaderType { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "VIP")]
        public bool VIP { get; set; }

        [Display(Name = "Durum")]
        public bool Status { get; set; }

        // CERTIFICATES
        public List<Certificate> Certificates { get; set; }
            = new();

        // NOTES
        public List<PersonNote> PersonNotes { get; set; }
            = new();
    }
}
