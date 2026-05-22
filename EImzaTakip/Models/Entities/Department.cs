using System.ComponentModel.DataAnnotations;

namespace EImzaTakip.Models.Entities
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Birim adı zorunludur")]
        [Display(Name ="Birim Adı")]
        [StringLength(150,ErrorMessage ="Birim adı en fazla 150 karakter uzunluğunda olmalıdır!")]
        public string Name { get; set; }

        public ICollection<Person> Persons { get; set; }
    }
}
