using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller
[Route("api/LoggerClient")]
public class LoggerClientController : ControllerBase
{
    private readonly LoggerClientService _loggerClientService;

    public LoggerClientController(LoggerClientService loggerClientService)
    {
        _loggerClientService = loggerClientService;
    }


    [HttpPost]
    public IActionResult AddLoggerClient([FromBody] LoggerClient loggerclient)
    {
        try
        {
            _loggerClientService.AddLoggerClient(loggerclient);
            return CreatedAtAction(nameof(AddLoggerClient), new { id = loggerclient.ID }, loggerclient);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}
