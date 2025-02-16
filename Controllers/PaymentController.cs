using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace SnackUpAPI.Controllers
{
    [ApiController]
    [Authorize]  // ✅ Protegge tutti gli endpoint di questo controller
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // Ottieni tutti i pagamenti
        [HttpGet]
        public ActionResult<IEnumerable<Payment>> GetAllPayments()
        {
            try
            {
                var payments = _paymentService.GetAllPayments();
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // Ottieni un pagamento specifico
        [HttpGet("{id}")]
        public ActionResult<Payment> GetPaymentById(int id)
        {
            try
            {
                var payment = _paymentService.GetPaymentById(id);
                if (payment == null)
                {
                    return NotFound(new { message = "Pagamento non trovato." });
                }

                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // Crea un nuovo pagamento
        [HttpPost]
        public IActionResult CreatePayment([FromBody] Payment payment)
        {
            if (payment == null)
            {
                return BadRequest(new { message = "Dati del pagamento non validi." });
            }

            try
            {
                _paymentService.CreatePayment(payment);
                return CreatedAtAction(nameof(GetPaymentById), new { id = payment.PaymentID }, payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // Aggiorna lo stato di un pagamento
        [HttpPatch("{id}/status")]
        public IActionResult UpdatePaymentStatus(int id, [FromBody] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return BadRequest(new { message = "Stato non valido." });
            }

            try
            {
                _paymentService.UpdatePaymentStatus(id, status);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // Elimina un pagamento (soft delete)
        [HttpDelete("{id}")]
        public IActionResult DeletePayment(int id)
        {
            try
            {
                _paymentService.DeletePayment(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
