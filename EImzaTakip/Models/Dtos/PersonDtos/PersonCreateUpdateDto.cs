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
        [StringLength(
            11,
            MinimumLength = 11,
            ErrorMessage = "Tc kimlik numarası 11 karakter olmalıdır!")]
        [RegularExpression(
            @"^[0-9]{11}$",
            ErrorMessage = "Tc kimlik numarası sadece rakamlardan oluşmalıdır!")]
        public string IdentityNumber { get; set; } = string.Empty;


        [Required(ErrorMessage = "Ad zorunludur!")]
        [Display(Name = "Ad")]
        [StringLength(
            100,
            ErrorMessage = "Ad en fazla 100 karakter olabilir!")]
        [RegularExpression(
            @"^[a-zA-ZçÇğĞıİöÖşŞüÜ\s'-]+$",
            ErrorMessage = "Ad alanında rakam kullanılamaz!")]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "Soyad zorunludur!")]
        [Display(Name = "Soyad")]
        [StringLength(
            100,
            ErrorMessage = "Soyad en fazla 100 karakter olabilir!")]
        [RegularExpression(
            @"^[a-zA-ZçÇğĞıİöÖşŞüÜ\s'-]+$",
            ErrorMessage = "Soyad alanında rakam kullanılamaz!")]
        public string Surname { get; set; } = string.Empty;


        [Display(Name = "Doğum Tarihi")]
        public DateTime Birthdate { get; set; } = DateTime.Today;


        [Required(ErrorMessage = "E-posta zorunludur!")]
        [Display(Name = "E-posta")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz!")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "Birim seçimi zorunludur!")]
        [Display(Name = "Birim")]
        public int DepartmentId { get; set; }


        [Required(ErrorMessage = "Başvuru tipi seçimi zorunludur!")]
        [Display(Name = "Başvuru Tipi")]
        public RecourseTypeEnum RecourseType { get; set; }


        [Display(Name = "Yedek mi?")]
        public bool YedekMi { get; set; }


        [Required(ErrorMessage = "Akıllı kart okuyucu seçimi zorunludur!")]
        [Display(Name = "Akıllı Kart Okuyucu Türü")]
        public SmartCardReaderTypeEnum SmartCardReaderType { get; set; }


        [Display(Name = "Açıklama")]
        public string? Description { get; set; }


        [Display(Name = "VIP")]
        public bool VIP { get; set; }


        [Display(Name = "Durum")]
        public bool Status { get; set; }


        public List<Certificate> Certificates { get; set; }
            = new();


        public List<PersonNote> PersonNotes { get; set; }
            = new();
    }
}
