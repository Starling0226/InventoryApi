using System.Net;
using System.Text.Json;

namespace InventoryApi.Exceptions
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,          
                BadRequestException => (int)HttpStatusCode.BadRequest,       
                ConflictException => (int)HttpStatusCode.Conflict,           
                ForbiddenException => (int)HttpStatusCode.Forbidden,         
                UnauthorizedException => (int)HttpStatusCode.Unauthorized,   
                _ => (int)HttpStatusCode.InternalServerError                 
            };

            var response = new
            {
                status = context.Response.StatusCode,
                message = exception.Message,
                error = exception.GetType().Name
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}