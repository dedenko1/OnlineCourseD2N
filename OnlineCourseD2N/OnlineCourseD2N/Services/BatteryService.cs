using OnlineCourseD2N.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourseD2N.Services
{
    public class BatteryService : IBatteryService
    {
        public Task<double> CheckLevel()
        {
            // Mengembalikan nilai 0.0 - 1.0 (Contoh: 0.85 artinya 85%)
            return Task.FromResult(Battery.ChargeLevel);
        }

        public string CheckState()
        {
            return Battery.Default.State.ToString();
            // Output: "Charging", "Discharging", "Full", "NotCharging", "Unknown"
        }

        public string CheckPowerSource()
        {
            return Battery.Default.PowerSource.ToString();
            // Output: "Battery", "AC", "Wireless", "Unknown"
        }
    }
}
