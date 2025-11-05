// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : dewashish
// Created          : 07-24-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-01-2014
// ***********************************************************************
// <copyright file="IAllocationGeneratorFactory.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects.Classes.Allocation;

/// <summary>
/// The Interfaces namespace.
/// </summary>
namespace Prana.Allocation.Common.Interfaces
{
    /// <summary>
    /// This constructs provide the interface for the factory which will in turn generate IAllocationGenerator
    /// </summary>
    public interface IAllocationGeneratorFactory
    {
        /// <summary>
        /// This method will return the respective generator
        /// </summary>
        /// <param name="baseType">Type of the base.</param>
        /// <returns>IAllocationGenerator.</returns>
        IAllocationGenerator GetGenerator(AllocationBaseType baseType);

        /// <summary>
        /// Gets the master fund allocation generator.
        /// </summary>
        /// <param name="baseType">Type of the base.</param>
        /// <returns></returns>
        IMasterFundAllocationGenerator GetMasterFundAllocationGenerator(AllocationBaseType baseType);
    }
}
