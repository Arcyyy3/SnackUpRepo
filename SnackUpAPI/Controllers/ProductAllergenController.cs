using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Services;
using SnackUpAPI.Models;
using System;

[ApiController]
[Route("api/ProductAllergens")]
public class ProductAllergenController : ControllerBase
{
    private readonly ProductAllergenService _productAllergenService;

    public ProductAllergenController(ProductAllergenService productAllergenService)
    {
        _productAllergenService = productAllergenService;
    }

    [HttpGet("{productId}")]
    public IActionResult GetAllergensByProductId(int productId)
    {
        try
        {
            var allergens = _productAllergenService.GetAllergensByProductId(productId);
            return Ok(allergens);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    public IActionResult AddProductAllergen([FromBody] ProductAllergen productAllergen)
    {
        try
        {
            _productAllergenService.AddProductAllergen(productAllergen.ProductID, productAllergen.AllergenID);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete]
    public IActionResult RemoveProductAllergen([FromBody] ProductAllergen productAllergen)
    {
        try
        {
            _productAllergenService.RemoveProductAllergen(productAllergen.ProductID, productAllergen.AllergenID);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}
