using Microsoft.AspNetCore.Http;

namespace PetLuvSystem.SharedLibrary.Middlewares
{
    public class ListenToOnlyApiGateway(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var signedHeader = context.Request.Headers["Api-Gateway"];

            if (signedHeader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Service Is Unavailable");
                return;
            }
            else
            {
                await next(context);
            }
        }
    }
};
