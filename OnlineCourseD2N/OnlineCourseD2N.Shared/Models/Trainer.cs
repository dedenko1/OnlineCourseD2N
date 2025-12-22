using System.ComponentModel.DataAnnotations;

namespace OnlineCourseD2N.Shared.Models
{
    public class Trainer
    {
        public int TrainerId { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Nama wajib diisi")]
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Keahlian wajib diisi")]
        public string Expertise { get; set; } = string.Empty;

        public string Bio { get; set; } = string.Empty;

        public string FotoProfil { get; set; } = string.Empty;

        public List<Course>? Courses { get; set; }
    }
}