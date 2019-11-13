namespace Dappery.Api.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Core.Exceptions;
    using Domain.Dtos;
    using Domain.ViewModels;
    using FluentValidation;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _pipeline;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate pipeline, ILogger<ErrorHandlingMiddleware> logger)
        {
            _pipeline = pipeline;
            _logger = logger;
        }
        
        /// <summary>
        /// Kicks off he request pipeline while catching any exceptions thrown in the application layer.
        /// </summary>
        /// <param name="context">HTTP context from the request pipeline</param>
        /// <returns>Hand off to next request delegate in the pipeline</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _pipeline(context);
            }
            catch (Exception e)
            {
                _logger.LogError($"An exception has occurred processing the request: {e.Message}");
                await HandleExceptionAsync(context, e);
            }
        }
        
        
        /// <summary>
        /// Handles any exception thrown during the pipeline process and in the application layer. Note that model state
        /// validation failures made in the web layer are handled by the ASP.NET Core model state validation failure filter.
        /// </summary>
        /// <param name="context">HTTP context from the request pipeline</param>
        /// <param name="exception">Exceptions thrown during pipeline processing</param>
        /// <returns>Writes the API response to the context to be returned in the web layer</returns>
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ErrorDto errors;
            ICollection<object> errorList = new List<object>();

            /*
             * Handle exceptions based on type, while defaulting to generic internal server error for unexpected exceptions.
             * Each case handles binding the API response message, API response status code, the HTTP response status code,
             * and any errors incurred in the application layer. Validation failures returned from Fluent Validation will
             * be added to the API response if there are any instances.
             */
            switch (exception)
            {
                case DapperyApiException conduitApiException:
                    errors = new ErrorDto(conduitApiException.Message);
                    context.Response.StatusCode = (int)conduitApiException.StatusCode;
                    if (conduitApiException.ApiErrors.Any())
                    {
                        errors.Details = conduitApiException.ApiErrors;
                    }

                    break;

                case ValidationException validationException:
                    context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    foreach (var validationFailure in validationException.Errors)
                    {
                        var conduitValidationError = new DapperyApiError(validationFailure.ErrorMessage, validationFailure.PropertyName);
                        errorList.Add(conduitValidationError);
                    }

                    // Instantiate an error object to add to the response
                    errors = new ErrorDto("A validation error has occured while processing the request.", errorList);
                    break;

                default:
                    errors = new ErrorDto("An error occured while processing the request.");
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            // Instantiate the response
            context.Response.ContentType = "application/json";
            var errorResponse = new ErrorViewModel(errors);

            // Serialize the response and write out to the context buffer to return
            var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true,
            });
            
            await context.Response.WriteAsync(result);
        }
    }
}