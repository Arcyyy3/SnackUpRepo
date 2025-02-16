using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller

[Route("api/Subscriptions")]
public class SubscriptionController : ControllerBase
{
    private readonly SubscriptionService _subscriptionService;

    public SubscriptionController(SubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    // Recupera tutti gli abbonamenti
    [HttpGet]
    public IActionResult GetAllSubscriptions()
    {
        var subscriptions = _subscriptionService.GetAllSubscriptions();
        return Ok(subscriptions); // Restituisce HTTP 200 con i dati
    }

    // Recupera gli abbonamenti di un utente specifico
    [HttpGet("user/{userId}")]
    public IActionResult GetSubscriptionsByUserId(int userId)
    {
        var subscriptions = _subscriptionService.GetSubscriptionsByUserId(userId);
        return Ok(subscriptions); // Restituisce HTTP 200 con i dati
    }

    // Aggiunge un nuovo abbonamento
    [HttpPost]
    public IActionResult AddSubscription([FromBody] Subscription subscription)
    {
        _subscriptionService.AddSubscription(subscription);
        return CreatedAtAction(nameof(GetAllSubscriptions), subscription); // Restituisce HTTP 201 Created
    }

    // Modifica un abbonamento esistente
    [HttpPut("{id}")]
    public IActionResult UpdateSubscription(int id, [FromBody] Subscription subscription)
    {
        _subscriptionService.UpdateSubscription(subscription);
        return NoContent(); // Restituisce HTTP 204 No Content
    }
    
    // Elimina un abbonamento per ID
    [HttpDelete("{subscriptionId}")]
    public IActionResult DeleteSubscription(int subscriptionId)
    {
        _subscriptionService.DeleteSubscription(subscriptionId);
        return NoContent(); // Restituisce HTTP 204 No Content
    }
}
