using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourseD2N.Shared.Models
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Nama wajib diisi")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email wajib diisi")]
        [EmailAddress(ErrorMessage = "Format email tidak valid")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password wajib diisi")]
        [MinLength(6, ErrorMessage = "Password minimal 6 karakter")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Konfirmasi Password wajib diisi")]
        [Compare(nameof(Password), ErrorMessage = "Password tidak cocok")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
