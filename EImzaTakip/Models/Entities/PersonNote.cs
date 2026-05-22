using System.ComponentModel.DataAnnotations;

namespace EImzaTakip.Models.Entities
{
    public class PersonNote
    {
        public int Id { get; set; }

        [Display(Name = "Açıklama")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage ="Kişi seçimi zorunludur!")]
        [Display(Name ="Kişi")]
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
