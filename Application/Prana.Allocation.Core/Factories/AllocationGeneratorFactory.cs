// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 07-24-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-01-2014
// ***********************************************************************
// <copyright file="AllocationGeneratorFactory.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Common.Interfaces;
using Prana.Allocation.Core.Generator;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;

/// <summary>
/// The Factories namespace.
/// </summary>
namespace Prana.Allocation.Core.Factories
{
    /// <summary>
    /// Implementation for AllocationGeneraorFactory which will create/generate the required IAllocationGenerator
    /// </summary>
    internal class AllocationGeneratorFactory : IAllocationGeneratorFactory
    {

        /// <summary>
        /// Public constructor to initialize this factory
        /// This factory hold the already created instance of Generator classes
        /// </summary>
        internal AllocationGeneratorFactory()
        {
            try
            {
                // Do initialization for AllocationGeneratorFactory
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


        #region IAllocationGeneratorFactory Members

        /// <summary>
        /// This method returns the generator for given base type
        /// </summary>
        /// <param name="baseType">AllocationBaseType, for which generator is needed</param>
        /// <returns>Returns generator for given base type</returns>
        public IAllocationGenerator GetGenerator(AllocationBaseType baseType)
        {
            try
            {
                // TODO: Need to check performance
                // This method creates a new instance each time. 
                // There is no data associated with generator classes so there should not be any performance hit because of this

                switch (baseType)
                {
                    case AllocationBaseType.CumQuantity:
                        return new CumQuantityGenerator();
                    case AllocationBaseType.Notional:
                        return new NotionalAllocationGenerator();
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

        /// <summary>
        /// Gets the master fund allocation generator.
        /// </summary>
        /// <param name="baseType">Type of the base.</param>
        /// <returns></returns>
        public IMasterFundAllocationGenerator GetMasterFundAllocationGenerator(AllocationBaseType baseType)
        {
            try
            {
                switch (baseType)
                {
                    case AllocationBaseType.CumQuantity:
                        return new MFCumQuantityAllocationGenerator();
                    case AllocationBaseType.Notional:
                        return new MFNotionalAllocationGenerator();
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        #endregion
    }
}
