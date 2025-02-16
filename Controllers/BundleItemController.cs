using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;


[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller
[Route("api/BundleItems")]
public class BundleItemController : ControllerBase
{
    private readonly BundleItemService _bundleItemService;

    public BundleItemController(BundleItemService bundleItemService)
    {
        _bundleItemService = bundleItemService;
    }

    [HttpGet]
    public IActionResult GetAllBundleItems()
    {
        try
        {
            var bundleItems = _bundleItemService.GetAllBundleItems();
            return Ok(bundleItems);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("bundle/{bundleID}")]
    public IActionResult GetItemsByBundleID(int bundleID)
    {
        try
        {
            var items = _bundleItemService.GetItemsByBundleID(bundleID);
            return Ok(items);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{bundleItemID}")]
    public IActionResult GetBundleItemById(int bundleItemID)
    {
        try
        {
            var bundleItem = _bundleItemService.GetBundleItemById(bundleItemID);
            if (bundleItem == null)
                return NotFound($"Bundle item with ID {bundleItemID} not found.");
            return Ok(bundleItem);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    public IActionResult AddBundleItem([FromBody] BundleItem bundleItem)
    {
        try
        {
            _bundleItemService.AddBundleItem(bundleItem);
            return CreatedAtAction(nameof(GetBundleItemById), new { bundleItemID = bundleItem.BundleItemID }, bundleItem);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPut("{bundleItemID}")]
    public IActionResult UpdateBundleItem(int bundleItemID, [FromBody] BundleItem bundleItem)
    {
        try
        {
            bundleItem.BundleItemID = bundleItemID;
            _bundleItemService.UpdateBundleItem(bundleItem);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{bundleItemID}")]
    public IActionResult DeleteBundleItem(int bundleItemID)
    {
        try
        {
            _bundleItemService.DeleteBundleItem(bundleItemID);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}
