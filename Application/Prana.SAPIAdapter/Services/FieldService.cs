// ***********************************************************************
// Assembly         : Bloomberg Library
// Author           : MJCarlucci
// Created          : 04-29-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 04-29-2013
// ***********************************************************************
// <copyright file="FieldService.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Bloomberg.Library
{
    /// <summary>
    /// Class FieldService
    /// </summary>
    public class FieldService : UserService
    {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public override string Url
        {
            get { return "//blp/apiflds"; }
            set {; }
        }
    }
}
