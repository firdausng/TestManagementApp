using AppCore.Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IHostEnvironment host;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, IHostEnvironment host)
        {
            this.next = next;
            this.host = host;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var body = context.Response.StatusCode;
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var problemDetail = new ProblemDetails()
            {
                Title = exception.Message,
                Detail = exception.StackTrace,
                Instance = context.Request.Path,
                Status = StatusCodes.Status500InternalServerError,

            };

            // for security reason, not leaking any implementation/error detail in production
            if (host.IsProduction())
            {
                problemDetail.Detail = "Unexpected error occured";
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "applications/problem+json";

            switch (exception)
            {
                case EntityNotFoundException _:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    problemDetail.Status = StatusCodes.Status404NotFound;
                    break;
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetail, options));
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
