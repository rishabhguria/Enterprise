// ***********************************************************************
// Assembly         : Bloomberg.Library
// Author           : Administrator
// Created          : 05-23-2013
//
// Last Modified By : Administrator
// Last Modified On : 05-23-2013
// ***********************************************************************
// <copyright file="FieldError.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberglp.Blpapi;
using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class FieldError
    /// </summary>
    [Serializable]
    public class FieldError : ResponseError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldError"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        public FieldError(Element element)
            : base(element)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldError"/> class.
        /// </summary>
        public FieldError() { }

    }
}
