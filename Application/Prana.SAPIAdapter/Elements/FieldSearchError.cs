// ***********************************************************************
// Assembly         : Bloomberg.Library
// Author           : Administrator
// Created          : 05-23-2013
//
// Last Modified By : Administrator
// Last Modified On : 05-23-2013
// ***********************************************************************
// <copyright file="FieldSearchError.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberglp.Blpapi;
using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class FieldSearchError
    /// </summary>
    [Serializable]
    public class FieldSearchError : ResponseError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldSearchError"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        public FieldSearchError(Element element)
            : base(element) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldSearchError"/> class.
        /// </summary>
        public FieldSearchError() { }
    }
}
