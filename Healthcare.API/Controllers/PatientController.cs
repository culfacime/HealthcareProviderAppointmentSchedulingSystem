using Microsoft.AspNetCore.Mvc;

namespace Healthcare.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientController : ControllerBase
{

    private readonly ILogger<PatientController> _logger;

    public PatientController(ILogger<PatientController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetPatients")]
    public IActionResult Get()
    {
        return Ok("Test");
    }
}