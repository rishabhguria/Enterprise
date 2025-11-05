// ***********************************************************************
// Assembly         : Bloomberg.Library
// Author           : Administrator
// Created          : 05-23-2013
//
// Last Modified By : Administrator
// Last Modified On : 05-24-2013
// ***********************************************************************
// <copyright file="FieldData.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberglp.Blpapi;
using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class FieldSearchRequest
    /// </summary>
    [Serializable]
    public class FieldData
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public string Id;

        /// <summary>
        /// Gets or sets the field info.
        /// </summary>
        /// <value>The field info.</value>
        public FieldSchema FieldInfo;

        /// <summary>
        /// Gets or sets the field error.
        /// </summary>
        /// <value>The field error.</value>
        public FieldError FieldError;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldData"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        public FieldData(Element element)
        {
            if (element.HasElement(new Name("id")))
            {
                Id = element.GetElement(new Name("id")).GetValueAsString();
            }
            if (element.HasElement(new Name("fieldInfo")))
            {
                FieldInfo = new FieldSchema(element.GetElement(new Name("fieldInfo")));
            }
            if (element.HasElement(new Name("fieldError")))
            {
                FieldError = new FieldError(element.GetElement(new Name("fieldError")));
            }
        }

    }
}
