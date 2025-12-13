using System.ComponentModel.DataAnnotations;

namespace Trandora.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter your name")]
        public string Name { get; set; }
        [Required, EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "Password must be between 5 and 10 characters")]
        public string Password { get; set; }
        public string Role { get; set; } = "User";
    }
}