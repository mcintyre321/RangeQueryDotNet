using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RangeQueryDotNet.AspNetCore
{
    public class RangeQueryMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RangeQueryOptions _options;

        public RangeQueryMiddleware(RequestDelegate next, RangeQueryOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var hasRangeHeader = context.Request.Headers.TryGetValue("range", out var values) &&
                                     values.FirstOrDefault().StartsWith(_options.RangeKey + "=");

            Stream originBody = null;
            if (hasRangeHeader)
            {
                var originBody1 = context.Response.Body;
                context.Response.Body = new MemoryStream();
                originBody = originBody1;
            }
            
            await _next(context);

            if (hasRangeHeader)
            {
                var rangeQuery = context.Request.Headers["range"].FirstOrDefault().Substring(_options.RangeKey.Length + 1);

                if (!string.IsNullOrWhiteSpace(rangeQuery))
                {
                    context.Response.StatusCode = 206;
                    context.Response.Headers["Content-Range"] = rangeQuery;
                    
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
                    var transformedText = _options.Transform((rangeQuery, text));
                    var requestContent = new StringContent(transformedText, Encoding.UTF8, context.Response.ContentType.Split(";", 2)[0]);
                    context.Response.Body = await requestContent.ReadAsStreamAsync();//modified stream
                    context.Response.ContentLength = context.Response.Body.Length;
                    
                    await context.Response.Body.CopyToAsync(originBody);//
                }
            }
        }

    }
}