using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Threading.Tasks;
namespace DotNetAdvance.Logging
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            Log.Information("Request {Method} {Url}  received", context.Request.Method, context.Request.Path);
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error($"{ex}");
                throw; 
            }
        }
    }

}
