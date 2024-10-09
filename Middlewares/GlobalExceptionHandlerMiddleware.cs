using System.Net;

namespace BookProject.Middlewares
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "error during executing {Context}", context.Request.Path.Value);
                var response = context.Response;
                response.ContentType = "application/json";

                var (status, message) = GetResponse(exception);
                response.StatusCode = (int)status;
                await response.WriteAsync(message);
            }
        }

        public (HttpStatusCode code, string message) GetResponse(Exception exception)
        {
            HttpStatusCode code;
            switch (exception)
            {
                case KeyNotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
                case UnauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    break;
                case InvalidOperationException:
                    code = HttpStatusCode.BadRequest;
                    break;
                case ArgumentException:
                    code = HttpStatusCode.BadRequest;
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    break;
            }

            return (code, exception.Message);
        }

    }
}
