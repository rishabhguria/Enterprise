// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : dewashish
// Created          : 08-17-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-01-2014
// ***********************************************************************
// <copyright file="PreferenceUpdateResult.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;

/// <summary>
/// The Definitions namespace.
/// </summary>
namespace Prana.BusinessObjects.Classes.Allocation
{
    /// <summary>
    /// Whenever any update or modification comes this will contain the response of the operation
    /// </summary>
    [Serializable]
    public class PreferenceUpdateResult
    {

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>The error.</value>
        public String Error { get; set; }


        /// <summary>
        /// Gets or sets the preference.
        /// </summary>
        /// <value>The preference.</value>
        public AllocationOperationPreference Preference { get; set; }

        /// <summary>
        /// Gets or sets the master fund preference.
        /// </summary>
        /// <value>
        /// The master fund preference.
        /// </value>
        public AllocationMasterFundPreference MasterFundPreference { get; set; }

        /// <summary>
        /// Gets or sets the master fund calculated preferences.
        /// </summary>
        /// <value>
        /// The master fund calculated preferences.
        /// </value>
        public List<AllocationOperationPreference> MasterFundCalculatedPreferences { get; set; }
    }
}
