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

        public string CoverImage { get; set; } = String.Empty;

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? LocationAddress { get; set; }

        // Relasi ke Trainer
        [ForeignKey("Trainer")]
        public int TrainerId { get; set; }
        public Trainer? Trainer { get; set; }
    }
}
