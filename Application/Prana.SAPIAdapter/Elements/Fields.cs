// ***********************************************************************
// Assembly         : Bloomberg.Library
// Author           : Administrator
// Created          : 05-21-2013
//
// Last Modified By : Administrator
// Last Modified On : 05-23-2013
// ***********************************************************************
// <copyright file="FieldData.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberglp.Blpapi;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class Fields
    /// </summary>
    [Serializable]
    public class Fields : Dictionary<string, Field>
    {
        /// <summary>
        /// Unique List of fields in use for the given instance
        /// </summary>
        public static Dictionary<string, int> gFields = new Dictionary<string, int>();

        /// <summary>
        /// Gets or sets the field info from a search request
        /// </summary>
        /// <value>The field info.</value>
        public FieldSchema FieldInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="Fields"/> class.
        /// </summary>
        /// <param name="fields">The fields.</param>
        public Fields(Element fields)
        {
            Update(fields);
        }
        protected Fields(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
        [SecurityPermissionAttribute(SecurityAction.Demand,
             SerializationFormatter = true)]
        public override void GetObjectData(
           SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
        /// <summary>
        /// Updates the specified fields.
        /// </summary>
        /// <param name="fields">The fields.</param>
        public void Update(Element fields)
        {
            //if (fields.Datatype == Schema.Datatype.SEQUENCE)
            //{
            //    Update(fields.Elements);
            //    return;
            //}

            Field field;
            foreach (Element item in fields.Elements)
            {
                //if (fields.Datatype == Schema.Datatype.SEQUENCE)
                //{
                //    Update(fields.Elements);
                //    return;
                //}                 

                if (TryGetValue(item.Name.ToString(), out field))
                {
                    field.Update(item);
                }
                else
                {
                    field = new Field(item);
                    Add(item.Name.ToString(), field);
                }
                int tmp;
                if (gFields.TryGetValue(field.Name, out tmp) == false)
                {
                    //System.Diagnostics.Debug.Print(field.Name);
                    gFields.Add(field.Name, 1);
                }
                else
                    gFields[field.Name] += 1;
            }
        }


        /// <summary>
        /// Get and Convert Value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topic"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public T GetValue<T>(string key, T def)
        {

            if (ContainsKey(key) == false) return def;

            object value = this[key].Value;
            object tvalue = Convert.ChangeType(value == null ? def : value, typeof(T));
            return (value == null) ? def : (T)tvalue;
        }
        /// <summary>
        /// Finds the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Field.</returns>
        public Field Find(string name)
        {
            Field field;
            if (TryGetValue(name, out field))
                return field;

            return null;
        }

        internal void Print()
        {
        }
    }
}
