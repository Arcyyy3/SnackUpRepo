using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller

[Route("api/Schools")]
public class SchoolController : ControllerBase
{
    private readonly SchoolService _schoolService;

    public SchoolController(SchoolService schoolService)
    {
        _schoolService = schoolService;
    }

    [HttpGet]
    public IActionResult GetAllSchools()
    {
        try
        {
            var schools = _schoolService.GetAllSchools();
            return Ok(schools);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching schools: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetSchoolById(int id)
    {
        try
        {
            var school = _schoolService.GetSchoolById(id);
            if (school == null)
                return NotFound($"School with ID {id} not found.");
            return Ok(school);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching school ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("address/{address}/name/{name}")]
    public IActionResult GetSchoolIDByAddressAndName(string address, string name)
    {
        try
        {
            var schoolId = _schoolService.GetSchoolIDByAddressAndName(address, name);
            if (schoolId == 0)
                return NotFound("No school found with the provided address and name.");
            return Ok(schoolId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching school by address and name: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("CityList")]
    public IActionResult GetCityList()
    {
        try
        {
            var cities = _schoolService.GetCityList();
            return Ok(cities);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching cities: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("schools-in-city/{city}")]
    public IActionResult GetSchoolsInCity(string city)
    {
        try
        {
            var schools = _schoolService.GetSchoolsInCity(city);
            if (!schools.Any())
                return NotFound($"No schools found in city {city}.");
            return Ok(schools);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching schools in city {city}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("producer-id/{schoolId}")]
    public IActionResult GetProducerIDBySchoolID(int schoolId)
    {
        try
        {
            var producerId = _schoolService.GetProducerIDBySchoolID(schoolId);
            if (!producerId.HasValue)
                return NotFound($"No producer found for school ID {schoolId}.");
            return Ok(producerId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching producer ID for school ID {schoolId}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    public IActionResult AddSchool([FromBody] School school)
    {
        try
        {
            _schoolService.AddSchool(school);
            return CreatedAtAction(nameof(GetSchoolById), new { id = school.SchoolID }, school);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding school: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateSchool(int id, [FromBody] School school)
    {
        try
        {
            school.SchoolID = id;
            _schoolService.UpdateSchool(school);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating school ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteSchool(int id)
    {
        try
        {
            _schoolService.DeleteSchool(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting school ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
