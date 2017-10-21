using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ISTS.Api.Filters
{
    public class HandleUnauthorizedAccessExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is UnauthorizedAccessException)
            {
                context.Result = new UnauthorizedResult();
                context.ExceptionHandled = true;
            }
        }
    }
}