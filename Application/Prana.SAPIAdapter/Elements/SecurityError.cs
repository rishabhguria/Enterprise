using Bloomberglp.Blpapi;
using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Security Error
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class SecurityError : ResponseError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public SecurityError()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityError"/> class.
        /// </summary>
        /// <param name="securityError">The security error.</param>
        /// <remarks></remarks>
        public SecurityError(Element securityError)
        {
            Source = securityError.GetElementAsString(new Name("source"));
            Code = securityError.GetElementAsInt32(new Name("code"));
            Category = securityError.GetElementAsString(new Name("category"));
            Message = securityError.GetElementAsString(new Name("message"));
            Subcategory = securityError.GetElementAsString(new Name("subcategory"));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return Message;
        }
    }
}
