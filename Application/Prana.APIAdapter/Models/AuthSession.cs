using System;

namespace Prana.APIAdapter.Models
{
    public class AuthSession
    {
        /// <summary>
        /// key
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// expires
        /// </summary>
        public DateTime expires { get; set; }

        /// <summary>
        /// is Authenticated
        /// </summary>
        public bool isAuthenticated { get; set; }

        /// <summary>
        /// message
        /// </summary>
        public string message { get; set; }

    }
}
