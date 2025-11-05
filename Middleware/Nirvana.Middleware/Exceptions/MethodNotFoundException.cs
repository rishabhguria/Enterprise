using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Nirvana.Middleware.Exceptions
{
    /// <summary>
    /// Method not found Exception Handler
    /// </summary>
    /// <remarks></remarks>
    public class MethodNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public MethodNotFoundException()
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public MethodNotFoundException(string message)
            : base(message)
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <remarks></remarks>
        public MethodNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        ///   
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        /// <remarks></remarks>
        protected MethodNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            
        }
    }
}
