using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace CNC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserServiceController(ILogger<UserServiceController> logger) : ControllerBase
    {
        [HttpGet("{user}",Name = "GetObject")]
        public IActionResult Get(string user, [FromQuery] string id)
        {
            Log("Run Succeed");
            byte[] byteArray = Encoding.UTF8.GetBytes(id);
            return File(byteArray, "application/octet-stream", $"{id}.bin");
        }

        private void Log(string message)
        {
            var now = DateTime.Now;
            logger.LogInformation($"[{now.ToShortDateString()} {now.ToShortTimeString()}]: {message}");
        }
    }
}
