using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MovieDB.Models;
using System.Linq;


namespace MovieDB.Helpers
{
//ActionFilter = ValidateModel
public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext actionContext)
    {
        if (!actionContext.ModelState.IsValid)
        {
            var errorResponse = new ErrorResponse
            {
                StatusCode = 400,
                ErrorMessage = string.Join(", ", actionContext.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)).ToString()
            };
            actionContext.Result = new BadRequestObjectResult(errorResponse);
        }; 
    }
}
}
