// ***********************************************************************
// Assembly         : Bloomberg.Wrapper
// Author           : MJCarlucci
// Created          : 06-14-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 06-14-2013
// ***********************************************************************
// <copyright file="AbstractRequest.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberglp.Blpapi;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bloomberg.Library.Requests
{
    /// <summary>
    /// Class AbstractRequest
    /// </summary>
	public class AbstractRequest : Request
    {
        /// <summary>
        /// The request
        /// </summary>
		public readonly Request request;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRequest"></see> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public AbstractRequest(Request request)
        {
            this.request = request;
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(string name, byte[] elementValue)
        {
            request.Append(name, elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(Name name, byte[] elementValue)
        {
            request.Append(name, elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(string name, string elementValue)
        {
            request.Append(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(Name name, string elementValue)
        {
            request.Append(name, elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(string name, Name elementValue)
        {
            request.Append(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(Name name, Name elementValue)
        {
            request.Append(name, elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(string name, Constant elementValue)
        {
            request.Append(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(Name name, Constant elementValue)
        {
            request.Append(name, elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(string name, Datetime elementValue)
        {
            request.Append(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(Name name, Datetime elementValue)
        {
            request.Append(name, elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(string name, float elementValue)
        {
            request.Append(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(Name name, float elementValue)
        {
            request.Append(name, elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
        /// <exception cref="System.NotImplementedException"></exception>
		public void Append(string name, double elementValue)
        {
            request.Append(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(Name name, double elementValue)
        {
            request.Append(name, elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(string name, long elementValue)
        {
            request.Append(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(Name name, long elementValue)
        {
            request.Append(name, elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(string name, int elementValue)
        {
            request.Append(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(Name name, int elementValue)
        {
            request.Append(name, elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(string name, char elementValue)
        {
            request.Append(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Append(Name name, char elementValue)
        {
            request.Append(name, elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">if set to <c>true</c> [element value].</param>
		public void Append(string name, bool elementValue)
        {
            request.Append(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">if set to <c>true</c> [element value].</param>
		public void Append(Name name, bool elementValue)
        {
            request.Append(name, elementValue);
        }
        /// <summary>
        /// Gets as element.
        /// </summary>
        /// <value>As element.</value>
		public Element AsElement
        {
            get
            {
                return request.AsElement;
            }
        }
        /// <summary>
        /// Gets the elements.
        /// </summary>
        /// <value>The elements.</value>
		public IEnumerable<Element> Elements
        {
            get
            {
                return request.Elements;
            }
        }
        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Element.</returns>
		public Element GetElement(string name)
        {
            return request.GetElement(Name.GetName(name));
        }
        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Element.</returns>
		public Element GetElement(Name name)
        {
            return request.GetElement(name);
        }
        /// <summary>
        /// Determines whether the specified name has element.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if the specified name has element; otherwise, <c>false</c>.</returns>
		public bool HasElement(string name)
        {
            return request.HasElement(Name.GetName(name));
        }
        /// <summary>
        /// Determines whether the specified name has element.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if the specified name has element; otherwise, <c>false</c>.</returns>
		public bool HasElement(Name name)
        {
            return request.HasElement(name);
        }
        /// <summary>
        /// Gets the operation.
        /// </summary>
        /// <value>The operation.</value>
		public Operation Operation
        {
            get
            {
                return request.Operation;
            }
        }

        public string RequestId => throw new NotImplementedException();

        /// <summary>
        /// Prints the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Print(System.IO.TextWriter writer)
        {
            request.Print(writer);
        }
        /// <summary>
        /// Prints the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
		public void Print(System.IO.Stream output)
        {
            request.Print(output);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(string name, byte[] elementValue)
        {
            request.Set(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(Name name, byte[] elementValue)
        {
            request.Set(name, elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(string name, string elementValue)
        {
            request.Set(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(Name name, string elementValue)
        {
            request.Set(name, elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(string name, Name elementValue)
        {
            request.Set(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(Name name, Name elementValue)
        {
            request.Set(name, elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(string name, Constant elementValue)
        {
            request.Set(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(Name name, Constant elementValue)
        {
            request.Set(name, elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(string name, Datetime elementValue)
        {
            request.Set(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(Name name, Datetime elementValue)
        {
            request.Set(name, elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(string name, float elementValue)
        {
            request.Set(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(Name name, float elementValue)
        {
            request.Set(name, elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(string name, double elementValue)
        {
            request.Set(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(Name name, double elementValue)
        {
            request.Set(name, elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(string name, long elementValue)
        {
            request.Set(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(Name name, long elementValue)
        {
            request.Set(name, elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(string name, int elementValue)
        {
            request.Set(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
        /// <exception cref="System.NotImplementedException"></exception>
		public void Set(Name name, int elementValue)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(string name, char elementValue)
        {
            request.Set(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">The element value.</param>
		public void Set(Name name, char elementValue)
        {
            request.Set(name, elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">if set to <c>true</c> [element value].</param>
		public void Set(string name, bool elementValue)
        {
            request.Set(Name.GetName(name), elementValue);
        }
        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="elementValue">if set to <c>true</c> [element value].</param>
		public void Set(Name name, bool elementValue)
        {
            request.Set(name, elementValue);
        }
        /// <summary>
        /// Gets the <see cref="Element"></see> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Element.</returns>
		public Element this[string name]
        {
            get
            {
                return request[name];
            }
        }
        /// <summary>
        /// Gets the <see cref="Element"></see> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Element.</returns>
		public Element this[Name name]
        {
            get
            {
                return request[name];
            }
        }

        /// <summary>
        /// Sets the elements.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public virtual bool SetElements(string classtype)
        {
            //This is not a good way of doing this. Need to get the correct name space
            Type type = Type.GetType("Bloomberg.Library.Requests." + classtype);

            MethodInfo[] methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (MethodInfo info in methods)
            {
                if (info.GetCustomAttributes(typeof(SetterAttribute), true).Length > 0)
                {
                    info.Invoke(this, null);
                }
            }
            return true;
        }
    }
}
