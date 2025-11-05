// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : dewashish
// Created          : 07-24-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-01-2014
// ***********************************************************************
// <copyright file="IAllocationGenerator.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Common.Definitions;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using System.Collections.Generic;

/// <summary>
/// The Interfaces namespace.
/// </summary>
namespace Prana.Allocation.Common.Interfaces
{
    /// <summary>
    /// This construct defines the basic AllocationGenrator
    /// All the classes which will provide service of allocation based on AllocationBaseType must implement this
    /// </summary>
    public interface IAllocationGenerator
    {
        /// <summary>
        /// This returns the AllocationBaseType for current implementation
        /// </summary>
        /// <value>The type of the base.</value>
        AllocationBaseType BaseType { get; }

        /// <summary>
        /// This will generate output for the given list of allocation group based on preference
        /// </summary>
        /// <param name="list">List of allocation group which will be allocated</param>
        /// <param name="parameter">Allocation will follow the given parameter</param>
        /// <returns>AllocationOutoputResult object which will contain allocation output per allocation group</returns>
        AllocationOutputResult Generate(List<AllocationGroup> list, AllocationParameter parameter);
    }
}
