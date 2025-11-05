// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : Disha Sharma
// Created          : 05-04-2017
// ***********************************************************************
// <copyright file="MasterFundAllocationOutput.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using System.Collections.Generic;

namespace Prana.Allocation.Common.Definitions
{
    /// <summary>
    /// This definition holds output data for each symbol
    /// </summary>
    public class MasterFundAllocationOutput
    {
        #region Members

        /// <summary>
        /// The error
        /// </summary>
        private string _errorMessage = string.Empty;

        /// <summary>
        /// The master fund quantity
        /// </summary>
        Dictionary<int, decimal> _masterFundQuantity = new Dictionary<int, decimal>();

        /// <summary>
        /// The order side wise virtual groups
        /// </summary>
        private SerializableDictionary<int, List<AllocationGroup>> _orderSideWiseVirtualGroups = new SerializableDictionary<int, List<AllocationGroup>>();

        /// <summary>
        /// The symbol wise group list
        /// </summary>
        private List<AllocationGroup> _symbolWiseGroupList = new List<AllocationGroup>();

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
        /// Gets or sets the master fund quantity.
        /// </summary>
        /// <value>
        /// The master fund quantity.
        /// </value>
        public Dictionary<int, decimal> MasterFundQuantity
        {
            get { return _masterFundQuantity; }
            set { _masterFundQuantity = value; }
        }

        /// <summary>
        /// Gets or sets the order side wise virtual groups.
        /// </summary>
        /// <value>
        /// The order side wise virtual groups.
        /// </value>
        public SerializableDictionary<int, List<AllocationGroup>> OrderSideWiseVirtualGroups
        {
            get { return _orderSideWiseVirtualGroups; }
            set { _orderSideWiseVirtualGroups = value; }
        }

        /// <summary>
        /// Gets or sets the group identifier.
        /// </summary>
        /// <value>
        /// The group identifier.
        /// </value>
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the total cum qty.
        /// </summary>
        /// <value>
        /// The total cum qty.
        /// </value>
        public decimal TotalCumQty { get; set; }

        /// <summary>
        /// Gets or sets the symbol wise group list.
        /// </summary>
        /// <value>
        /// The symbol wise group list.
        /// </value>
        public List<AllocationGroup> SymbolWiseGroupList
        {
            get { return _symbolWiseGroupList; }
            set { _symbolWiseGroupList = value; }
        }

        /// <summary>
        /// Gets or sets the match closing transaction.
        /// </summary>
        /// <value>
        /// The match closing transaction.
        /// </value>
        public MatchClosingTransactionType MatchClosingTransaction { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterFundAllocationOutput"/> class.
        /// </summary>
        public MasterFundAllocationOutput()
        {
            this.TotalCumQty = 0.0M;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterFundAllocationOutput"/> class.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public MasterFundAllocationOutput(string symbol)
            : this()
        {
            this.Symbol = symbol;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterFundAllocationOutput"/> class.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="totalCumQty">The total cum qty.</param>
        /// <param name="averagePrice">The average price.</param>
        /// <param name="masterFundQuantity">The master fund quantity.</param>
        /// <param name="orderSideWiseGroups">The order side wise groups.</param>
        /// <param name="symbolWiseGroupList">The symbol wise group list.</param>
        public MasterFundAllocationOutput(string symbol, decimal totalCumQty, Dictionary<int, decimal> masterFundQuantity, SerializableDictionary<int, List<AllocationGroup>> orderSideWiseGroups, List<AllocationGroup> symbolWiseGroupList, MatchClosingTransactionType matchClosingTransaction)
        {
            this.Symbol = symbol;
            this.TotalCumQty = totalCumQty;
            this.MasterFundQuantity = masterFundQuantity;
            this.OrderSideWiseVirtualGroups = orderSideWiseGroups;
            this.SymbolWiseGroupList = symbolWiseGroupList;
            this.MatchClosingTransaction = matchClosingTransaction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterFundAllocationOutput"/> class.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="errorMessage">The error message.</param>
        public MasterFundAllocationOutput(string symbol, string errorMessage, MatchClosingTransactionType matchClosingTransaction, List<AllocationGroup> symbolWiseGroupList)
        {
            this.Symbol = symbol;
            this.ErrorMessage = errorMessage;
            this.MatchClosingTransaction = matchClosingTransaction;
            this.SymbolWiseGroupList = symbolWiseGroupList;
        }

        #endregion Constructors
    }
}
