using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Workout.API.Models.Middleware
{
    public class ApiKeyAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyAuthenticationMiddleware> _logger;
        private readonly string _apiKeyHeaderName;
        private readonly string[] _validApiKeys;
        private readonly string[] _excludedPaths;

        public ApiKeyAuthenticationMiddleware(
            RequestDelegate next,
            IConfiguration configuration,
            ILogger<ApiKeyAuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;

            _apiKeyHeaderName = configuration["Authentication:ApiKey:HeaderName"] ?? "X-API-Key";
            _validApiKeys = (configuration["Authentication:ApiKey:ValidKeys"] ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries);

            _excludedPaths = (configuration["Authentication:ApiKey:ExcludedPaths"] ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip authentication for excluded paths (e.g., health checks, swagger)
            var requestPath = context.Request.Path.Value?.ToLower();
            if (ShouldSkipAuthentication(requestPath))
            {
                await _next(context);
                return;
            }

            // Check if API key is provided in header
            if (!context.Request.Headers.TryGetValue(_apiKeyHeaderName, out var extractedApiKey))
            {
                _logger.LogWarning("API key was not provided. Request from {IpAddress} to {Path}",
                    context.Connection.RemoteIpAddress, context.Request.Path);

                await HandleUnauthorizedAsync(context, "API key is missing");
                return;
            }

            // Validate the API key
            var key = extractedApiKey.ToString();
            if (!_validApiKeys.Any(k => k == key))
            {
                _logger.LogWarning("Invalid API key: {ApiKey} was provided for request to {Path}",
                    key, context.Request.Path);

                await HandleUnauthorizedAsync(context, "Invalid API key");
                return;
            }

            // API key is valid, proceed with the request
            await _next(context);
        }

        private bool ShouldSkipAuthentication(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            // Skip authentication for excluded paths
            foreach (var excludedPath in _excludedPaths)
            {
                if (path.StartsWith(excludedPath.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }

        private async Task HandleUnauthorizedAsync(HttpContext context, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            var response = new
            {
                context.Response.StatusCode,
                Message = message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            }));
        }
    }
}