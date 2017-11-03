using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using ISTS.Application.Common;

namespace ISTS.Api.Filters
{
    public class HandleApiExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var isUnauthorizedAccess = context.Exception is UnauthorizedAccessException;
            var isDataValidationException = context.Exception is DataValidationException;

            if (!isUnauthorizedAccess && !isDataValidationException)
            {
                var exceptionMessage = context.Exception.Message;
                context.HttpContext.Response.StatusCode = 500;
                context.Result = new JsonResult(new
                {
                    ErrorMessage = exceptionMessage,
                    StackTrace = context.Exception.StackTrace
                });
            }

            if (isDataValidationException)
            {
                var exceptionMessage = context.Exception.InnerException.Message;
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(new
                {
                    ValidationErrorMessage = exceptionMessage
                });
            }
        }
    }
}