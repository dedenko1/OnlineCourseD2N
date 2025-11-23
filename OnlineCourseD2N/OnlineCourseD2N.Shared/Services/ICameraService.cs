using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourseD2N.Shared.Services
{
    public interface ICameraService
    {
        Task<Stream?> TakePhotoAsync();
    }
}
