using Healthcare.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Healthcare.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{

    [NonAction]
    public IActionResult CreateActionResult<T>(CustomResponseDto<T> response)
    {
        if (response.StatusCode == 204)
        {
            return new ObjectResult(null)
            {
                StatusCode = response.StatusCode
            };
        }
        return new ObjectResult(response)
        {
            StatusCode = response.StatusCode
        };
    }
}