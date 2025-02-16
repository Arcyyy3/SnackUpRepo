using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/CartItems")]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller
public class CartItemController : ControllerBase
{
    private readonly CartItemService _cartItemService;

    public CartItemController(CartItemService cartItemService)
    {
        _cartItemService = cartItemService;
    }

    [HttpGet("{sessionId}")]
    public IActionResult GetItemsBySessionId(int sessionId)
    {
        try
        {
            var items = _cartItemService.GetItemsBySessionId(sessionId);
            return Ok(items);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching cart items for session {sessionId}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpGet("SessionIDByUserID{userID}")]
    public IActionResult SessionIDByUserID(int userID)
    {
        try
        {
            int items = _cartItemService.SessionIDByUserID(userID);
            return Ok(items);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching cart items for session {userID}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [HttpGet("PROVA{sessionId}")]
    public IActionResult GetItemsBySessionIdAAA(int sessionId)
    {
        try
        {
            var items = _cartItemService.GetItemsBySessionIdAAA(sessionId);
            return Ok(items);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching cart items for session {sessionId}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpGet("GetCartProduct/{userID}")]
    public IActionResult GetCartProduct(int userID)
    {
        try
        {
            var items = _cartItemService.GetCartProduct(userID);
            return Ok(items);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching cart items for userID {userID}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [HttpGet("Total/{userID}")]
    public IActionResult GetTotalPrice(int userID)
    {
        try
        {
            var items = _cartItemService.GetTotalPrice(userID);
            return Ok(new { Items=items });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching cart items for GetTotalPrice {userID}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpPost]
    public IActionResult AddOrUpdateItem([FromBody] CartItem item)
    {
        try
        {
            _cartItemService.AddOrUpdateItem(item.SessionID, item.ProductID, item.Quantity, item.Price);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding/updating cart item: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("Remove{cartItemId}")]
    public IActionResult RemoveItem(int cartItemId)
    {
        try
        {
            _cartItemService.RemoveItem(cartItemId);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting cart item ID {cartItemId}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpDelete("{cartName}/{userID}")]
    public IActionResult RemoveItemFromName(string cartName, int userID)
    {
        Console.WriteLine($"Request received for cartName: {cartName}, SessionID: {userID}");
        try
        {
            _cartItemService.RemoveItemFromName(cartName, userID);
            Console.WriteLine($"Successfully deleted cart item: {cartName} for SessionID: {userID}");
            return NoContent(); // Risposta standard 204 se l'eliminazione è riuscita
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante l'eliminazione del prodotto '{cartName}': {ex.Message}");
            return StatusCode(500, "Errore interno del server");
        }
    }

    [HttpPost("add-to-cart")]
    public IActionResult AddToCart([FromBody] AddToCartPayload request)
    {
        try
        {
            // Valida il payload
            if (string.IsNullOrEmpty(request.ProductName) || request.UserID <= 0 || request.Quantity <= 0 || request.Price <= 0)
            {
                return BadRequest("Il payload non è valido.");
            }

            // Chiama il servizio per aggiungere l'elemento al carrello
            _cartItemService.AddItemToCartFromPayload(request.ProductName, request.Quantity, request.UserID, request.Price);

            return Ok("Prodotto aggiunto al carrello con successo.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Errore durante l'aggiunta al carrello: {ex.Message}");
        }
    }

    [HttpPut("update-parameters")]
    public IActionResult UpdateCartItemParameters([FromBody] UpdateCartItemParametersPayload payload)
    {
        try
        {
            _cartItemService.UpdateCartItemParameters(payload.SessionID, payload.ProductID, payload.DeliveryDate, payload.Recreation);
            return Ok("Parametri aggiornati correttamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore nell'aggiornamento dei parametri: {ex.Message}");
            return StatusCode(500, $"Errore: {ex.Message}");
        }
    }
    public class AddToCartPayload
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int UserID { get; set; }
        public decimal Price { get; set; }
    }
    // Definisci il DTO per il payload:
    public class UpdateCartItemParametersPayload
    {
        public int SessionID { get; set; }
        public int ProductID { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Recreation { get; set; }
    }



}
