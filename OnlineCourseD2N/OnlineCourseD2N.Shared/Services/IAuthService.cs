using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourseD2N.Shared.Models;

namespace OnlineCourseD2N.Shared.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginDTO model);
        Task<AuthResponse> RegisterAsync(RegisterDTO request);
        Task LogoutAsync();
    }
}
