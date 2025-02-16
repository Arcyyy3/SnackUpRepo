using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller
[Route("api/Bundles")]
public class BundleProductController : ControllerBase
{
    private readonly BundleProductService _bundleProductService;

    public BundleProductController(BundleProductService bundleProductService)
    {
        _bundleProductService = bundleProductService;
    }

    [HttpGet]
    public IActionResult GetAllBundles()
    {
        try
        {
            var bundles = _bundleProductService.GetAllBundles();
            return Ok(bundles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{bundleID}")]
    public IActionResult GetBundleById(int bundleID)
    {
        try
        {
            var bundle = _bundleProductService.GetBundleById(bundleID);
            if (bundle == null)
                return NotFound($"Bundle with ID {bundleID} not found.");
            return Ok(bundle);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    public IActionResult AddBundle([FromBody] BundleProduct bundle)
    {
        try
        {
            _bundleProductService.AddBundle(bundle);
            return CreatedAtAction(nameof(GetBundleById), new { bundleID = bundle.BundleID }, bundle);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPut("{bundleID}")]
    public IActionResult UpdateBundle(int bundleID, [FromBody] BundleProduct bundle)
    {
        try
        {
            bundle.BundleID = bundleID;
            _bundleProductService.UpdateBundle(bundle);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{bundleID}")]
    public IActionResult DeleteBundle(int bundleID)
    {
        try
        {
            _bundleProductService.DeleteBundle(bundleID);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}
