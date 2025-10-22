using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace OnlineCourseD2N.Shared.Models
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
