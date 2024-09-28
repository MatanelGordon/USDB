using CNC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;

namespace CNC.Controllers
{
    [ApiController]
    [Route("Api/Object/[controller]")]
    public class StorageController(UsersStorageService usersStorageService,ILogger<StorageController> logger) : ControllerBase
    {
        [HttpGet("{user}/{id}")]
        public IActionResult Get(string user, string id)
        {
            if (!usersStorageService.Exists(user))
            {
                return NotFound($"User {user} was not found");
            }

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
