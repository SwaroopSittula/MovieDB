using Microsoft.AspNetCore.Http;
using MovieDB.Helpers;
using MovieDB.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MovieDB.Middleware
{
    /// <summary>
    /// The Purpose of this MiddleWare is to handle Internal Server Errors only
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        /// <summary>
        /// Http Request Delegete, stores next in pipeline and passes context to next Middleware in Pipeline
        /// </summary>
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Method that catches the Internal Server Error, if not sends HttpContext to next Delegete in Pipeline 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception)
            {
                await HandleExceptionAsync(context);
            }
        }

        /// <summary>
        /// Async method to handle the Internal Server error and produce a custom error response
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static async Task HandleExceptionAsync(HttpContext context)
        {
            var result = JsonConvert.SerializeObject(new ErrorResponse()
            {
                ErrorMessage = "Internal Server Error!",
                StatusCode = 500
            });
            context.Response.ContentType = Constants.Json;
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(result);
        }
    }

}
