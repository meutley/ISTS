using System;

namespace ISTS.Api.Model
{
    public class ApiModelResult<T>
    {
        public T Value { get; }

        public HttpResult HttpResult { get; }
        
        protected ApiModelResult(HttpResult httpResult, T value)
        {
            HttpResult = httpResult;
            Value = value;
        }

        public static ApiModelResult<T> Create(HttpResult httpResult, T value)
        {
            return new ApiModelResult<T>(httpResult, value);
        }

        public static ApiModelResult<T> Ok(T value, string message = "")
        {
            return new ApiModelResult<T>(HttpResult.Ok(message), value);
        }

        public static ApiModelResult<T> OkNoContent(T value, string message = "")
        {
            return new ApiModelResult<T>(HttpResult.OkNoContent(message), value);
        }

        public static ApiModelResult<T> NotFound(T value, string message = "")
        {
            return new ApiModelResult<T>(HttpResult.NotFound(message), value);
        }
    }
}