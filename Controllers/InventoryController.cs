using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly InventoryService _inventoryService;

    public InventoryController(InventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public IActionResult GetAllInventories()
    {
        try
        {
            var inventories = _inventoryService.GetAllInventories();
            return Ok(inventories);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching inventories: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{productID}")]
    public IActionResult GetInventoryByProductID(int productID)
    {
        try
        {
            var inventory = _inventoryService.GetInventoryByProductID(productID);
            if (inventory == null)
                return NotFound($"Inventory not found for ProductID {productID}.");
            return Ok(inventory);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching inventory for ProductID {productID}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("update")]
    public IActionResult UpdateInventory([FromBody] InventoryUpdateRequest request)
    {
        try
        {
            _inventoryService.UpdateInventory(request.ProductID, request.QuantityChange, request.Reason, request.ChangedBy);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating inventory: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}

public class InventoryUpdateRequest
{
    public int ProductID { get; set; }
    public int QuantityChange { get; set; }
    public string Reason { get; set; }
    public string ChangedBy { get; set; }
}
