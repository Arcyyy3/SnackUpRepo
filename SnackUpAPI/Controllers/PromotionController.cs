using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;

[ApiController]
[Route("api/Promotions")]
public class PromotionController : ControllerBase
{
    private readonly PromotionService _promotionService;

    public PromotionController(PromotionService promotionService)
    {
        _promotionService = promotionService;
    }

    [HttpGet]
    public IActionResult GetAllPromotions([FromQuery] bool? active)
    {
        try
        {
            var promotions = active.HasValue && active.Value
                ? _promotionService.GetActivePromotions()
                : _promotionService.GetAllPromotions();
            return Ok(promotions); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching promotions: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    public IActionResult AddPromotion([FromBody] Promotion promotion)
    {
        try
        {
            _promotionService.AddPromotion(promotion);
            return CreatedAtAction(nameof(GetAllPromotions), new { id = promotion.PromotionID }, promotion); // HTTP 201
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message); // HTTP 400
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding promotion: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdatePromotion(int id, [FromBody] Promotion promotion)
    {
        try
        {
            promotion.PromotionID = id;
            _promotionService.UpdatePromotion(promotion);
            return NoContent(); // HTTP 204
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message); // HTTP 400
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating promotion ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{promotionId}")]
    public IActionResult DeletePromotion(int promotionId)
    {
        try
        {
            _promotionService.DeletePromotion(promotionId);
            return NoContent(); // HTTP 204
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting promotion ID {promotionId}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
