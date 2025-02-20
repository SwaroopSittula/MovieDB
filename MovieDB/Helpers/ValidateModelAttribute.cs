﻿
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
    /// 
    /// adding this following code in startup.cs we can avoid badrequestobject return to be of pascal case instead of camelcase (gives single point control)
    ///            services.AddControllers().AddJsonOptions(options => 
    ///            {
    ///                 options.JsonSerializerOptions.PropertyNamingPolicy = null;
    ///            });
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
                //following code => inconsistency in error response (giving camel case as response)
                //context.Result = new BadRequestObjectResult(errorResponse)


                //following results in pascal case output response
                context.Result = new ContentResult
                { 
                    Content = JsonConvert.SerializeObject(errorResponse),
                    ContentType = Constants.Json,
                    StatusCode = 400
                };
            } 
        }
        
    }
}
