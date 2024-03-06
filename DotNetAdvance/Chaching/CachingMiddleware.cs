using Microsoft.Extensions.Caching.Memory;

namespace DotNetAdvance.Chaching
{
    public class CachingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;

        public CachingMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task Invoke(HttpContext context)
        {
            // Intercept only GET requests
            if (context.Request.Method == HttpMethods.Get)
            {
                var cacheKey = context.Request.Path;
                if (_cache.TryGetValue(cacheKey, out string cachedResponse))
                {
                    // If the response is found in cache, return it directly
                    context.Response.Headers.Add("X-Data-Source", "Cache");
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(cachedResponse);
                    return;
                }
                else
                {
                    using (var responseBody = new MemoryStream())
                    {
                        var originalBodyStream = context.Response.Body;
                        context.Response.Body = responseBody;

                        try
                        {
                            // Call the next middleware in the pipeline
                            await _next(context);

                            // Check if the response is successful (status code in the 200 range)
                            if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
                            {
                                // Reset stream position and read the response content
                                responseBody.Seek(0, SeekOrigin.Begin);
                                cachedResponse = await new StreamReader(responseBody).ReadToEndAsync();

                                // Cache the response for future requests
                                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
                                _cache.Set(cacheKey, cachedResponse, cacheEntryOptions);

                                // Reset stream position again and copy cached response to the original response body stream
                                responseBody.Seek(0, SeekOrigin.Begin);
                                await responseBody.CopyToAsync(originalBodyStream);
                            }
                            else
                            {
                                // If the response is not successful, do not cache it
                                await responseBody.CopyToAsync(originalBodyStream);
                            }
                        }
                        finally
                        {
                            context.Response.Body = originalBodyStream; // Restore the original response body stream
                        }
                    }
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
