// ***********************************************************************
// Assembly         : PSVar
// Author           : MJCarlucci
// Created          : 04-29-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 04-29-2013
// ***********************************************************************
// <copyright file="InstrumentService.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberg.Library.Requests;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class InstrumentService
    /// </summary>
    public class InstrumentService : UserService
    {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public override string Url
        {
            get { return "//blp/instruments"; }
            set {; }
        }

        /// <summary>
        /// Creates the request.
        /// </summary>
        /// <returns>InstrumentListRequest.</returns>
        public InstrumentListRequest CreateRequest()
        {
            return new InstrumentListRequest(Service.CreateRequest("instrumentListRequest"));
        }
        /// <summary>
        /// Creates the request.
        /// </summary>
        /// <param name="security">The security.</param>
        /// <param name="maxResults">The max results.</param>
        /// <returns>InstrumentListRequest.</returns>
        public InstrumentListRequest CreateRequest(string security, int maxResults)
        {
            return new InstrumentListRequest(Service.CreateRequest("instrumentListRequest"), security, maxResults);
        }
    }
}
