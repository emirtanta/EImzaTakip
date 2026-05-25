using System.ComponentModel.DataAnnotations;

namespace EImzaTakip.Models.Entities
{
    public class Role
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Rol adı zorunludur!")]
        [Display(Name ="Rol Adı")]
        [StringLength(100,ErrorMessage ="Rol adı en fazla 100 karakter olmalıdır!")]
        public string Name { get; set; }

        public bool Status { get; set; }

        public ICollection<User> Users { get; set; }= new List<User>();
    }
}
