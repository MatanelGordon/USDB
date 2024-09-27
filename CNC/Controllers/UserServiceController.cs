using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace CNC.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class UserServiceController(ILogger<UserServiceController> logger) : ControllerBase
    {
        private readonly HashSet<string> _connected = new ();
        
        [HttpGet("Object/{user}",Name = "GetObject")]
        public IActionResult Get(string user, [FromQuery] string id)
        {
            Log("Run Succeed");
            byte[] byteArray = Encoding.UTF8.GetBytes(id);
            return File(byteArray, "application/octet-stream", $"{id}.bin");
        }

        [HttpGet("Register/{user}",Name = "RegisterUser")]
        public IActionResult Register(string user)
        {
            if (_connected.Contains(user))
            {
                return StatusCode(500, $"User ${user} already exists");
            }
            
            _connected.Add(user);
            return Ok($"User has been registered");
        }

        [HttpGet("Unregister/{user}",Name = "UnregisterUser")]
        public IActionResult Unregister(string user)
        {
            if (!_connected.Contains(user))
            {
                return NotFound($"User {user} not found");
            }
            
            _connected.Remove(user);
            return Ok($"User has been registered");
        }

        private void Log(string message)
        {
            var now = DateTime.Now;
            logger.LogInformation($"[{now.ToShortDateString()} {now.ToShortTimeString()}]: {message}");
        }
    }
}
