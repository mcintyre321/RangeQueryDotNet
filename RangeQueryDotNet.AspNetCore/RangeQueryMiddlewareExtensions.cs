using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;

namespace RangeQueryDotNet.AspNetCore
{ 
    public static class RangeQueryMiddlewareExtensions  
    {  
        public static IApplicationBuilder UseRangeQueryMiddleware(this IApplicationBuilder builder, string rangeKey, Func<(string query, string responseContent), string> transform)  
        {  
            return builder.UseMiddleware<RangeQueryMiddleware>(new RangeQueryOptions()
            {
                RangeKey = rangeKey,
                Transform = transform
            });  
        }  
    }
}