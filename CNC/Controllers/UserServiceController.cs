using Microsoft.AspNetCore.Mvc;

namespace CNC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserServiceController : ControllerBase
    {

        private readonly ILogger<UserServiceController> _logger;

        public UserServiceController(ILogger<UserServiceController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetObject")]
        public IEnumerable<WeatherForecast> Get()
        {
            Log("Run Succeed");

            
        }

        private void Log(string message)
        {
            var now = DateTime.Now;
            _logger.LogInformation($"[{now.ToShortDateString()} {now.ToShortTimeString()}]: {message}");
        }
    }
}
