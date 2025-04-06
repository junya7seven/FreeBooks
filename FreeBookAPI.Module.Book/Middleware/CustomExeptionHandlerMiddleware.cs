using FreeBookAPI.Infrastructure.StorageAPI;
using System.Net;
using System.Text.Json;

namespace FreeBookAPI.Module.Book.Middleware
{
    public class CustomExeptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExeptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;
            switch (exception)
            {
                case ArgumentNullException ex:
                    code = HttpStatusCode.BadRequest;
                    result = ex.Message;
                    break;
                case ArgumentException ex:
                    code = HttpStatusCode.BadRequest;
                    result = ex.Message;
                    break;
                case UnauthorizedAccessException ex:
                    code = HttpStatusCode.BadRequest;
                    result = ex.Message;
                    break;
                case InsufficientStorageYandexException ex:
                    code = HttpStatusCode.InsufficientStorage;
                    result = ex.Message;
                    break;
                case ConflictException ex:
                    code = HttpStatusCode.BadRequest;
                    result = ex.Message;
                    break;
                case TooLargeFileYandexException ex:
                    code = HttpStatusCode.BadRequest;
                    result = ex.Message;
                    break;
                case TechnicalWorkYandexException ex:
                    code = HttpStatusCode.ServiceUnavailable;
                    result = ex.Message;
                    break;
                case UnavailableService ex:
                    code = HttpStatusCode.ServiceUnavailable;
                    result = ex.Message;
                    break;
                case HttpRequestException ex:
                    code = HttpStatusCode.BadRequest;
                    result = ex.Message;
                    break;
                case FileNotFoundException ex:
                    code = HttpStatusCode.NotFound;
                    result = ex.Message;
                    break;
                case Exception ex:
                    code = HttpStatusCode.BadRequest;
                    result = ex.Message;
                    break;
                default:
                    result = "Внутренняя ошибка сервера.";
                    break;
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (result == string.Empty)
            {
                result = JsonSerializer.Serialize(new { error = exception.Message });
            }

            await context.Response.WriteAsync(result);
        }
    }
}
