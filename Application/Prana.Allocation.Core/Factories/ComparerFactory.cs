// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 07-24-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-01-2014
// ***********************************************************************
// <copyright file="ComparerFactory.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Core.Comparers;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;
using System.Collections.Generic;

/// <summary>
/// The Factories namespace.
/// </summary>
namespace Prana.Allocation.Core.Factories
{
    /// <summary>
    /// IComparerFactory Instance
    /// </summary>
    internal class ComparerFactory
    {

        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        private static readonly ComparerFactory _singletonInstance = new ComparerFactory();

        /// <summary>
        /// Instance method to return the singleton instance of the object in the memory
        /// </summary>
        /// <value>The instance.</value>
        internal static ComparerFactory Instance
        {
            get
            {
                return _singletonInstance;
            }
        }

        /// <summary>
        /// Private constructor to restrict object creation
        /// </summary>
        private ComparerFactory()
        {
            try
            {
                // Do initialization for ComparerFactory
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Gets the comparer for.
        /// </summary>
        /// <param name="baseType">Type of the base.</param>
        /// <param name="isAscending">Sorting order</param>
        /// <returns>IComparer&lt;AllocationGroup&gt;.</returns>
        public IComparer<AllocationGroup> GetComparerFor(AllocationBaseType baseType, bool isAscending)
        {
            try
            {
                switch (baseType)
                {
                    case AllocationBaseType.CumQuantity:
                        return new CumQtyComparer(isAscending);
                    case AllocationBaseType.Notional:
                        return new NotionalComparer(isAscending);
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

    }
}
