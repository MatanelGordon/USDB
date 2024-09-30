using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using CNC.Communicators.Abstraction;
using CNC.Services;
using Common.Compression.Abstraction;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace CNC.Controllers;

[ApiController]
[Route("Api/Object/[controller]")]
public class StorageController(UsersStorageService usersStorageService, ICommunicator communicator, ICompression compression, ILogger<StorageController> logger)
    : ControllerBase
{
    [HttpGet("{user}/{id}")]
    public async Task<IActionResult> Get(string user, string id)
    {
        if (!usersStorageService.Exists(user))
        {
            return NotFound($"User {user} was not found");
        }
        
        var validIdPattern = "^[a-zA-Z0-9_-]+$";
        
        if (!Regex.IsMatch(id, validIdPattern))
        {
            return StatusCode(400, $"Id contains invalid characters");
        }

        var request = new RequestSchema()
        {
            Id = id,
            Method = RequestMethod.GET,
            From = Dns.GetHostName(),
        };

        var result = await communicator.MakeRequest(user, request);

        if (result.ResponseStatus != ResponseStatus.Success)
        {
            var message = Encoding.UTF8.GetString(result.Content);

            return StatusCode(500, $"ResponseSchemaError: {message}");
        }
        
        // TODO: Add Decompression and MimeType Checker for Json XML and UTF8 and UTF16.        
        Log("Run Succeed");
        return File(result.Content, "application/octet-stream", $"{result.Id}.{compression.Extension}");
    }

    private void Log(string message)
    {
        var now = DateTime.Now;
        logger.LogInformation($"[{now.ToShortDateString()} {now.ToShortTimeString()}]: {message}");
    }
}