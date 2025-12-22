using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourseD2N.Shared.Models
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty; // JWT Token
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
