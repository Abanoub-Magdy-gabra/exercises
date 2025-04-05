using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Workout.API.Models.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly RecyclableMemoryStreamManager _streamManager;
        private readonly string[] _excludedPathsFromBodyLogging;

        public RequestLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _streamManager = new RecyclableMemoryStreamManager();

            // Paths to exclude from detailed body logging (e.g., file uploads, health checks)
            _excludedPathsFromBodyLogging = new[]
            {
                "/api/exercises/media",
                "/health",
                "/favicon.ico"
            };
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Generate a unique ID for this request
            var requestId = Guid.NewGuid().ToString();
            context.Request.Headers["X-Request-ID"] = requestId;

            // Start timer for request duration
            var stopwatch = Stopwatch.StartNew();

            // Capture the request body
            await LogRequest(context, requestId);

            // Capture the response body by wrapping the response stream
            var originalResponseBody = context.Response.Body;
            using var responseBodyStream = _streamManager.GetStream();
            context.Response.Body = responseBodyStream;

            try
            {
                // Call the next middleware
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log any unhandled exceptions (the exception middleware will handle the response)
                _logger.LogError(ex, "Unhandled exception for request {RequestId}", requestId);
                throw;
            }
            finally
            {
                // Log the response
                stopwatch.Stop();
                await LogResponse(context, responseBodyStream, originalResponseBody, stopwatch.ElapsedMilliseconds, requestId);
            }
        }

        private async Task LogRequest(HttpContext context, string requestId)
        {
            context.Request.EnableBuffering();

            var requestPath = context.Request.Path.Value?.ToLower();
            var requestMethod = context.Request.Method;
            var requestContentType = context.Request.ContentType ?? "not specified";
            var requestContentLength = context.Request.ContentLength ?? 0;
            var clientIp = context.Connection.RemoteIpAddress;
            var userAgent = context.Request.Headers["User-Agent"].ToString();

            // Basic request info
            _logger.LogInformation(
                "Request {RequestId}: {Method} {Path} from {ClientIp} - Content-Type: {ContentType}, Length: {ContentLength}, User-Agent: {UserAgent}",
                requestId, requestMethod, requestPath, clientIp, requestContentType, requestContentLength, userAgent);

            // Only log request body if not a file upload or excluded path
            if (!ShouldExcludeFromBodyLogging(requestPath) && requestContentLength > 0 && requestContentLength < 10000)
            {
                using var streamReader = new StreamReader(context.Request.Body);
                var requestBody = await streamReader.ReadToEndAsync();

                // Log the request body (truncate if too large)
                if (requestBody.Length > 1000)
                {
                    _logger.LogDebug("Request {RequestId} Body (truncated): {RequestBody}...",
                        requestId, requestBody.Substring(0, 1000));
                }
                else
                {
                    _logger.LogDebug("Request {RequestId} Body: {RequestBody}",
                        requestId, requestBody);
                }

                // Rewind the stream so the next middleware can read it
                context.Request.Body.Position = 0;
            }
        }

        private async Task LogResponse(HttpContext context, MemoryStream responseBodyStream, Stream originalResponseBody, long elapsedMs, string requestId)
        {
            var requestPath = context.Request.Path.Value?.ToLower();
            var statusCode = context.Response.StatusCode;
            var responseContentType = context.Response.ContentType ?? "not specified";

            // Log basic response info
            _logger.LogInformation(
                "Response {RequestId}: {StatusCode} for {Method} {Path} - Took {ElapsedMs}ms, Content-Type: {ContentType}, Length: {ContentLength}",
                requestId, statusCode, context.Request.Method, requestPath, elapsedMs, responseContentType, responseBodyStream.Length);

            // Only log response body for non-binary content and not too large
            if (!ShouldExcludeFromBodyLogging(requestPath) && responseBodyStream.Length > 0 && responseBodyStream.Length < 10000)
            {
                // Rewind the memory stream
                responseBodyStream.Position = 0;

                using var streamReader = new StreamReader(responseBodyStream);
                var responseBody = await streamReader.ReadToEndAsync();

                // Log the response body (truncate if too large)
                if (responseBody.Length > 1000)
                {
                    _logger.LogDebug("Response {RequestId} Body (truncated): {ResponseBody}...",
                        requestId, responseBody.Substring(0, 1000));
                }
                else
                {
                    _logger.LogDebug("Response {RequestId} Body: {ResponseBody}",
                        requestId, responseBody);
                }
            }

            // Copy the response body to the original stream and restore it
            responseBodyStream.Position = 0;
            await responseBodyStream.CopyToAsync(originalResponseBody);
            context.Response.Body = originalResponseBody;
        }

        private bool ShouldExcludeFromBodyLogging(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            foreach (var excludedPath in _excludedPathsFromBodyLogging)
            {
                if (path.StartsWith(excludedPath.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}