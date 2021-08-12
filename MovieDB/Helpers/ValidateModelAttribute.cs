using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MovieDB.Models;
using System.Linq;


namespace MovieDB.Helpers
{
//ActionFilter = ValidateModel
public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errorResponse = new ErrorResponse
            {
                StatusCode = 400,
                ErrorMessage = string.Join(", ", context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)).ToString()
            };
            context.Result = new BadRequestObjectResult(errorResponse);
        } 
    }
}
}
