using LiquidLabsAssessment.Infrastructure.Exceptions;
using System.Net;
using System.Text.Json;

namespace LiquidLabsAssessment.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json";

                var response = context.Response;

                response.StatusCode = ex switch
                {
                    NotFoundException => (int)HttpStatusCode.NotFound,

                    BadHttpRequestException => (int)HttpStatusCode.BadRequest,

                    _ => (int)HttpStatusCode.InternalServerError
                };

                var result = JsonSerializer.Serialize(new
                {
                    Error = ex.Message,
                    StatusCode = response.StatusCode
                });

                await response.WriteAsync(result);
            }
        }
    }
}
