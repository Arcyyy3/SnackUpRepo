using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller

[Route("api/Reviews")]
public class ReviewController : ControllerBase
{
    private readonly ReviewService _reviewService;

    public ReviewController(ReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    // Recupera tutte le recensioni
    [HttpGet]
    public IActionResult GetAllReviews()
    {
        try
        {
            var reviews = _reviewService.GetAllReviews();
            return Ok(reviews);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching reviews: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // Recupera le recensioni di un prodotto specifico
    [HttpGet("product/{productId}")]
    public IActionResult GetReviewsByProductId(int productId)
    {
        try
        {
            var reviews = _reviewService.GetReviewsByProductId(productId);
            return Ok(reviews);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching reviews for product {productId}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // Aggiunge una nuova recensione
    [HttpPost]
    public IActionResult AddReview([FromBody] Review review)
    {
        try
        {
            _reviewService.AddReview(review);
            return CreatedAtAction(nameof(GetAllReviews), review);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding review: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // Modifica una recensione esistente
    [HttpPut("{id}")]
    public IActionResult UpdateReview(int id, [FromBody] Review review)
    {
        try
        {
            review.ReviewID = id;
            _reviewService.UpdateReview(review);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating review ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // Elimina una recensione per ID
    [HttpDelete("{reviewId}")]
    public IActionResult DeleteReview(int reviewId)
    {
        try
        {
            _reviewService.DeleteReview(reviewId);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting review ID {reviewId}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
