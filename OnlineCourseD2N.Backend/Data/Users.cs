using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineCourseD2N.Backend.Data
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [JsonIgnore]
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        public Trainer? TrainerProfile { get; set; }
    }
}