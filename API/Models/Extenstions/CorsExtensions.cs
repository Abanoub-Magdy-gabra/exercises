using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Workout.API.Models.Extenstions
{
    /// <summary>
    /// Extensions for configuring CORS (Cross-Origin Resource Sharing)
    /// </summary>
    public static class CorsExtensions
    {
        private const string DefaultCorsPolicyName = "FitnessTrackerCorsPolicy";

        /// <summary>
        /// Adds CORS services to the service collection
        /// </summary>
        public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
        {
            // Get CORS configuration
            var corsConfig = configuration.GetSection("Cors");
            var allowedOrigins = corsConfig.GetSection("AllowedOrigins").Get<string[]>() ?? new[] { "*" };
            var allowedMethods = corsConfig.GetSection("AllowedMethods").Get<string[]>() ?? new[] { "GET", "POST", "PUT", "DELETE", "OPTIONS" };
            var allowedHeaders = corsConfig.GetSection("AllowedHeaders").Get<string[]>() ?? new[] { "*" };
            var exposedHeaders = corsConfig.GetSection("ExposedHeaders").Get<string[]>() ?? new[] { "X-Pagination", "X-Processing-Time-Ms" };
            var allowCredentials = corsConfig.GetValue<bool>("AllowCredentials");
            var maxAge = corsConfig.GetValue<int?>("MaxAge");

            // Configure CORS
            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    // Handle the origins configuration
                    if (allowedOrigins.Length == 1 && allowedOrigins[0] == "*")
                    {
                        builder.AllowAnyOrigin();
                    }
                    else
                    {
                        builder.WithOrigins(allowedOrigins);
                    }

                    // Handle methods
                    if (allowedMethods.Length == 1 && allowedMethods[0] == "*")
                    {
                        builder.AllowAnyMethod();
                    }
                    else
                    {
                        builder.WithMethods(allowedMethods);
                    }

                    // Handle headers
                    if (allowedHeaders.Length == 1 && allowedHeaders[0] == "*")
                    {
                        builder.AllowAnyHeader();
                    }
                    else
                    {
                        builder.WithHeaders(allowedHeaders);
                    }

                    // Expose headers
                    if (exposedHeaders.Length > 0)
                    {
                        builder.WithExposedHeaders(exposedHeaders);
                    }

                    // Allow credentials if configured
                    if (allowCredentials)
                    {
                        builder.AllowCredentials();
                    }
                    else
                    {
                        builder.DisallowCredentials();
                    }

                    // Set max age if configured
                    if (maxAge.HasValue)
                    {
                        builder.SetPreflightMaxAge(TimeSpan.FromSeconds(maxAge.Value));
                    }
                });
            });

            return services;
        }

        /// <summary>
        /// Configures the app to use CORS
        /// </summary>
        public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
        {
            return app.UseCors(DefaultCorsPolicyName);
        }
    }
}