// ***********************************************************************
// Assembly         : Bloomberg.Wrapper
// Author           : MJCarlucci
// Created          : 06-12-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 06-12-2013
// ***********************************************************************
// <copyright file="ElementTree.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberglp.Blpapi;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class ElementTree
    /// </summary>
    [Serializable]
    public class ElementTree : LinkedList<Element>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementTree" /> class.
        /// </summary>
        public ElementTree()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementTree" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public ElementTree(IEnumerable<Element> collection)
            : base(collection)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementTree" /> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected ElementTree(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }

        /// <summary>
        /// Displays the tree.
        /// </summary>
        /// <param name="node">The node.</param>
        public void DisplayTree(LinkedListNode<Element> node)
        {
            if (node == null)
                return;
            //DisplayTree(node.Previous);
            Logger.LoggerWrite(node.Value.ToString() + " ");
            DisplayTree(node.Next);
        }
    }
}
