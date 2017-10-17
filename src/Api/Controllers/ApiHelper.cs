using System;
using System.Text;

namespace ISTS.Api.Controllers
{
    public static class ApiHelper
    {
        public static string GetResourceUri(params object[] args)
        {
            if (args == null || args.Length == 0)
            {
                return string.Empty;
            }
            
            var builder = new StringBuilder();
            builder.Append("api/");
            builder.Append(string.Join("/", args));
            
            return builder.ToString();
        }
    }
}