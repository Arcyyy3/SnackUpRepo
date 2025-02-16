using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller
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
    [HttpGet("GetTotal/{date}")]
    public IActionResult GetTotalPrice(DateTime date)
    {
        try
        {
            var totalPrice = _orderDetailService.GetTotalPrice(date);
            return Ok(totalPrice); // Restituisce il totale con un codice 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving total price: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpGet("GetDetails/{date}/{recreation}")]
    public IActionResult GetDetailsProducer(DateTime date, string recreation)
    {
        try
        {
            var details = _orderDetailService.GetDetailsProducer(date, recreation);

            if (details == null || !details.Any())
            {
                return NotFound(new { message = "No data found for the specified date." });
            }

            return Ok(details);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
        }
    }
    [HttpGet("GetDetailsAll/{date}")]
    public IActionResult GetDetailsProducerAll(DateTime date)
    {
        try
        {
            var details = _orderDetailService.GetDetailsProducerAll(date);

            if (details == null || !details.Any())
            {
                return NotFound(new { message = "No data found for the specified date." });
            }

            return Ok(details);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
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


    [HttpGet("GetProductCodes")]
    public IActionResult GetProductCodesByClassAndDate(DateTime date, int classYear, string classSection, string productName)
    {
        try
        {
            var productCodes = _orderDetailService.GetProductCodesByClassAndDate(date, classYear, classSection, productName);

            if (productCodes == null || !productCodes.Any())
            {
                return NotFound(new { message = "No product codes found for the specified criteria." });
            }

            return Ok(productCodes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while processing your request.",
                details = ex.Message
            });
        }
    }

    [HttpPost]
    public IActionResult AddOrderDetail([FromBody] OrderDetail orderDetail)
    {
        if (orderDetail == null)
        {
            return BadRequest("Order detail cannot be null.");
        }

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
        if (orderDetail == null)
        {
            return BadRequest("Order detail cannot be null.");
        }

        try
        {
            orderDetail.DetailID = id;
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
