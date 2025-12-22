using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnlineCourseD2N.Backend.Data
{
    public class Trainer
    {
        [Key]
        public int TrainerId { get; set; }

        [Required]
        public int UserId { get; set; }

        // Navigasi ke User (Parent)
        [ForeignKey("UserId")]
        public Users? User { get; set; }

        [Required]
        [MaxLength(100)]
        public string Expertise { get; set; } = string.Empty;

        public string Bio { get; set; } = string.Empty;

        public string FotoProfil { get; set; } = string.Empty;

        public List<Course>? Courses { get; set; }
    }
}