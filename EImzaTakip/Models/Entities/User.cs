using System.ComponentModel.DataAnnotations;

namespace EImzaTakip.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Surname { get; set; }

        [StringLength(100)]
        public string NickName { get; set; }

        [StringLength(100)]
        public string Email { get; set; }
        
        public string Password { get; set; }

        public bool Status { get; set; }


        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
