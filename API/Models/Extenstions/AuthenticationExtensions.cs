using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Workout.API.Models.Extenstions
{
    /// <summary>
    /// Extensions for configuring authentication
    /// </summary>
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Adds JWT authentication to the service collection
        /// </summary>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Get JWT configuration
            var jwtSection = configuration.GetSection("Authentication:Jwt");
            var jwtSettings = new JwtSettings
            {
                Secret = jwtSection["Secret"],
                Issuer = jwtSection["Issuer"],
                Audience = jwtSection["Audience"],
                ExpirationMinutes = int.TryParse(jwtSection["ExpirationMinutes"], out var mins) ? mins : 60
            };

            // Validate required settings
            if (string.IsNullOrEmpty(jwtSettings.Secret))
            {
                throw new InvalidOperationException("JWT Secret is not configured");
            }

            // Configure token validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = !string.IsNullOrEmpty(jwtSettings.Issuer),
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = !string.IsNullOrEmpty(jwtSettings.Audience),
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            // Add JWT authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
                options.SaveToken = true;

                // Add event handling
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        /// <summary>
        /// Adds authorization policies to the service collection
        /// </summary>
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Define policy for administrators
                options.AddPolicy("RequireAdminRole", policy =>
                    policy.RequireRole("Admin"));

                // Define policy for coaches
                options.AddPolicy("RequireCoachRole", policy =>
                    policy.RequireRole("Coach"));

                // Define policy for content creators (admins or coaches)
                options.AddPolicy("RequireContentCreator", policy =>
                    policy.RequireRole("Admin", "Coach"));

                // Define policy requiring verified user
                options.AddPolicy("RequireVerifiedUser", policy =>
                    policy.RequireClaim("verified", "true"));
            });

            return services;
        }

        /// <summary>
        /// Settings for JWT authentication
        /// </summary>
        private class JwtSettings
        {
            public string Secret { get; set; }
            public string Issuer { get; set; }
            public string Audience { get; set; }
            public int ExpirationMinutes { get; set; }
        }
    }
}