// ***********************************************************************
// Assembly         : Bloomberg.Wrapper
// Author           : MJCarlucci
// Created          : 06-15-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 06-15-2013
// ***********************************************************************
// <copyright file="Overrides.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberglp.Blpapi;
using System.Collections.Generic;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class Overrides
    /// </summary>
    public class Overrides : List<Override>
    {
        /// <summary>
        /// The overrides
        /// </summary>
        private readonly Element overrides = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Overrides"/> class.
        /// </summary>
        /// <param name="overrides">The overrides.</param>
        public Overrides(Element overrides)
        {
            this.overrides = overrides;
        }

        /// <summary>
        /// Adds the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void Add(string name, string value)
        {
            Override item = new Override(overrides, name, value);
            Add(item);
        }

    }
}
