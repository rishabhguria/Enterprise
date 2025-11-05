// ***********************************************************************
// Assembly         : Bloomberg.Library
// Author           : MJCarlucci
// Created          : 06-14-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 06-14-2013
// ***********************************************************************
// <copyright file="SecurityType.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Enum SecurityType
    /// </summary>
    public enum SecurityType
    {
        /// <summary>
        /// Government
        /// </summary>
        [ElementValue("Govt")]
        [KeyValue("Undefined")]
        Govt,
        /// <summary>
        /// Corporate
        /// </summary>
        [ElementValue("Corp")]
        [KeyValue("Undefined")]
        Corp,
        /// <summary>
        /// Mortgage
        /// </summary>
        [ElementValue("Mtge")]
        [KeyValue("Undefined")]
        Mtge,
        /// <summary>
        /// The M MKT
        /// </summary>
        [ElementValue("M-Mkt")]
        [KeyValue("Undefined")]
        MMkt,
        /// <summary>
        /// Municipal
        /// </summary>
        [ElementValue("Muni")]
        [KeyValue("Undefined")]
        Muni,
        /// <summary>
        /// Preferred
        /// </summary>
        [ElementValue("Pfd")]
        [KeyValue("Undefined")]
        Pfd,
        /// <summary>
        /// Equity
        /// </summary>
        [ElementValue("Equity")]
        [KeyValue("Equity")]
        Equity,
        /// <summary>
        /// Commodity
        /// </summary>
        [ElementValue("Cmdty")]
        [KeyValue("Future")]
        Cmdty,
        /// <summary>
        /// Index
        /// </summary>
        [ElementValue("Index")]
        [KeyValue("Indice")]
        Index,
        /// <summary>
        /// Currency
        /// </summary>
        [ElementValue("Curncy")]
        [KeyValue("FX")]
        Curncy
    }
}
