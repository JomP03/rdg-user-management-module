using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Services.API.Extensions
{
    /// <summary>
    /// Middleware for CORS
    /// </summary>
    public class CorsMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Options for CORS
        /// </summary>
        /// <param name="next"></param>
        public CorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invoke method
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == "OPTIONS")
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
                context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");

                context.Response.StatusCode = 200;
                await context.Response.WriteAsync("OK");
            }
            else
            {
                await _next(context);
            }
        }
    }

}