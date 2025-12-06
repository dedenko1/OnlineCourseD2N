using OnlineCourseD2N.Shared.Services;

namespace OnlineCourseD2N.Web.Services
{
    public class ConnectivityServiceWeb : IConnectivityService
    {
        public bool IsConnected()
        {
            // Anggap selalu ada internet kalau di web
            return true;
        }
    }
}
