//using System.Threading.RateLimiting;

namespace Prana.ServiceGateway.Models
{
    public class RateLimitingOptions
    {
        /// <summary>
        /// Maximum tokens that can be stored in the bucket (burst capacity).
        /// </summary>
        public int TokenLimit { get; set; } = 20;

        /// <summary>
        /// Number of tokens added per replenishment period.
        /// </summary>
        public int TokensPerPeriod { get; set; } = 2;

        /// <summary>
        /// Interval (in seconds) at which tokens are added.
        /// </summary>
        public int ReplenishmentPeriodInSeconds { get; set; } = 1;

        /// <summary>
        /// Maximum number of queued requests when tokens are exhausted.
        /// </summary>
        public int QueueLimit { get; set; } = 0;

        /// <summary>
        /// Optional policy name for advanced use (e.g., per endpoint mapping).
        /// </summary>
        public string PolicyName = "IpAndPathPolicy";

        /// <summary>
        /// Optional custom rejection message for 429 responses.
        /// </summary>
        public string RejectionMessage = "Too many requests. Please try again later.";
    }

}
