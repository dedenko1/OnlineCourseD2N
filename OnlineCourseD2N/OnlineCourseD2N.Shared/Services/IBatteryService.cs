using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourseD2N.Shared.Services
{
    public interface IBatteryService
    {
        Task<double> CheckLevel(); // Ubah jadi Task
        string CheckState();
        string CheckPowerSource();
    }
}
