using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller
[Route("api/OrdersTracking")]
public class OrderTrackingController : ControllerBase
{
    private readonly OrderTrackingService _orderTrackingService;

    public OrderTrackingController(OrderTrackingService orderTrackingService)
    {
        _orderTrackingService = orderTrackingService;
    }

    [HttpGet]
    public IActionResult GetAllOrderTrackings()
    {
        try
        {
            var trackings = _orderTrackingService.GetAllOrderTrackings();
            return Ok(trackings);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving order trackings: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("order/{orderId}")]
    public IActionResult GetTrackingByOrderId(int orderId)
    {
        try
        {
            var tracking = _orderTrackingService.GetTrackingByOrderId(orderId);
            if (tracking == null) return NotFound($"Tracking for OrderID {orderId} not found.");
            return Ok(tracking);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving tracking for OrderID {orderId}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    public IActionResult AddOrderTracking([FromBody] OrderTracking orderTracking)
    {
        try
        {
            _orderTrackingService.AddOrderTracking(orderTracking);
            return CreatedAtAction(nameof(GetTrackingByOrderId), new { orderId = orderTracking.OrderID }, orderTracking);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding order tracking: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateOrderTracking(int id, [FromBody] OrderTracking orderTracking)
    {
        try
        {
            orderTracking.TrackingID = id;
            _orderTrackingService.UpdateOrderTracking(orderTracking);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating tracking ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{trackingId}")]
    public IActionResult DeleteOrderTracking(int trackingId)
    {
        try
        {
            _orderTrackingService.DeleteOrderTracking(trackingId);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting tracking ID {trackingId}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
