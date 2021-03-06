namespace Dappery.Core.Extensions
{
    using System.Reflection;
    using Infrastructure;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public static class StartupExtensions
    {
        /// <summary>
        /// Extension to contain all of our business layer dependencies for our external server providers (ASP.NET Core in our case). 
        /// </summary>
        /// <param name="services">Service collection for dependency injection</param>
        public static void AddDapperyCore(this IServiceCollection services)
        {
            // Add our MediatR and FluentValidation dependencies
            services.AddMediatR(Assembly.GetExecutingAssembly());
            
            // Add our MediatR validation pipeline
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        }
    }
}