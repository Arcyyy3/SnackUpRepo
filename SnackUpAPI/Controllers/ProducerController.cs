using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;

[ApiController]
[Route("api/Producers")]
public class ProducerController : ControllerBase
{
    private readonly ProducerService _producerService;

    public ProducerController(ProducerService producerService)
    {
        _producerService = producerService;
    }

    [HttpGet]
    public IActionResult GetAllProducers()
    {
        try
        {
            var producers = _producerService.GetAllProducers();
            return Ok(producers); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving producers: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetProducerById(int id)
    {
        try
        {
            var producer = _producerService.GetProducerById(id);
            if (producer == null)
                return NotFound($"Producer with ID {id} not found."); // HTTP 404
            return Ok(producer); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving producer ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    public IActionResult AddProducer([FromBody] Producer producer)
    {
        try
        {
            _producerService.AddProducer(producer);
            return CreatedAtAction(nameof(GetProducerById), new { id = producer.ProducerID }, producer);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding producer: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProducer(int id, [FromBody] Producer producer)
    {
        try
        {
            producer.ProducerID = id;
            _producerService.UpdateProducer(producer);
            return NoContent(); // HTTP 204
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating producer ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProducer(int id)
    {
        try
        {
            _producerService.DeleteProducer(id);
            return NoContent(); // HTTP 204
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting producer ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
