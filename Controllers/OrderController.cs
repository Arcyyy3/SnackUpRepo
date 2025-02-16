using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller
[Route("api/Orders")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public IActionResult GetAllOrders()
    {
        try
        {
            var orders = _orderService.GetAllOrders();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving orders: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpGet("orders/current-recreation/{userId}")]
    public IActionResult GetOrdersByUserIdAndCurrentRecreation(int userId)
    {
        try
        {
            var orders = _orderService.GetOrdersByUserIdAndCurrentRecreation(userId);

            if (!orders.Any())
            {
                return NotFound(new { Message = "No orders found for the current recreation period." });
            }

            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving orders.", Details = ex.Message });
        }
    }

    [HttpGet("OrderById{id}")]
    public IActionResult GetOrderById(int id)
    {
        try
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
                return NotFound($"Order with ID {id} not found.");

            return Ok(order);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving order by ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }



    [HttpPost]
    public IActionResult AddOrder([FromBody] Order order)
    {
        if (order == null)
        {
            return BadRequest("Order cannot be null.");
        }

        try
        {
            int orderId = _orderService.AddOrder(order);
            return Ok(orderId); // Restituisce solo l'ID
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding order: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }


    [HttpGet("GroupedByUser/{userId}")]
    public IActionResult GetGroupedOrdersByUserId(int userId)
    {
        try
        {
            var groupedOrders = _orderService.GetGroupedOrdersByUserId(userId);

            if (groupedOrders == null || !groupedOrders.Any())
                return NotFound($"Nessun ordine trovato per l'utente con ID {userId}.");

            return Ok(groupedOrders);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il recupero degli ordini per l'utente {userId}: {ex.Message}");
            return StatusCode(500, "Errore interno del server. Contatta il supporto tecnico.");
        }
    }
    [HttpPut("{id}")]
    public IActionResult UpdateOrder(int id, [FromBody] Order order)
    {
        try
        {
            order.OrderID = id;
            _orderService.UpdateOrder(order);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating order ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteOrder(int id)
    {
        try
        {
            _orderService.DeleteOrder(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting order ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
