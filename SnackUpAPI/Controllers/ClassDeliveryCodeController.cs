using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System;

[ApiController]
[Route("api/ClassDeliveryCodes")]
public class ClassDeliveryCodeController : ControllerBase
{
    private readonly ClassDeliveryCodeService _classDeliveryCodeService;

    public ClassDeliveryCodeController(ClassDeliveryCodeService classDeliveryCodeService)
    {
        _classDeliveryCodeService = classDeliveryCodeService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            var codes = _classDeliveryCodeService.GetAll();
            return Ok(codes);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving delivery codes: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        try
        {
            var code = _classDeliveryCodeService.GetById(id);
            if (code == null)
                return NotFound($"Delivery code with ID {id} not found.");

            return Ok(code);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving delivery code by ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
  
    [HttpGet("WithOrders")]
    public IActionResult GetClassDeliveryCodesWithOrders()
    {
        try
        {
            var deliveryCodes = _classDeliveryCodeService.GetAll();

            var result = deliveryCodes.Select(dc => new
            {
                dc.Code1,
                dc.Code2,
                dc.SchoolClassID,
                Orders = _classDeliveryCodeService.GetOrdersByClassId(dc.SchoolClassID).Select(order => new
                {
                    order.OrderID,
                    UserName = _classDeliveryCodeService.GetUserById(order.UserID).Name,
                    order.TotalPrice,
                    order.Status
                }).ToList()
            }).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving delivery codes with orders: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpGet("GetCodeStatus/{userID}")]
    public IActionResult GetCodeStatus(int userID)
    {
        string currentMoment = Utility.GetCurrentMoment();
        var result = _classDeliveryCodeService.GetCodeStatus(userID, currentMoment);
        return Ok(result);
    }

    [HttpGet("IsCodeRetrieved/{userID}")]
    public IActionResult IsCodeAlreadyRetrieved(int userID)
    {
        string currentMoment = Utility.GetCurrentMoment();
        bool isRetrieved = _classDeliveryCodeService.IsCodeAlreadyRetrieved(userID, currentMoment);
        return Ok(new { IsRetrieved = isRetrieved });
    }
    [HttpGet("IsCodeAlreadyRetrievedWithName/{userID}")]
    public async Task<IActionResult> IsCodeAlreadyRetrievedWithName(int userID)
    {
        string currentMoment = Utility.GetCurrentMoment();
        string ritirato = await _classDeliveryCodeService.IsCodeAlreadyRetrievedWithName(userID, currentMoment);
        return Ok(new { Ritirato = ritirato });
    }

    [HttpGet("GetCode/{schoolClassID}")]
    public IActionResult GetDeliveryCode(int schoolClassID)
    {
        try
        {
            string currentMoment = Utility.GetCurrentMoment();
            if (currentMoment == "OutsideHours")
                return BadRequest("No codes are available outside break times.");

            var code = _classDeliveryCodeService.GetDeliveryCode(schoolClassID, currentMoment);
            return Ok(new { Code = code });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpPost("ConfirmRetrieval/{userID}")]
    public IActionResult ConfirmCodeRetrieval(int userID)
    {
        try
        {
            string currentMoment = Utility.GetCurrentMoment();
            if (string.IsNullOrEmpty(currentMoment))
                return BadRequest("Invalid moment of the day.");

            bool isRetrieved = _classDeliveryCodeService.IsCodeAlreadyRetrieved(userID, currentMoment);
            if (isRetrieved)
            {
                return BadRequest("Code already retrieved.");
            }

            _classDeliveryCodeService.ConfirmCodeRetrieval(userID, currentMoment);
            string userName = _classDeliveryCodeService.GetUserName(userID); // Metodo opzionale per ottenere il nome utente

            return Ok(new { Message = "Code confirmed.", RetrievedBy = userName });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
    [HttpGet("GetCodeInfo/{userID}")]
    public IActionResult GetCodeInformation(int userID)
    {
        string currentMoment = Utility.GetCurrentMoment();

        try
        {

            var codeInfo = _classDeliveryCodeService.GetCodeInformation(userID, currentMoment);
            return Ok(codeInfo);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching code information.", Details = ex.Message });
        }
    }

    [HttpGet("CheckCodeStatus/{userID}")]
    public IActionResult CheckCodeStatus(int userID)
    {
        var currentMoment = Utility.GetCurrentMoment();
        var status = _classDeliveryCodeService.GetCodeStatus(userID, currentMoment);
        if (status.IsRetrieved)
        {
            return Ok(new { IsRetrieved = true, RetrievedBy = status.RetrievedBy });
        }

        return Ok(new { IsRetrieved = false });
    }


    [HttpPost]
    public IActionResult Add([FromBody] ClassDeliveryCode code)
    {
        try
        {
            _classDeliveryCodeService.Add(code);
            return CreatedAtAction(nameof(GetById), new { id = code.ClassDeliveryCodeID }, code);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding delivery code: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ClassDeliveryCode code)
    {
        try
        {
            code.ClassDeliveryCodeID = id;
            _classDeliveryCodeService.Update(code);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating delivery code ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _classDeliveryCodeService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting delivery code ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
