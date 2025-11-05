// ***********************************************************************
// Assembly         : Bloomberg.Library
// Author           : Administrator
// Created          : 05-23-2013
//
// Last Modified By : Administrator
// Last Modified On : 05-23-2013
// ***********************************************************************
// <copyright file="FieldInfo.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberglp.Blpapi;
using System;
using System.Collections.Generic;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class FieldInfo
    /// </summary>
    [Serializable]
    public class FieldSchema
    {
        /// <summary>
        /// Gets or sets the mnemonic.
        /// </summary>
        /// <value>The mnemonic.</value>
        public string Mnemonic;
        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public string DataType;

        /// <summary>
        /// Gets or sets the type of the field.
        /// </summary>
        /// <value>The type of the field.</value>
        public string FieldType;
        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>The name of the category.</value>
        public string CategoryName;

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>The categories.</value>
        public List<string> Categories;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description;
        /// <summary>
        /// Gets or sets the documentation.
        /// </summary>
        /// <value>The documentation.</value>
        public string Documentation;

        /// <summary>
        /// Gets or sets the property id.
        /// </summary>
        /// <value>The property id.</value>
        public string PropertyId;

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        /// <value>The property value.</value>
        public string PropertyValue;

        /// <summary>
        /// Gets or sets the overrides.
        /// </summary>
        /// <value>The overrides.</value>
        public List<Override> Overrides;

        /// <summary>
        /// Gets or sets the field error.
        /// </summary>
        /// <value>The field error.</value>
        public FieldError FieldError;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldSchema"/> class.
        /// </summary>
        /// <param name="fieldInfo">The value.</param>
        public FieldSchema(Element fieldInfo)
        {

            Mnemonic = fieldInfo.GetElementAsString(new Name("mnemonic"));
            DataType = fieldInfo.GetElementAsString(new Name("datatype"));

            if (fieldInfo.HasElement(new Name("categoryName")))
            {
                if (fieldInfo.GetElement(new Name("categoryName")).NumValues > 0)
                {
                    CategoryName = fieldInfo.GetElementAsString(new Name("categoryName"));
                    Categories = new List<string>(CategoryName.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
                }
            }


            Description = fieldInfo.GetElementAsString(new Name("description"));

            if (fieldInfo.HasElement(new Name("documentation")))
            {
                Documentation = fieldInfo.GetElementAsString(new Name("documentation"));
            }

            if (fieldInfo.HasElement(new Name("ftype")))
            {
                FieldType = fieldInfo.GetElementAsString(new Name("ftype"));
            }

            if (fieldInfo.HasElement(new Name("property")))
            {
                Element property = fieldInfo.GetElement(new Name("property"));
                if (property.IsArray)
                {
                    //TODO: Need to check this. Documentation doesn't seem to match return values
                    for (int index = 0; index < property.NumValues; index++)
                    {
                        PropertyId = property.GetElementAsString(new Name("id"));
                        PropertyValue = property.GetElementAsString(new Name("value"));
                    }
                }
            }

            if (fieldInfo.HasElement(new Name("overrides")))
            {
                Element overrides = fieldInfo.GetElement(new Name("overrides"));
                if (overrides.IsArray)
                {
                    for (int i = 0; i < overrides.NumElements; i++)
                    {
                        Overrides.Add(new Override(overrides.GetElement(i)));
                    }
                }
            }
        }



    }
}
