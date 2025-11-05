// ***********************************************************************
// Assembly         : Bloomberg.Library
// Author           : Administrator
// Created          : 05-22-2013
//
// Last Modified By : Administrator
// Last Modified On : 05-23-2013
// ***********************************************************************
// <copyright file="Override.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberglp.Blpapi;
using System;

namespace Bloomberg
{
    /// <summary>
    /// Bloomberg Override
    /// </summary>
    [Serializable]
    public class Override
    {
        /// <summary>
        /// The element
        /// </summary>
        [NonSerializedAttribute]
        readonly Element element = null;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name;
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Override" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Override(Element value)
        {
            Name = value.GetElementAsString(new Name("id"));
            Value = value.GetElementAsString(new Name("value"));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Override" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Override(Element item, string name, string value)
        {
            element = item;
            element.SetElement(new Name("fieldId"), name);
            element.SetElement(new Name("value"), value);

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Override"/> class.
        /// </summary>
        public Override() { }
    }
}
