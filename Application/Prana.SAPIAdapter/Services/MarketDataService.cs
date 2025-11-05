// ***********************************************************************
// Assembly         : Bloomberg Library
// Author           : MJCarlucci
// Created          : 04-29-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 04-29-2013
// ***********************************************************************
// <copyright file="MarektDataService.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberg.Library.Requests;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class MarketDataService
    /// </summary>
    public class MarketDataService : UserService
    {

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public override string Url
        {
            get { return "//blp/mktdata"; }
            set {; }
        }

        /// <summary>
        /// Creates the request.
        /// </summary>
        /// <returns>ReferenceDataRequest.</returns>
        public MarketDataRequest CreateRequest()
        {
            return new MarketDataRequest();
        }
    }
}
