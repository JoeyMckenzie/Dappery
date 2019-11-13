namespace Dappery.Api.Middleware
{
    using Microsoft.AspNetCore.Builder;

    public static class ConduitErrorHandlingMiddlewareExtension
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}