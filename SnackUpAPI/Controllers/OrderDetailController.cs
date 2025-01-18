using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;

[ApiController]
[Route("api/OrderDetails")]
public class OrderDetailController : ControllerBase
{
    private readonly OrderDetailService _orderDetailService;

    public OrderDetailController(OrderDetailService orderDetailService)
    {
        _orderDetailService = orderDetailService;
    }

    [HttpGet]
    public IActionResult GetAllOrderDetails()
    {
        try
        {
            var orderDetails = _orderDetailService.GetAllOrderDetails();
            return Ok(orderDetails);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving order details: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("order/{orderId}")]
    public IActionResult GetOrderDetailsByOrderId(int orderId)
    {
        try
        {
            var orderDetails = _orderDetailService.GetOrderDetailsByOrderId(orderId);
            return Ok(orderDetails);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving order details for OrderID {orderId}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    public IActionResult AddOrderDetail([FromBody] OrderDetail orderDetail)
    {
        try
        {
            _orderDetailService.AddOrderDetail(orderDetail);
            return CreatedAtAction(nameof(GetOrderDetailsByOrderId), new { orderId = orderDetail.OrderID }, orderDetail);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding order detail: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateOrderDetail(int id, [FromBody] OrderDetail orderDetail)
    {
        try
        {
            orderDetail.OrderDetailID = id;
            _orderDetailService.UpdateOrderDetail(orderDetail);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating order detail ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{orderDetailId}")]
    public IActionResult DeleteOrderDetail(int orderDetailId)
    {
        try
        {
            _orderDetailService.DeleteOrderDetail(orderDetailId);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting order detail ID {orderDetailId}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
