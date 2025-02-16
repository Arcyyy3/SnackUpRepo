using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller

[Route("api/SchoolClasses")]
public class SchoolClassController : ControllerBase
{
    private readonly SchoolClassService _schoolClassService;

    public SchoolClassController(SchoolClassService schoolClassService)
    {
        _schoolClassService = schoolClassService;
    }

    [HttpGet("ByUser/{userID}")]
    public IActionResult GetClassYearAndSectionByUserID(int userID)
    {
        try
        {
            var classInfo = _schoolClassService.GetClassYearAndSectionByUserID(userID);

            if (classInfo == null || classInfo.Count == 0)
            {
                return NotFound($"Nessuna informazione trovata per l'utente con ID {userID}.");
            }

            return Ok(classInfo); // HTTP 200 con i dati trovati
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il recupero delle informazioni sulla classe per l'utente {userID}: {ex.Message}");
            return StatusCode(500, "Errore interno del server");
        }
    }


    [HttpGet("{id}")]
    public IActionResult GetSchoolClassById(int id)
    {
        try
        {
            var schoolClass = _schoolClassService.GetSchoolClassById(id);
            if (schoolClass == null)
                return NotFound($"School class with ID {id} not found."); // HTTP 404
            return Ok(schoolClass); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching school class ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("register/{schoolId}/{classYear}/{classSection}")]
    public IActionResult GetSchoolClassIDByParameters(int schoolId, int classYear, string classSection)
    {
        try
        {
            var schoolClassId = _schoolClassService.GetSchoolClassesIDByParameters(schoolId, classYear, classSection);
            return Ok(schoolClassId); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching school class ID by parameters: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{schoolId},{classYear}")]
    public IActionResult GetClassSectionsByParameters(int schoolId, int classYear)
    {
        try
        {
            var sections = _schoolClassService.GetClassSectionByParameters(schoolId, classYear);
            if (sections == null || !sections.Any())
                return NotFound("No class sections found."); // HTTP 404
            return Ok(sections); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching class sections: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    public IActionResult AddSchoolClass([FromBody] SchoolClass schoolClass)
    {
        try
        {
            _schoolClassService.AddSchoolClass(schoolClass);
            return CreatedAtAction(nameof(GetSchoolClassById), new { id = schoolClass.SchoolClassID }, schoolClass); // HTTP 201
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding school class: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateSchoolClass(int id, [FromBody] SchoolClass schoolClass)
    {
        try
        {
            schoolClass.SchoolClassID = id;
            _schoolClassService.UpdateSchoolClass(schoolClass);
            return NoContent(); // HTTP 204
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating school class ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteSchoolClass(int id)
    {
        try
        {
            _schoolClassService.DeleteSchoolClass(id);
            return NoContent(); // HTTP 204
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting school class ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
