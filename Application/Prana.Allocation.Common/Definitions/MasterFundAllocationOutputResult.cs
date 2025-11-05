// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : Disha Sharma
// Created          : 05-04-2017
// ***********************************************************************
// <copyright file="MasterFundAllocationOutputResult.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;

namespace Prana.Allocation.Common.Definitions
{
    /// <summary>
    ///  It contains the dictionary of MasterFundAllocationOutput for given symbol and Error Message
    /// </summary>
    public class MasterFundAllocationOutputResult
    {
        #region Members

        /// <summary>
        /// The error message
        /// </summary>
        private string _errorMessage = string.Empty;
        /// <summary>
        /// The output collection
        /// </summary>
        private Dictionary<string, MasterFundAllocationOutput> _outputCollection = new Dictionary<string, MasterFundAllocationOutput>();

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        /// <summary>
        /// Gets or sets the output collection.
        /// </summary>
        /// <value>
        /// The output collection.
        /// </value>
        public Dictionary<string, MasterFundAllocationOutput> OutputCollection
        {
            get { return _outputCollection; }
            set { _outputCollection = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterFundAllocationOutputResult"/> class.
        /// </summary>
        public MasterFundAllocationOutputResult()
        {
        }

        #endregion Constructors
    }
}
