using System;
using System.Net;

namespace ISTS.Api.Model
{
    public class HttpResult
    {
        public HttpStatusCode StatusCode { get; protected set; }

        public string Message { get; protected set; }

        protected HttpResult(HttpStatusCode statusCode, string message = "")
        {
            StatusCode = statusCode;
            Message = message;
        }

        public static HttpResult Ok(string message = "")
        {
            return new HttpResult(HttpStatusCode.OK, message);
        }

        public static HttpResult OkNoContent(string message = "")
        {
            return new HttpResult(HttpStatusCode.NoContent, message);
        }

        public static HttpResult NotFound(string message = "")
        {
            return new HttpResult(HttpStatusCode.NotFound, message);
        }
    }
}