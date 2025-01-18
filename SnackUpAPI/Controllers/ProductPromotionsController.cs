using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class ProductPromotionController : ControllerBase
{
    private readonly ProductPromotionService _productPromotionService;

    public ProductPromotionController(ProductPromotionService productPromotionService)
    {
        _productPromotionService = productPromotionService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var productPromotions = _productPromotionService.GetAllProductPromotions();
        return Ok(productPromotions);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var productPromotion = _productPromotionService.GetProductPromotionById(id);
        if (productPromotion == null)
        {
            return NotFound($"ProductPromotion with ID {id} not found.");
        }
        return Ok(productPromotion);
    }

    [HttpPost]
    public IActionResult Create([FromBody] ProductPromotion productPromotion)
    {
        if (productPromotion == null)
        {
            return BadRequest("Invalid ProductPromotion data.");
        }

        _productPromotionService.AddProductPromotion(productPromotion);
        return CreatedAtAction(nameof(GetById), new { id = productPromotion.ProductPromotionID }, productPromotion);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ProductPromotion productPromotion)
    {
        if (productPromotion == null || productPromotion.ProductPromotionID != id)
        {
            return BadRequest("Invalid data.");
        }

        _productPromotionService.UpdateProductPromotion(productPromotion);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _productPromotionService.DeleteProductPromotion(id);
        return NoContent();
    }
}
    