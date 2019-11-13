namespace Dappery.Core.Extensions
{
    using System.Reflection;
    using AutoMapper;
    using Infrastructure;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public static class StartupExtensions
    {
        public static void AddDapperyCore(this IServiceCollection services)
        {
            // Add our AutoMapper, MediatR, and FluentValidation dependencies
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            
            // Add our MediatR validation pipeline
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        }
    }
}