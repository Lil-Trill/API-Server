using System.ComponentModel.DataAnnotations;

namespace API_Server.Entities
{
    public class Users
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
