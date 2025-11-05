// ***********************************************************************
// Assembly         : Bloomberg.Wrapper
// Author           : MJCarlucci
// Created          : 06-11-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 06-11-2013
// ***********************************************************************
// <copyright file="AuthorizationRequest.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberglp.Blpapi;

namespace Bloomberg.Library.Requests
{
    /// <summary>
    /// Class AuthorizationRequest
    /// </summary>
    public class AuthorizationRequest : AbstractRequest
    {

        /// <summary>
        /// Gets or sets the identity.
        /// </summary>
        /// <value>The identity.</value>
        public Identity Identity;

        /// <summary>
        /// Gets or sets the correlation id.
        /// </summary>
        /// <value>The correlation id.</value>
        public CorrelationID CorrelationId;

        /// <summary>
        /// Gets or sets the UUID.
        /// </summary>
        /// <value>The UUID.</value>
        public string UUID
        {
            get
            {
                Element e = GetElement("uuid");
                return e.GetValue().ToString();
            }
            set
            {
                Set("uuid", value);
            }
        }
        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        public string ipAddress
        {
            get
            {
                Element e = GetElement("ipAddress");
                return e.GetValue().ToString();
            }
            set
            {
                Set("ipAddress", value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationRequest"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="uuid">The UUID.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="identity">The identity.</param>
        /// <param name="correlationId">The correlation id.</param>
        public AuthorizationRequest(Request request, string uuid, string ipAddress, Identity identity, CorrelationID correlationId)
            : base(request)
        {
            UUID = uuid;
            this.ipAddress = ipAddress;
            this.Identity = identity;
            this.CorrelationId = correlationId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationRequest"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="uuid">The UUID.</param>
        /// <param name="ipAddress">The ip address.</param>
        public AuthorizationRequest(Request request, string uuid, string ipAddress)
            : base(request)
        {
            UUID = uuid;
            this.ipAddress = ipAddress;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRequest" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public AuthorizationRequest(Request request)
            : base(request)
        {

        }
    }
}
