using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller

[Route("api/Promotions")]
public class PromotionController : ControllerBase
{
    private readonly PromotionService _promotionService;

    public PromotionController(PromotionService promotionService)
    {
        _promotionService = promotionService;
    }

    [HttpGet]
    public IActionResult GetAllPromotions()
    {
        try
        {
            var promotions =  _promotionService.GetAllPromotions();
            return Ok(promotions); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching promotions: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpGet("GetActivePromotions")]
    public IActionResult GetActivePromotions()
    {
        try
        {
            var promotions = _promotionService.GetActivePromotions();

            if (promotions == null ||
                (!promotions.BundlePromotions.Any() && !promotions.RegularPromotions.Any()))
            {
                return NotFound(new { message = "No active promotions found." }); // HTTP 404
            }

            return Ok(promotions); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching promotions: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    public IActionResult AddPromotion([FromBody] PromotionForProductRequest request)
    {
        try
        {
            _promotionService.AddPromotionForProductsByName(request);

            // HTTP 201 Created
            return CreatedAtAction(
                nameof(GetAllPromotions),  // Sostituisci con il nome del metodo che restituisce le promozioni
                new { /* eventualmente includi qualche identificatore */ },
                request
            );
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message); // 404 se almeno un prodotto non esiste
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message); // 400 se c'è un overlap nelle date
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
