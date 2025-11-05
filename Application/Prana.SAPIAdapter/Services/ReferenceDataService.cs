// ***********************************************************************
// Assembly         : Bloomberg.Wrapper
// Author           : MJCarlucci
// Created          : 06-11-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 06-15-2013
// ***********************************************************************
// <copyright file="ReferenceDataService.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberg.Library.Requests;
using Bloomberglp.Blpapi;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class ReferenceDataService
    /// </summary>
    public class ReferenceDataService : UserService
    {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public override string Url
        {
            get { return "//blp/refdata"; }
            set {; }
        }



        /// <summary>
        /// Creates the request.
        /// </summary>
        /// <returns>ReferenceDataRequest.</returns>
        public ReferenceDataRequest CreateRequest()
        {
            return new ReferenceDataRequest(Service.CreateRequest("ReferenceDataRequest"));
        }


        /// <summary>
        /// Creates the request.
        /// </summary>
        /// <param name="forceDelay">if set to <c>true</c> [force delay].</param>
        /// <param name="returnEids">if set to <c>true</c> [return eids].</param>
        /// <param name="useUTCTime">if set to <c>true</c> [use UTC time].</param>
        /// <returns>ReferenceDataRequest.</returns>
        public ReferenceDataRequest CreateRequest(bool forcedDelay, bool returnEids, bool useUTCTime)
        {
            ReferenceDataRequest request = new ReferenceDataRequest(
                Service.CreateRequest("ReferenceDataRequest"),
                returnEids, false, useUTCTime, forcedDelay);

            return request;
        }

    }
}
