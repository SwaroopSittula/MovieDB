using Microsoft.AspNetCore.Http;
using MovieDB.Helpers;
using MovieDB.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MovieDB.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<ErrorResponse>(exception.Message);
                context.Response.ContentType = Constants.Json;
                context.Response.StatusCode = result.StatusCode;
                await context.Response.WriteAsync(exception.Message);

            }
            catch(Exception e)
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

}
