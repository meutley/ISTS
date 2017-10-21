using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ISTS.Api.Filters
{
    public class HandleGenericExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (!(context.Exception is UnauthorizedAccessException))
            {
                var exceptionMessage = context.Exception.Message;
                context.HttpContext.Response.StatusCode = 500;
                context.Result = new JsonResult(new {
                    ErrorMessage = exceptionMessage
                });
            }
        }
    }
}