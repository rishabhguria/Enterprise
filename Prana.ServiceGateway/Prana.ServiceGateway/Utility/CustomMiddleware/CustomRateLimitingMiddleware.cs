using Microsoft.Extensions.Options;
using Prana.ServiceGateway.Models;
using System.Collections.Concurrent;

namespace Prana.ServiceGateway.Utility.CustomMiddleware
{
    /// <summary>
    /// Middleware to apply token bucket rate limiting based on client IP and request path.
    /// </summary>
    public class CustomRateLimitingMiddleware : IMiddleware
    {
        private readonly RateLimitingOptions _options;
        private readonly ILogger<CustomRateLimitingMiddleware> _logger;

        // Thread-safe dictionary to maintain token buckets per IP and request path
        private readonly ConcurrentDictionary<string, TokenBucket> _buckets = new();

        /// <summary>
        /// Constructor to initialize the middleware with options.
        /// </summary>
        /// <param name="options">Injected rate limiting options.</param>
        /// <param name="logger"></param>
        public CustomRateLimitingMiddleware(IOptions<RateLimitingOptions> options, ILogger<CustomRateLimitingMiddleware> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        /// <summary>
        /// Middleware execution logic.
        /// </summary>
        /// <param name="context">HTTP context for the current request.</param>
        /// <param name="next"></param>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                string path = context.Request.Path.ToString().ToLowerInvariant();

                // Skip rate limiting for SignalR/WebSocket connections to prevent disruption
                // Covers paths like: /servicegatewayhub, /servicegatewayhubrtpnl, etc.
                if (path.StartsWith("/servicegatewayhub"))
                {
                    await next(context);
                    return;
                }

                // Use IP + path as key to uniquely identify the rate-limiting scope
                string ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                string key = $"{ip}:{path}";

                // Retrieve or create a token bucket for this key
                var bucket = _buckets.GetOrAdd(key, _ =>
                    new TokenBucket(
                        _options.TokenLimit,
                        _options.TokensPerPeriod,
                        TimeSpan.FromSeconds(_options.ReplenishmentPeriodInSeconds)
                    ));

                // Try to consume a token for the request
                if (bucket.TryConsume())
                {
                    await next(context); // Allow request to continue
                }
                else
                {
                    _logger.LogWarning("Rate limit exceeded. Returning 429 for path: {Path}, IP: {IP}, TokenLimit:{TokenLimit}, Retry-After:{retryAfter} seconds", context.Request.Path, ip, _options.TokenLimit.ToString(), _options.ReplenishmentPeriodInSeconds.ToString());

                    // Reject the request
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.Response.WriteAsync(_options.RejectionMessage);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in CustomRateLimitingMiddleware"); // Log the error so that it can be diagnosed
                await next(context); // Allow request to continue as we don't want to block the request pipeline on an error on rate limiting
                return;
            }
        }
    }
}