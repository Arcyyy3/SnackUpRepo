using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;

[ApiController]
[Route("api/ShoppingSessions")]
public class ShoppingSessionController : ControllerBase
{
    private readonly ShoppingSessionService _sessionService;

    public ShoppingSessionController(ShoppingSessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpGet]
    public IActionResult GetAllSessions()
    {
        try
        {
            var sessions = _sessionService.GetAllSessions();
            return Ok(sessions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching sessions: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetSessionById(int id)
    {
        try
        {
            var session = _sessionService.GetSessionById(id);
            if (session == null)
                return NotFound($"Session with ID {id} not found.");
            return Ok(session);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching session ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpGet("active/{userId}")]
    public IActionResult GetActiveSession(int userId)
    {
        try
        {
            var session = _sessionService.GetActiveSessionForUser(userId);
            if (session == null)
                return NotFound("Nessuna sessione attiva trovata.");

            return Ok(session.SessionID);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il recupero della sessione attiva: {ex.Message}");
            return StatusCode(500, "Errore interno del server.");
        }
    }
    [HttpGet("user/{userId}")]
    public ActionResult<ShoppingSession> GetOrCreateActiveSession(int userId)
    {
        try
        {
            var session = _sessionService.GetOrCreateActiveSessionForUser(userId);

            if (session == null)
            {
                return NotFound(new { Message = "Impossibile creare una sessione per l'utente." });
            }

            return Ok(session);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Errore durante l'elaborazione della richiesta.", Error = ex.Message });
        }
    }
    [HttpPost]
    public IActionResult CreateSession([FromBody] int? userId)
    {
        try
        {
            var sessionId = _sessionService.CreateSession(userId);
            return CreatedAtAction(nameof(GetSessionById), new { id = sessionId }, new { SessionID = sessionId });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating session: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPatch("{id}/update-total")]
    public IActionResult UpdateSessionTotal(int id)
    {
        try
        {
            _sessionService.UpdateSessionTotal(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating session total for ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteSession(int id)
    {
        try
        {
            _sessionService.DeleteSession(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting session ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
