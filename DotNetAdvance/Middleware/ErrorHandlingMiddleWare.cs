using Serilog;

namespace DotNetAdvance.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
          
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error($"{ex}");

                // Return a generic error response
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
                
            }
        }
    }

}
