using System;
using System.Collections.Generic;
using System.Net;
using Api.Errors;
using Application.Exceptions;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api.Filter
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
        public ApiExceptionFilter()
        {
            // Register known exception types and handlers.
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(UnauthorizedUserException), HandleUnauthorizedException },
                { typeof(BadRequestException), HandleBadRequestException },
                { typeof(EntityException), HandleBadRequestException },
                { typeof(DeleteFailureException), HandleBadRequestException }
            };

        }
        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            Type type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            HandleUnknownException(context);
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            var ex = context.Exception;
            var env = context.HttpContext.RequestServices.GetRequiredService<IHostEnvironment>();

            int statusCode = (int)HttpStatusCode.InternalServerError;
            var response = env.IsDevelopment()
                ? new ApiException(statusCode, ex.Message, ex.StackTrace.ToString())
                : new ApiException(statusCode);

            context.Result = new ObjectResult(response)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            context.ExceptionHandled = true;
        }

        private void HandleValidationException(ExceptionContext context)
        {
            var exception = context.Exception as ValidationException;

            var response = new ApiValidationErrorResponse()
            {
                Errors = exception.Errors
            };

            context.Result = new BadRequestObjectResult(response);

            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;

            var response = new ApiResponse((int)HttpStatusCode.NotFound, exception.Message);

            context.Result = new NotFoundObjectResult(response);

            context.ExceptionHandled = true;
        }

        private void HandleUnauthorizedException(ExceptionContext context)
        {
            var exception = context.Exception as UnauthorizedUserException;

            var response = new ApiResponse((int)HttpStatusCode.Unauthorized, exception.Message);

            context.Result = new UnauthorizedObjectResult(response);

            context.ExceptionHandled = true;
        }

        private void HandleBadRequestException(ExceptionContext context)
        {
            var exception = context.Exception as Exception;

            var response = new ApiResponse((int)HttpStatusCode.BadRequest, exception.Message);

            context.Result = new BadRequestObjectResult(response);

            context.ExceptionHandled = true;
        }
    }

}