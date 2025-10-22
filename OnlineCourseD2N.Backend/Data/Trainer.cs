using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace OnlineCourseD2N.Backend.Data
{
    public class Trainer
    {
        public int TrainerId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = String.Empty;

        [MaxLength(100)]
        public string Expertise { get; set; } = String.Empty;

        public string Bio { get; set; } = String.Empty;

        [EmailAddress]
        public string Email { get; set; } = String.Empty;

        public List<Course>? Courses { get; set; }
    }
}
