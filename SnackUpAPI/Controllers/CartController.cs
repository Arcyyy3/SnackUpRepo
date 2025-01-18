using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;

[ApiController]
[Route("api/Cart")]
public class CartController : ControllerBase
{
	private readonly CartItemService _cartItemService;

	public CartController(CartItemService cartItemService)
	{
		_cartItemService = cartItemService;
	}

	// Endpoint per aggiungere un prodotto al carrello
	[HttpPost("add")]
	public IActionResult AddToCart([FromBody] AddToCartRequest request)
	{
		if (request == null || request.SessionID <= 0 || request.ProductID <= 0 || request.Quantity <= 0)
			return BadRequest("Dati della richiesta non validi.");

		try
		{
			_cartItemService.AddOrUpdateItem(request.SessionID, request.ProductID, request.Quantity, request.Price);
			return Ok("Prodotto aggiunto al carrello.");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Errore durante l'aggiunta al carrello: {ex.Message}");
			return StatusCode(500, "Errore interno del server.");
		}
	}
    // Endpoint per aggiornare la quantit� di un prodotto nel carrello
    [HttpPut("update-quantity")]
    public IActionResult UpdateQuantity([FromBody] UpdateCartQuantityRequest request)
    {
        if (request == null || request.SessionID <= 0 || request.ProductID <= 0 || request.Quantity < 0)
            return BadRequest("Dati della richiesta non validi.");

        try
        {
            _cartItemService.UpdateItemQuantity(request.SessionID, request.ProductID, request.Quantity);
            return Ok("Quantit� aggiornata nel carrello.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante l'aggiornamento della quantit�: {ex.Message}");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    // Endpoint per recuperare gli articoli di una sessione
    [HttpGet("{sessionId}")]
	public IActionResult GetCartItems(int sessionId)
	{
		try
		{
			var items = _cartItemService.GetItemsBySessionId(sessionId);
			return Ok(items);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Errore durante il recupero degli articoli del carrello: {ex.Message}");
			return StatusCode(500, "Errore interno del server.");
		}
	}
}
