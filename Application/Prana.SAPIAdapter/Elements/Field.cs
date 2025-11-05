// ***********************************************************************
// Assembly         : Bloomberg.Library
// Author           : Administrator
// Created          : 01-14-2013
//
// Last Modified By : Administrator
// Last Modified On : 05-23-2013
// ***********************************************************************
// <copyright file="Field.cs" company="">
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
    /// Field
    /// </summary>
    [Serializable]
    public class Field
    {
        /// <summary>
        /// The FIEL d_ ID
        /// </summary>
        public static readonly Name FIELD_ID = new Name("id");
        /// <summary>
        /// The FIEL d_ MNEMONIC
        /// </summary>
        public static readonly Name FIELD_MNEMONIC = new Name("mnemonic");
        /// <summary>
        /// The FIEL d_ DATA
        /// </summary>
        public static readonly Name FIELD_DATA = new Name("fieldData");
        /// <summary>
        /// The FIEL d_ DESC
        /// </summary>
        public static readonly Name FIELD_DESC = new Name("description");
        /// <summary>
        /// The FIEL d_ INFO
        /// </summary>
        public static readonly Name FIELD_INFO = new Name("fieldInfo");
        /// <summary>
        /// The FIEL d_ ERROR
        /// </summary>
        public static readonly Name FIELD_ERROR = new Name("fieldError");
        /// <summary>
        /// The FIEL d_ MSG
        /// </summary>
        public static readonly Name FIELD_MSG = new Name("message");


        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public string Id;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name;
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value;
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description;

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public string DataType;

        /// <summary>
        /// Gets or sets the name of the internal.
        /// </summary>
        /// <value>The name of the internal.</value>
        public string InternalName;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public Field()
        {

        }

        public static void IterateElements(Element parent)
        {
            if (parent.Datatype == Schema.Datatype.SEQUENCE)
            {
                System.Diagnostics.Debug.Print(parent.Name.ToString());
            }
            foreach (Element child in parent.Elements)
            {
                for (int index = 0; index < child.NumValues; index++)
                {
                    if (child.IsArray && child.Datatype == Schema.Datatype.SEQUENCE)
                    {
                        IterateElements(child.GetValueAsElement(index));
                    }
                    else if (child.Datatype == Schema.Datatype.SEQUENCE)
                    {
                        System.Diagnostics.Debug.Print(parent.Name.ToString());
                    }
                    else
                    {
                        object value = child.GetValue(index);
                    }
                }
                IterateElements(child);
            }
        }


        /// <summary>
        /// Updates the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        public void Update(Element field)
        {
            String fldMnemonic, fldDesc;

            if (field.HasElement(FIELD_INFO))
            {
                Element fldInfo = field.GetElement(FIELD_INFO);
                fldMnemonic = fldInfo.GetElementAsString(FIELD_MNEMONIC);
                fldDesc = fldInfo.GetElementAsString(FIELD_DESC);
                Name = fldMnemonic;
                Description = fldDesc;
                Id = field.GetElementAsString(FIELD_ID);
            }
            else if (field.HasElement(FIELD_ERROR))
            {
                Element fldError = field.GetElement(FIELD_ERROR);
                fldDesc = fldError.GetElementAsString(FIELD_MSG);
                Description = fldDesc;
            }
            else
            {
                DataType = field.Datatype.ToString();
                Name = field.Name.ToString();

                if (field.Datatype == Schema.Datatype.DATE && field.NumValues == 1)
                {
                    Value = DateTime.Parse(field.GetValueAsString(0));
                }

                //else if (field.Datatype == Schema.Datatype.SEQUENCE)

                //if (field.Datatype.ToString().ToUpper().Contains("DATE"))
                //{
                //    if (field.NumValues == 1)
                //        Value = DateTime.Parse(field.GetValueAsString(0));
                //    else if (field.NumValues > 1)
                //    {
                //        Value = new List<object>();
                //        for (int row = 0; row < field.NumValues; row++)
                //        {
                //            ((List<object>)Value).Add(field.GetValueAsString(0));
                //        }

                //    }
                //}
                else
                {
                    if (field.NumValues == 1)
                        Value = field.GetValue(0);
                    else if (field.NumValues > 1)
                    {
                        Value = new List<Fields>();
                        for (int i = 0; i < field.NumValues; i++)
                        {
                            Fields xx = new Fields(field.GetValueAsElement(i));
                            ((List<Fields>)Value).Add(xx);
                        }

                        //for (int row = 0; row < field.NumValues; row++)
                        //{
                        //   // Element e1 = field.GetValueAsElement(row);

                        //    ((List<object>)Value).Add(field.GetValue(row));
                        //}
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Field" /> class.
        /// </summary>
        /// <param name="field">The field.</param>
        public Field(Element field)
        {
            Update(field);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0}={1}({2})", Name, Value.GetType(), Value);
        }

    }
}
