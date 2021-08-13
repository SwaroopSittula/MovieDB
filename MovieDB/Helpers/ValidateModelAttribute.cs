
using Microsoft.AspNetCore.Mvc.Filters;
using MovieDB.Models;
using Newtonsoft.Json;
using System;



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
                        ErrorMessage = "Bad Request!"
                        //ErrorMessage = string.Join(", ", context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)).ToString()
                    };
                //context.Result = new BadRequestObjectResult(errorResponse)

                Exception exception = new Exception(JsonConvert.SerializeObject(errorResponse));
                throw exception;
            } 
        }
    }
}
