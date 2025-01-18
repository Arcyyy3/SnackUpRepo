using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;

[ApiController]
[Route("api/CartItems")]
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





}
