using System.Text;
using CNC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CNC.Controllers
{
    
    [ApiController]
    [Route("Api/User/[controller]")]
    public class UserServiceController(UsersStorageService usersStorageService,ILogger<UserServiceController> logger) : ControllerBase
    {

        [HttpGet("Register")]
        public IActionResult Register(string user)
        {
            var result = usersStorageService.Register(user);

            if (!result)
            {
                Log($"Unsuccessful Registration - {user}");
                return StatusCode(500, $"Could not register {user}");
            }
            
            Log($"Successful Registration - {user}");
            return Ok($"User {user} has been registered");
        }

        [HttpGet("Unregister")]
        public IActionResult Unregister(string user)
        {
            var result = usersStorageService.Unregister(user);

            if (!result)
            {
                Log($"Unsuccessful Unregistration - {user}");
                return StatusCode(500, $"Could not unregister {user}");
            }
            
            Log($"Successful Unregistration - {user}");
            return Ok($"User {user} has been unregistered");
        }

        [HttpGet("Clear")]
        public IActionResult ClearAll()
        {
            usersStorageService.Clear();
            return Ok($"User Storage has been cleared");
        }

        private void Log(string message)
        {
            var now = DateTime.Now;
            logger.LogInformation($"[{now.ToShortDateString()} {now.ToShortTimeString()}]: {message}");
        }
    }
}
