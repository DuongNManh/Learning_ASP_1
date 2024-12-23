﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Learning_Web.API.Models;
using Learning_Web.API.Models.Response;
using Learning_Web.API.Models.Domain;
using Microsoft.AspNetCore.Http;

namespace Learning_Web.API.CustomActionFilters
{
    public class ValidateModelAttributes : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = new Dictionary<string, string[]>();

                foreach (var modelStateEntry in context.ModelState)
                {
                    var key = modelStateEntry.Key;
                    var errorMessages = modelStateEntry.Value.Errors
                        .Select(error => string.IsNullOrEmpty(error.ErrorMessage)
                            ? error.Exception?.Message
                            : error.ErrorMessage)
                        .Where(errorMessage => !string.IsNullOrEmpty(errorMessage))
                        .ToArray();

                    if (errorMessages.Any())
                    {
                        errors[key] = errorMessages;
                    }
                }

                var response = ApiResponseBuilder.BuildErrorResponse(
                    data: errors,
                    message: "Validation failed",
                    statusCode: StatusCodes.Status400BadRequest,
                    reason: "One or more validation errors occurred"
                    );

                context.Result = new BadRequestObjectResult(response);
            }
        }
    }
}
