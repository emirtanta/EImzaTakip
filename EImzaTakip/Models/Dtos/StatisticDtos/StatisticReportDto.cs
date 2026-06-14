using System.ComponentModel.DataAnnotations;

namespace EImzaTakip.Models.Dtos.StatisticDtos
{
    public class StatisticReportDto
    {
        public int PersonId { get; set; }

        [Display(Name = "TC Kimlik No")]
        public string IdentityNumber { get; set; } = string.Empty;

        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Birim")]
        public string DepartmentName { get; set; } = string.Empty;

        [Display(Name = "Sertifika Türü")]
        public string CertificateName { get; set; } = string.Empty;

        [Display(Name = "Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Bitiş Tarihi")]
        public DateTime ExpirationDate { get; set; }
    }
}
