using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Workout.API.Models.Extenstions
{
    /// <summary>
    /// Extensions for configuring Swagger documentation
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Adds Swagger services to the service collection
        /// </summary>
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            // Add API versioning
            object value = services.AddApiVersioning(static options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            });

            // Configure Swagger generation
            services.AddSwaggerGen(static options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FitnessTracker Exercises API",
                    Version = "v1",
                    Description = "RESTful API for managing exercises in the FitnessTracker application",
                    Contact = new OpenApiContact
                    {
                        Name = "FitnessTracker Team",
                        Email = "support@fitnesstracker.com",
                        Url = new Uri("https://fitnesstracker.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "FitnessTracker License",
                        Url = new Uri("https://fitnesstracker.com/license")
                    }
                });

                // Add security definition for API key authentication
                options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Name = "X-API-Key",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "API key authentication using the 'X-API-Key' header"
                });

                // Apply the security requirement to all operations
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            }
                        },
                        new List<string>()
                    }
                });

                // Enable XML comments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });

            return services;
        }

        /// <summary>
        /// Configures the app to use Swagger and Swagger UI
        /// </summary>
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger(static options =>
            {
                options.RouteTemplate = "api-docs/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(static options =>
            {
                options.SwaggerEndpoint("/api-docs/v1/swagger.json", "FitnessTracker Exercises API v1");
                options.RoutePrefix = "api-docs";
                options.DocumentTitle = "FitnessTracker Exercises API Documentation";

                // Customize Swagger UI
                options.EnableDeepLinking();
                options.DisplayRequestDuration();
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });

            return app;
        }
    }
}