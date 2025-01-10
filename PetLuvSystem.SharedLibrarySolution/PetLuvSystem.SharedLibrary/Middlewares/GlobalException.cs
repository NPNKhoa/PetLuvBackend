using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Logs;
using System.Net;
using System.Text.Json;

namespace PetLuvSystem.SharedLibrary.Middlewares
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            string title = "Error";
            string message = "Internal Server Error";
            int statusCode = (int)HttpStatusCode.InternalServerError;

            try
            {
                await next(context);

                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "Too many requests";
                    statusCode = StatusCodes.Status429TooManyRequests;

                    await ModifyHeader(context, title, message, statusCode);
                }

                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Unauthorized";
                    message = "Invalid token or token is expired";
                    statusCode = StatusCodes.Status401Unauthorized;

                    await ModifyHeader(context, title, message, statusCode);
                }

                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Forbidden";
                    message = "You don't have permission to access these resources";
                    statusCode = StatusCodes.Status403Forbidden;

                    await ModifyHeader(context, title, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                if (ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Warning";
                    message = "Request Timeout";
                    statusCode = StatusCodes.Status408RequestTimeout;

                    await ModifyHeader(context, title, message, statusCode);
                }

                await ModifyHeader(context, title, message, statusCode);
            }
        }

        private static async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Title = title,
                Detail = message,
                Status = statusCode
            }), CancellationToken.None);

            return;
        }
    }
}
