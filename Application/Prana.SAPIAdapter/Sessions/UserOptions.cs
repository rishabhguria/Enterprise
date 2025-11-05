// ***********************************************************************
// Assembly         : Bloomberg Library
// Author           : MJCarlucci
// Created          : 04-29-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 04-29-2013
// ***********************************************************************
// <copyright file="UserOptions.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberglp.Blpapi;

namespace Bloomberg.Library
{
    /// <summary>
    /// User Options
    /// </summary>
    public class UserOptions : SessionOptions
    {
        /// <summary>
        /// User Options Construct
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        public UserOptions(string host, int port)
        {
            ServerHost = host;
            ServerPort = port;
        }
    }
}
