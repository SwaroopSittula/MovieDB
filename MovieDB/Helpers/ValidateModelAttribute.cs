
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MovieDB.Models;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace MovieDB.Helpers
{
    /// <summary>
    /// ActionFilter = ValidateModel
    /// Used to validate the request payload Models in controllers generally
    /// </summary>
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
