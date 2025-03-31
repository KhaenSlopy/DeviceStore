using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceStore.Models
{
    [Table("Login")]
    public class Login
    {
        [Key]
        public Guid LoginID { get; set; }

        [Required]
        [ForeignKey("User")]
        public string EmployeeID { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
        public string Status { get; set; }

        public User? User { get; set; }
    }
}
