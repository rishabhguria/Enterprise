// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : dewashish
// Created          : 07-24-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-01-2014
// ***********************************************************************
// <copyright file="CustomOperator.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

/// <summary>
/// The Enums namespace.
/// </summary>
namespace Prana.BusinessObjects.Classes.Allocation
{
    /// <summary>
    /// This defines the custom operator enum
    /// </summary>
    public enum CustomOperator
    {
        /// <summary>
        /// According to this all of the data whatever it is; should return true
        /// </summary>
        All = 1,

        /// <summary>
        /// If it is selected, then a list of object must be associated with this, which will contain the list of allowed data
        /// </summary>
        Include = 2,

        /// <summary>
        /// If it is selected, then a list of object must be associated with this, which will contain the list of dis-allowed data
        /// If associated list is null or empty this mean it will work same as All.
        /// </summary>
        Exclude = 3
    }
}
