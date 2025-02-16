using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller
[Route("api/[controller]")]
public class InventoryHistoryController : ControllerBase
{
    private readonly InventoryHistoryService _historyService;

    public InventoryHistoryController(InventoryHistoryService historyService)
    {
        _historyService = historyService;
    }

    [HttpGet]
    public IActionResult GetAllHistory()
    {
        try
        {
            var history = _historyService.GetAllHistory();
            return Ok(history);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching inventory history: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{productID}")]
    public IActionResult GetHistoryByProductID(int productID)
    {
        try
        {
            var history = _historyService.GetHistoryByProductID(productID);
            if (history == null || !history.Any())
                return NotFound($"No history found for ProductID {productID}.");
            return Ok(history);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching history for ProductID {productID}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("details/{historyID}")]
    public IActionResult GetHistoryByID(int historyID)
    {
        try
        {
            var history = _historyService.GetHistoryByID(historyID);
            if (history == null)
                return NotFound($"No history found for HistoryID {historyID}.");
            return Ok(history);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching history for HistoryID {historyID}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    public IActionResult AddHistory([FromBody] InventoryHistory history)
    {
        try
        {
            _historyService.AddHistory(history);
            return CreatedAtAction(nameof(GetHistoryByID), new { historyID = history.HistoryID }, history);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding inventory history: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{historyID}")]
    public IActionResult SoftDeleteHistory(int historyID)
    {
        try
        {
            _historyService.SoftDeleteHistory(historyID);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting inventory history: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
