﻿
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MovieDB.Models;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using Serilog.Formatting.Compact;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MovieDB.Middleware
{
    /// <summary>
    /// Middle ware to divert logs containing PII(Personally Identifiable Information) into seperate lod folder
    /// </summary>
    public class AuditMiddleware
    {
        /// <summary>
        /// variable to store next delegete in pipeline
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// log builder based on serilog configs
        /// </summary>
        private readonly Logger _logger;

        /// <summary>
        /// Local Logger variable
        /// </summary>
        public static Logger Logger { get; set; }

        public AuditMiddleware(RequestDelegate requestDelegate, IOptions<UserSettings> config)
        {
            _logger = new Serilog.LoggerConfiguration()
                                 .MinimumLevel.Debug()
                                 .Enrich.FromLogContext()
                                 .WriteTo.File(new CompactJsonFormatter(), 
                                                config.Value.AuditLog.Path, 
                                                rollingInterval: (RollingInterval)Enum.Parse(typeof(RollingInterval), 
                                                config.Value.AuditLog.RollingInterval), 
                                                shared: config.Value.AuditLog.Shared,
                                                retainedFileCountLimit: config.Value.AuditLog.RetainedFileCountLimit)
                                 .CreateLogger();
            Logger = _logger;

            _next = requestDelegate;
        }

        /// <summary>
        /// Method to log the request and response and forward the context to next delegete in pipeline
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var transId = Guid.NewGuid().ToString();

            if (_logger != null)
            {
                using (var requestStream = new MemoryStream()) //building pipeline, using => dispose after use
                {
                    await context.Request.Body.CopyToAsync(requestStream);

                    requestStream.Seek(0, SeekOrigin.Begin);

                    var requestBodyAsText = await new StreamReader(requestStream).ReadToEndAsync();

                    _logger.Information(
                        string.Format("Request: Transaction ID-{0}, Method-{1}, Path-{2}, QueryString-{3}, Payload-{4}",
                        transId, context.Request.Method, context.Request.Path,
                        context.Request.QueryString, JsonConvert.SerializeObject(requestBodyAsText)));

                    //reset the request stream position
                    requestStream.Seek(0, SeekOrigin.Begin);
                    context.Request.Body = requestStream;
                    context.Request.Headers["tranid"] = transId;

                    //Copy a pointer to the original response body stream
                    var originalBodyStream = context.Response.Body;
                    using (var responseStream = new MemoryStream())
                    {
                        try
                        {
                            context.Response.Body = responseStream;

                            await _next(context).ConfigureAwait(continueOnCapturedContext: false);
                            responseStream.Seek(0, SeekOrigin.Begin);

                            var responseBodyasText = await new StreamReader(responseStream).ReadToEndAsync();
                            _logger.Information(
                                string.Format("Response:TransactionID-{0}, Method-{1}, Path-{2}, QueryString-{3}, Payload-{4}",
                                    transId, context.Request.Method, context.Request.Path, context.Request.QueryString, responseBodyasText));
                            responseStream.Seek(0, SeekOrigin.Begin);
                            await responseStream.CopyToAsync(originalBodyStream);
                        }
                        catch
                        {
                            context.Response.Body = originalBodyStream;
                            throw;
                        }
                    }
                }
            }
            else
            {
                context.Request.Headers["tranid"] = transId;
                await _next(context).ConfigureAwait(continueOnCapturedContext: false);
            }

        }

    }
}
