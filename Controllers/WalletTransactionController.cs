using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller

[Route("api/WalletTransactions")]
public class WalletTransactionController : ControllerBase
{
    private readonly WalletTransactionService _transactionService;

    public WalletTransactionController(WalletTransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    // Ottieni tutte le transazioni
    [HttpGet]
    public IActionResult GetAllTransactions()
    {
        try
        {
            var transactions = _transactionService.GetAllTransactions();
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    // Ottieni una transazione per ID
    [HttpGet("{id}")]
    public IActionResult GetTransactionById(int id)
    {
        try
        {
            var transaction = _transactionService.GetTransactionById(id);
            if (transaction == null)
                return NotFound($"Transaction with ID {id} not found.");
            return Ok(transaction);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    // Aggiungi una nuova transazione
    [HttpPost]
    public IActionResult AddTransaction([FromBody] WalletTransaction transaction)
    {
        try
        {
            _transactionService.AddTransaction(transaction);
            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.TransactionID }, transaction);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    // Ottieni tutte le transazioni di un Wallet specifico
    [HttpGet("wallet/{walletId}")]
    public IActionResult GetTransactionsByWalletId(int walletId)
    {
        try
        {
            var transactions = _transactionService.GetTransactionsByWalletId(walletId);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    // Cancella logicamente una transazione
    [HttpDelete("{id}")]
    public IActionResult DeleteTransaction(int id)
    {
        try
        {
            _transactionService.DeleteTransaction(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}
