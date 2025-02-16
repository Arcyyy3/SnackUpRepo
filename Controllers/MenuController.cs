using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller
[Route("api/Menu")]
public class MenuController : ControllerBase
{
    [Authorize]
    [HttpGet("BaseSections")]
    public IActionResult GetBaseSections()
    {
        return Ok(new { Sections = new[] { "Home", "BaseSection1", "BaseSection2" } });
    }

    [Authorize(Roles = "Owner")]
    [HttpGet("OwnerSection")]
    public IActionResult GetOwnerSection()
    {
        return Ok("Accesso consentito solo agli Owner.");
    }

    [Authorize(Roles = "SchoolAdmin")]
    [HttpGet("SchoolAdminSection")]
    public IActionResult GetSchoolAdminSection()
    {
        return Ok("Accesso consentito solo agli School Admin.");
    }

    [Authorize(Roles = "Producer")]
    [HttpGet("ProducerSection")]
    public IActionResult GetProducerSection()
    {
        return Ok("Accesso consentito solo ai Producers.");
    }

    [Authorize]
    [HttpGet("DynamicMenu")]
    public IActionResult GetDynamicMenu()
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        var menu = new List<string> { "Home", "BaseSection1", "BaseSection2" };

        if (role == "Owner")
        {
            menu.Add("OwnerSection");
            menu.Add("SchoolAdminSection");
            menu.Add("ProducerSection");
        }
        else if (role == "SchoolAdmin")
        {
            menu.Add("SchoolAdminSection");
        }
        else if (role == "Producer")
        {
            menu.Add("ProducerSection");
        }

        return Ok(menu);
    }
}
