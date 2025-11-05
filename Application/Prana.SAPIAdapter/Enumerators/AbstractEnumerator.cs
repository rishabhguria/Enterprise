// ***********************************************************************
// Assembly         : Bloomberg.Wrapper
// Author           : MJCarlucci
// Created          : 06-15-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 06-15-2013
// ***********************************************************************
// <copyright file="AbstractElement.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberglp.Blpapi;
using System.Collections.Generic;

namespace Bloomberg.Library
{

    public class DataElement
    {

    }
    /// <summary>
    /// Class AbstractElement
    /// </summary>
    public class AbstractEnumerator : List<SecurityInfo>, IEnumerable<Element>
    {
        /// <summary>
        /// The element
        /// </summary>
        internal readonly Element element = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractEnumerator" /> class.
        /// </summary>
        /// <param name="securities">The securities.</param>
        public AbstractEnumerator(Element securities)
        {
            this.element = securities;

        }
        /// <summary>
        /// Gets the <see cref="System.String" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>System.String.</returns>
        public new object this[int index]
        {
            get
            {
                return element[index];
            }
        }
        /// <summary>
        /// Gets the <see cref="System.String" /> at the specified index.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>System.String.</returns>
        public virtual object this[Name name]
        {
            get
            {
                return element[name];
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>System.Object.</returns>
        public virtual object this[string name]
        {
            get
            {
                return element[new Name(name)];
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Object" /> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="index">The index.</param>
        /// <returns>System.Object.</returns>
        public virtual object this[Name name, int index]
        {
            get
            {
                return element[name, index];
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="index">The index.</param>
        /// <returns>System.Object.</returns>
        public virtual object this[string name, int index]
        {
            get
            {
                return element[new Name(name), index];
            }
        }
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public new int Count
        {
            get
            {
                return element.NumElements;
            }
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public new IEnumerator<Element> GetEnumerator()
        {
            return element.Elements.GetEnumerator();
        }
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return element.Elements.GetEnumerator();
        }
    }
}
