// API/Middleware/ExceptionHandlingMiddleware.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Domain.Exceptions;

namespace API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next,
                                           ILogger<ExceptionHandlingMiddleware> logger,
                                           IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");

                int status = StatusCodes.Status500InternalServerError;
                string title = "An unexpected error occurred.";
                string typeUri = $"https://httpstatuses.com/{status}";
                string detail = ex.Message;

                //map ApiException (includes NotFoundException)
                if (ex is ApiException apiEx)
                {
                    status = apiEx.StatusCode;
                    title = apiEx.Message;
                    typeUri = $"https://httpstatuses.com/{status}";
                }
                else if (ex is FluentValidation.ValidationException validationEx)
                {
                    status = StatusCodes.Status400BadRequest;
                    title = "Validation failed";
                    detail = string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage));
                    typeUri = $"https://httpstatuses.com/{status}";
                }
                // add more custom exceptions as needed

                var problem = new ProblemDetails
                {
                    Status = status,
                    Title = title,
                    Detail = (_env.IsDevelopment() ? ex.ToString() : detail),
                    Instance = context.Request.Path,
                    Type = typeUri
                };

                // include traceId 
                problem.Extensions["traceId"] = context.TraceIdentifier;

                context.Response.Clear();
                context.Response.StatusCode = status;
                context.Response.ContentType = "application/problem+json";

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(problem, options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
