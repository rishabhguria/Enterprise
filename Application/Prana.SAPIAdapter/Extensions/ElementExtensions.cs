// ***********************************************************************
// Assembly         : Bloomberg.Library
// Author           : Administrator
// Created          : 05-21-2013
//
// Last Modified By : Administrator
// Last Modified On : 05-23-2013
// ***********************************************************************
// <copyright file="ElementValue.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberg.Library;
using Bloomberglp.Blpapi;
using Prana.LogManager;
using System.Reflection;

namespace System
{
    /// <summary>
    /// Class ElementValue
    /// </summary>
    public static class ElementValue
    {
        /// <summary>
        /// Gets Enum Value's Description Attribute
        /// </summary>
        /// <param name="value">The value you want the description attribute for</param>
        /// <returns>The description, if any, else it's .ToString()</returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public static string ElementName(Enum value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            FieldInfo fi = value.GetType().GetField(value.ToString());
            ElementValueAttribute[] attributes =
                (ElementValueAttribute[])fi.GetCustomAttributes(
                                             typeof(ElementValueAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        public static Element GetElement(Element element, ElementKeys key)
        {
            if (element.HasElement(new Name(key.ToString())))
            {
                return element.GetElement(new Name(key.ToString()));
            }
            return null;
        }

        public static void ProcessMessage(Message message)
        {
            Logger.LoggerWrite(message.MessageType);
            IterateElements(message.AsElement);
        }

        public static void IterateElements(Element parent)
        {
            foreach (Element child in parent.Elements)
            {
                for (int index = 0; index < child.NumValues; index++)
                {
                    if (child.IsArray && child.Datatype == Schema.Datatype.SEQUENCE)
                    {
                        System.Diagnostics.Debug.Print(parent.Name.ToString());
                        IterateElements(child.GetValueAsElement(index));
                    }
                    else if (child.Datatype == Schema.Datatype.SEQUENCE)
                    {
                        System.Diagnostics.Debug.Print(parent.Name.ToString());
                    }
                    else
                    {
                        object name = child.Name;
                        object value = child.GetValue(index);
                        System.Diagnostics.Debug.Print("{2}, {0}={1}", name, value, parent.Name);
                    }
                }
                IterateElements(child);
            }
        }

    }
}
