using FitnessTracker.Exercises.API.Middleware;
using Microsoft.AspNetCore.Builder;
using Workout.API.Models.Middleware;

namespace FitnessTracker.Exercises.API.Extensions
{
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Adds the custom exception handling middleware to the application pipeline.
        /// </summary>
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        /// <summary>
        /// Adds the API key authentication middleware to the application pipeline.
        /// </summary>
        public static IApplicationBuilder UseApiKeyAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyAuthenticationMiddleware>();
        }

        /// <summary>
        /// Adds the request logging middleware to the application pipeline.
        /// </summary>
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }

        /// <summary>
        /// Adds the performance monitoring middleware to the application pipeline.
        /// </summary>
        public static IApplicationBuilder UsePerformanceMonitoring(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PerformanceMiddleware>();
        }

        /// <summary>
        /// Adds all custom middleware components in the recommended order.
        /// </summary>
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder
                .UsePerformanceMonitoring()   // 1. Performance monitoring (should be first to measure entire pipeline)
                .UseRequestLogging()          // 2. Request/response logging
                .UseApiKeyAuthentication()     // 3. API key authentication
                .UseCustomExceptionHandler();  // 4. Exception handling (should be last to catch all exceptions)
        }
    }
}