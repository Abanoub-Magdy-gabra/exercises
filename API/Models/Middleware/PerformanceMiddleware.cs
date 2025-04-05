using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FitnessTracker.Exercises.API.Middleware
{
    public class PerformanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceMiddleware> _logger;
        private readonly Stopwatch _stopwatch;
        private readonly int _slowRequestThresholdMs;

        public PerformanceMiddleware(
            RequestDelegate next,
            ILogger<PerformanceMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _stopwatch = new Stopwatch();

            // Configure the threshold for slow requests (in milliseconds)
            _slowRequestThresholdMs = 500; // Default value
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _stopwatch.Restart();

            // Store the start time in the HttpContext items to make it available to other components
            context.Items["RequestStartTime"] = DateTime.UtcNow;

            // Call the next middleware
            await _next(context);

            _stopwatch.Stop();

            // Calculate execution time
            var elapsedMs = _stopwatch.ElapsedMilliseconds;

            // Add response header with processing time
            context.Response.Headers["X-Processing-Time-Ms"] = elapsedMs.ToString();

            // Log slow requests
            if (elapsedMs > _slowRequestThresholdMs)
            {
                _logger.LogWarning("Slow request: {Method} {Path} took {ElapsedMs}ms",
                    context.Request.Method, context.Request.Path, elapsedMs);

                // Could also record metrics to a monitoring system here
            }

            // Log all request performance at trace level
            _logger.LogTrace("Request performance: {Method} {Path} - {StatusCode} - {ElapsedMs}ms",
                context.Request.Method, context.Request.Path, context.Response.StatusCode, elapsedMs);
        }
    }
}