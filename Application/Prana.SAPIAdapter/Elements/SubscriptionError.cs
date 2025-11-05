// ***********************************************************************
// Assembly         : Bloomberg.Library
// Author           : Administrator
// Created          : 05-28-2013
//
// Last Modified By : Administrator
// Last Modified On : 05-28-2013
// ***********************************************************************
// <copyright file="SubscriptionError.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class SubscriptionError
    /// </summary>
    [Serializable]
    public class SubscriptionError : ResponseError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionError"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SubscriptionError(object message)
            : base(message)
        {

        }
    }
}
