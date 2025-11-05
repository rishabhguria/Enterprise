// ***********************************************************************
// Assembly         : Bloomberg Library
// Author           : MJCarlucci
// Created          : 04-29-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 04-29-2013
// ***********************************************************************
// <copyright file="MarketDataEventArgs.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Prana.SAPIAdapter
{

    /// <summary>
    /// Class MarketDataEventsArgs
    /// </summary>
    public class MarketDataEventsArgs : Prana.BusinessObjects.Data
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarketDataEventsArgs"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        public MarketDataEventsArgs(Prana.BusinessObjects.SymbolData info)
        {
            Info = info;
        }
    }
}
