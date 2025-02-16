using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller
[Route("api/LoggerServer")]
public class LoggerServerController : ControllerBase
{
    private readonly LoggerServerService _loggerServerService;

    public LoggerServerController(LoggerServerService loggerServerService)
    {
        _loggerServerService = loggerServerService;
    }


    [HttpPost]
    public IActionResult AddLoggerServer([FromBody] LoggerServer loggerserver)
    {
        try
        {
            _loggerServerService.AddLoggerServer(loggerserver);
            return CreatedAtAction(nameof(AddLoggerServer), new { id = loggerserver.ID }, loggerserver);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}
