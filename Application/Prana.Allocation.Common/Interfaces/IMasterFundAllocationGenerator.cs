// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : Disha Sharma
// Created          : 05-04-2017
// ***********************************************************************
// <copyright file="IMasterFundAllocationGenerator.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Common.Definitions;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using System.Collections.Generic;

namespace Prana.Allocation.Common.Interfaces
{
    public interface IMasterFundAllocationGenerator
    {
        /// <summary>
        /// This returns the AllocationBaseType for current implementation
        /// </summary>
        /// <value>The type of the base.</value>
        AllocationBaseType BaseType { get; }

        /// <summary>
        /// Generates the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="masterFundPref">The master fund preference.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>MasterFundAllocationOutputResult object which will contain allocation output per symbol</returns>
        MasterFundAllocationOutputResult Generate(List<AllocationGroup> list, AllocationMasterFundPreference masterFundPref, int userId);
    }
}
