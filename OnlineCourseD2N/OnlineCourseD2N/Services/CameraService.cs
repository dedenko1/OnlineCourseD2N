using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourseD2N.Shared.Services;

namespace OnlineCourseD2N.Services
{
    public class CameraService : ICameraService
    {
        public async Task<Stream?> TakePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo == null) return null;

                return await photo.OpenReadAsync();
            }
            catch
            {
                return null;
            }
        }
    }
}
