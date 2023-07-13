using Hangfire.Logging;
using Healthcare.Core.DTOs;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Healthcare.API.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.Request.Method == "POST" && !context.ModelState.IsValid)
        {
            var hataList = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Fail(400, hataList));

            return;
        }
        await next();
    }
}