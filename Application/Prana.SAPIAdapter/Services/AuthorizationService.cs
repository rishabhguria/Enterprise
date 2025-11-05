// ***********************************************************************
// Assembly         : Bloomberg.Wrapper
// Author           : MJCarlucci
// Created          : 06-11-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 06-12-2013
// ***********************************************************************
// <copyright file="AuthorizationService.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberg.Library.Requests;
using Bloomberglp.Blpapi;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class AuthorizationService
    /// </summary>
    public class AuthorizationService : UserService
    {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public override string Url
        {
            get { return "//blp/apiauth"; }
            set {; }

        }

        /// <summary>
        /// Creates the authorization request.
        /// </summary>
        /// <param name="uuid">The UUID.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>AuthorizationRequest.</returns>
        public AuthorizationRequest CreateRequest(string uuid, string ipAddress, Identity identity, CorrelationID correlationId)
        {
            return new AuthorizationRequest(Service.CreateAuthorizationRequest(), uuid, ipAddress, identity, correlationId);

        }

        /// <summary>
        /// Creates the authorization request.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="uuid">The UUID.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>AuthorizationRequest.</returns>
        public AuthorizationRequest CreateRequest(string uuid, string ipAddress)
        {
            return new AuthorizationRequest(Service.CreateAuthorizationRequest(), uuid, ipAddress, Session.CreateIdentity(), new CorrelationID(uuid));

        }


        /// <summary>
        /// Creates the authorization request.
        /// </summary>
        /// <returns>AuthorizationRequest.</returns>
        public new AuthorizationRequest CreateAuthorizationRequest()
        {
            return new AuthorizationRequest(Service.CreateAuthorizationRequest());
        }


    }
}
