using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller

[Route("api/ProducerUsers")]
public class ProducerUsersController : ControllerBase
{
    private readonly ProducerUsersService _producerUsersService;

    public ProducerUsersController(ProducerUsersService ProducerUsersService)
    {
        _producerUsersService = ProducerUsersService;
    }

    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        try
        {
            var product = _producerUsersService.GetProducerByID(id);
            if (product == null)
                return NotFound($"Product with ID {id} not found."); // HTTP 404
            return Ok(product); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching product ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}