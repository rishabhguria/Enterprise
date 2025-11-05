using Prana.ServiceGateway.Constants;

namespace Prana.ServiceGateway.Models
{
    /// <summary>
    /// Represents a token for touch authentication.
    /// </summary>
    public class TouchTokenDto
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public long Timestamp { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TouchTokenDto"/> class.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null or empty.</exception>
        public TouchTokenDto(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName), MessageConstants.MSG_CONST_USERNAME_BLANK);

            UserName = userName;
            Timestamp = DateTime.UtcNow.Ticks;
        }
    }
}
