namespace Prana.ServiceGateway.Utility
{
    /// <summary>
    /// Implements the Token Bucket algorithm for rate limiting.
    /// Allows a fixed number of tokens (requests) and refills them at a defined interval.
    /// </summary>
    public class TokenBucket
    {
        private readonly object _lock = new(); // Ensures thread safety.
        private int _tokens;                   // Current number of available tokens.
        private readonly int _capacity;        // Maximum number of tokens the bucket can hold.
        private readonly int _refillAmount;    // Number of tokens to add on each refill.
        private readonly TimeSpan _refillPeriod; // Time interval after which tokens are added.
        private DateTime _lastRefill;          // Timestamp of the last refill.

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenBucket"/> class.
        /// </summary>
        /// <param name="capacity">Maximum number of tokens the bucket can hold.</param>
        /// <param name="refillAmount">Number of tokens added per refill period.</param>
        /// <param name="refillPeriod">Interval after which tokens are replenished.</param>
        public TokenBucket(int capacity, int refillAmount, TimeSpan refillPeriod)
        {
            _capacity = capacity;
            _refillAmount = refillAmount;
            _refillPeriod = refillPeriod;
            _tokens = capacity; // Initially full
            _lastRefill = DateTime.UtcNow;
        }

        /// <summary>
        /// Attempts to consume a token from the bucket.
        /// </summary>
        /// <returns>True if a token was available and consumed; otherwise, false.</returns>
        public bool TryConsume()
        {
            lock (_lock)
            {
                RefillIfNeeded();

                if (_tokens > 0)
                {
                    _tokens--;
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Returns the current number of available tokens without consuming any.
        /// </summary>
        /// <returns>The number of available tokens.</returns>
        public int PeekTokens()
        {
            lock (_lock)
            {
                RefillIfNeeded();
                return _tokens;
            }
        }

        /// <summary>
        /// Refills tokens based on the elapsed time since the last refill.
        /// </summary>
        private void RefillIfNeeded()
        {
            var now = DateTime.UtcNow;
            var timeElapsed = now - _lastRefill;

            if (timeElapsed >= _refillPeriod)
            {
                // Calculate how many refill periods have passed
                var periods = (int)(timeElapsed.TotalSeconds / _refillPeriod.TotalSeconds);
                var refillTokens = periods * _refillAmount;

                // Add tokens up to the capacity limit
                _tokens = Math.Min(_tokens + refillTokens, _capacity);
                _lastRefill = now;
            }
        }
    }
}