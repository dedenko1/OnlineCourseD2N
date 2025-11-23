using Microsoft.AspNetCore.Mvc;
using OnlineCourseD2N.Backend.Handler;

namespace OnlineCourseD2N.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UploadsController : ControllerBase
    {
        [HttpPost]
        public IActionResult UploadFile(IFormFile file, [FromForm] string? oldFileName)
        {
            Console.Write(oldFileName);
            return Ok(new UploadHandler().Upload(file, oldFileName));
        }
    }
}
