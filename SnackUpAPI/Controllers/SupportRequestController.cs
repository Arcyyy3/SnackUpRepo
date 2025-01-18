using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;

[ApiController]
[Route("api/SupportRequests")]
public class SupportRequestController : ControllerBase
{
    private readonly SupportRequestService _supportRequestService;

    public SupportRequestController(SupportRequestService supportRequestService)
    {
        _supportRequestService = supportRequestService;
    }

    [HttpGet]
    public IActionResult GetAllSupportRequests()
    {
        try
        {
            var supportRequests = _supportRequestService.GetAllSupportRequests();
            return Ok(supportRequests); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching support requests: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("user/{userId}")]
    public IActionResult GetSupportRequestsByUserId(int userId)
    {
        try
        {
            var supportRequests = _supportRequestService.GetSupportRequestsByUserId(userId);
            return Ok(supportRequests); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching support requests for user ID {userId}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    public IActionResult AddSupportRequest([FromBody] SupportRequest supportRequest)
    {
        try
        {
            _supportRequestService.AddSupportRequest(supportRequest);
            return CreatedAtAction(nameof(GetAllSupportRequests), supportRequest); // HTTP 201
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding support request: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateSupportRequest(int id, [FromBody] SupportRequest supportRequest)
    {
        try
        {
            supportRequest.SupportRequestID = id;
            _supportRequestService.UpdateSupportRequest(supportRequest);
            return NoContent(); // HTTP 204
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating support request ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{supportRequestId}")]
    public IActionResult DeleteSupportRequest(int supportRequestId)
    {
        try
        {
            _supportRequestService.DeleteSupportRequest(supportRequestId);
            return NoContent(); // HTTP 204
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting support request ID {supportRequestId}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
