using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller

[Route("api/Wallet")]
public class WalletController : ControllerBase
{
    private readonly WalletService _walletService;

    public WalletController(WalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet("{userId}")]
    public IActionResult GetWalletByUserId(int userId)
    {
        try
        {
            var wallet = _walletService.GetWalletByUserId(userId);
            if (wallet == null)
                return NotFound("Wallet not found.");
            return Ok(wallet);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    [HttpGet("WithName/{userId}")]
    public IActionResult GetWalletByUserIdWithName(int userId)
    {
        try
        {
            var wallet = _walletService.GetWalletByUserIdWithName(userId);
            if (wallet == null)
                return NotFound("Wallet not found.");
            return Ok(wallet);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpGet("{walletId}/transactions")]
    public IActionResult GetTransactionsByWalletId(int walletId)
    {
        try
        {
            var transactions = _walletService.GetTransactionsByWalletId(walletId);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("{walletId}/add-funds")]
    public IActionResult AddFunds(int walletId, [FromBody] decimal amount)
    {
        try
        {
            _walletService.AddFunds(walletId, amount, "Deposit");
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("{walletId}/deduct-funds")]
    public IActionResult DeductFunds(int walletId, [FromBody] decimal amount)
    {
        try
        {
            _walletService.DeductFunds(walletId, amount, "Purchase");
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    [HttpPost("{userId}/create-wallet")]
    public IActionResult CreateWallet(int userId)
    {
        try
        {
            _walletService.CreateWallet(userId);
            return CreatedAtAction(nameof(GetWalletByUserId), new { userId = userId }, new { message = "Wallet created successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

}
