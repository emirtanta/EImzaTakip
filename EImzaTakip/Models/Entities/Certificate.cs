using System.ComponentModel.DataAnnotations;

namespace EImzaTakip.Models.Entities
{
    public class Certificate
    {
        public int Id { get; set; }

        [Display(Name ="Sertifika adı")]
        public string? CertificateName { get; set; }

        [Display(Name = "Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Bitiş Tarihi")]
        public DateTime ExpirationDate { get; set; }

        [Required(ErrorMessage ="Kişi seçimi zorunludur!")]
        [Display(Name ="Kişi")]
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
