using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using GtMotive.Fleet.Api.Authorization;
using GtMotive.Fleet.Api.DependencyInjection;
using GtMotive.Fleet.Api.Filters;
using GtMotive.Fleet.ApplicationCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

[assembly: CLSCompliant(false)]

namespace GtMotive.Fleet.Api
{
    [ExcludeFromCodeCoverage]
    public static class ApiConfiguration
    {
        public static void ConfigureControllers(MvcOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            options.Filters.Add<BusinessExceptionFilter>();
        }

        public static IMvcBuilder WithApiControllers(this IMvcBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.AddApplicationPart(typeof(ApiConfiguration).GetTypeInfo().Assembly);

            AddApiDependencies(builder.Services);

            return builder;
        }

        public static void AddApiDependencies(this IServiceCollection services)
        {
            services.AddAuthorization(AuthorizationOptionsExtensions.Configure);
            services.AddMediatR(typeof(ApiConfiguration).GetTypeInfo().Assembly);
            services.AddUseCases();
            services.AddPresenters();
        }
    }
}
