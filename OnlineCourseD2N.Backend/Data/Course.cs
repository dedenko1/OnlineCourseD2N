using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourseD2N.Backend.Data
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = String.Empty;

        [MaxLength(50)]
        public string Category { get; set; } = String.Empty;

        public int Duration { get; set; } // jam
        public string Level { get; set; } = String.Empty;

        // Relasi ke Trainer
        [ForeignKey("Trainer")]
        public int TrainerId { get; set; }
        public Trainer? Trainer { get; set; }
    }
}
