using EImzaTakip.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace EImzaTakip.Models.Entities
{
    public class Person
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Tc kimlik no zounludur")]
        [Display(Name = "Tc Kimlik No")]
        [StringLength(11,ErrorMessage ="Tc kimlik no en fazla 11 karakter uzunluğunda olmalıdır!")]
        [MinLength(11,ErrorMessage ="Tc kimlik no en az 11 karakter uzunluğunda olmalıdır!")]
        public string IdentityNumber { get; set; }

        [Required(ErrorMessage ="Ad zorunludur!")]
        [Display(Name = "Ad")]
        [StringLength(100,ErrorMessage ="Ad alanı en fazla 100 karakter uzunluğunda olmalıdır!")]
        [MinLength(3,ErrorMessage ="Ad alanı en az 3 karakter uzunluğunda olmalıdır!")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Soyad zorunludur!")]
        [Display(Name = "Soyad")]
        [StringLength(100,ErrorMessage ="Soyad en fazla 100 karakter uzunluğunda olmalıdır!")]
        [MinLength(2,ErrorMessage ="Soyad en az 2 karakter uzunluğunda olmalıdır!")]
        public string Surname { get; set; }

        [Display(Name = "Doğum Tarihi")]
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage ="E-posta adresi zorunludur!")]
        [Display(Name = "E-posta")]
        [StringLength(100)]
        [DataType(DataType.EmailAddress,ErrorMessage ="E-posta adresi uygun formatta değil")]
        public string Email { get; set; }

        [Display(Name ="Birim")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [Required(ErrorMessage ="Başvuru tipi seçiniz!")]
        [Display(Name ="Başvuru Tipi")]
        public RecourseTypeEnum RecourseType { get; set; }

        [Display(Name = "Yedek mi?")]
        public bool YedekMi { get; set; } 
        public SmartCardReaderTypeEnum SmartCardReaderType { get; set; }

        [Display(Name = "Açıklama")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "VIP")]
        public bool VIP { get; set; }

        [Display(Name = "Durum")]
        public bool Status { get; set; }

        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

        public ICollection<PersonNote> PersonNotes { get; set; } = new List<PersonNote>();

    }
}
