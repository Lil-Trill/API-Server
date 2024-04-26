using System.ComponentModel.DataAnnotations;

namespace API_Server.Entities
{
    public class Users
    {
        public int Id { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required] 
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
