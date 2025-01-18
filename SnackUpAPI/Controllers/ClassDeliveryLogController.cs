using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;

[ApiController]
[Route("api/ClassDeliveryLogs")]
public class ClassDeliveryLogController : ControllerBase
{
    private readonly ClassDeliveryLogService _service;

    public ClassDeliveryLogController(ClassDeliveryLogService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            var logs = _service.GetAll();
            return Ok(logs);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving delivery logs: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("ByCode/{classDeliveryCodeId}")]
    public IActionResult GetLogsByClassDeliveryCodeId(int classDeliveryCodeId)
    {
        string currentMoment = Utility.GetCurrentMoment();

        try
        {
            var logs = _service.GetLogsByClassDeliveryCodeId(classDeliveryCodeId, currentMoment);
            return Ok(logs);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving logs for delivery code ID {classDeliveryCodeId}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    public IActionResult Add([FromBody] ClassDeliveryLog log)
    {
        try
        {
            _service.Add(log);
            return Ok(log);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding delivery log: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
