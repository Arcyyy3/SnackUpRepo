using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/Allergens")]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller
public class AllergenController : ControllerBase
{
    private readonly AllergenService _allergenService;

    public AllergenController(AllergenService allergenService)
    {
        _allergenService = allergenService;
    }

    [HttpGet]
    public IActionResult GetAllAllergens()
    {
        try
        {
            var allergens = _allergenService.GetAllAllergens();
            return Ok(allergens);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    //[HttpGet("{id}")]
    //public IActionResult GetAllergenById(int id)
    //{
    //    try
    //    {
    //        var allergen = _allergenService.GetAllergenById(id);
    //        if (allergen == null)
    //            return NotFound($"Allergen with ID {id} not found.");
    //        return Ok(allergen);
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, "Internal Server Error");
    //    }
    //}

    //[HttpPost]
    //public IActionResult AddAllergen([FromBody] Allergen allergen)
    //{
    //    try
    //    {
    //        _allergenService.AddAllergen(allergen);
    //        return CreatedAtAction(nameof(GetAllergenById), new { id = allergen.AllergenID }, allergen);
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, "Internal Server Error");
    //    }
    //}

    //[HttpPut("{id}")]
    //public IActionResult UpdateAllergen(int id, [FromBody] Allergen allergen)
    //{
    //    try
    //    {
    //        allergen.AllergenID = id;
    //        _allergenService.UpdateAllergen(allergen);
    //        return NoContent();
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, "Internal Server Error");
    //    }
    //}

    //[HttpDelete("{id}")]
    //public IActionResult DeleteAllergen(int id)
    //{
    //    try
    //    {
    //        _allergenService.DeleteAllergen(id);
    //        return NoContent();
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, "Internal Server Error");
    //    }
    //}
}
