// ***********************************************************************
// Assembly         : Bloomberg.Wrapper
// Author           : MJCarlucci
// Created          : 06-14-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 06-14-2013
// ***********************************************************************
// <copyright file="ElementValueAttribute.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class ElementValueAttribute
    /// </summary>
    class ElementValueAttribute : Attribute
    {
        /// <summary>
        /// The type
        /// </summary>
        protected string type;
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementValueAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public ElementValueAttribute(string type)
        {
            this.type = type;
        }
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return type; }
        }
    }
}
