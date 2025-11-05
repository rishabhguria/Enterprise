using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// The response object with calculations
    /// </summary>
    public class PTTResponseObject : BindableBase
    {
        /// <summary>
        /// The _account identifier
        /// </summary>
        private int _accountId;
        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        /// <value>
        /// The account identifier.
        /// </value>
        public int AccountId
        {
            get { return _accountId; }
            set { SetProperty(ref _accountId, value); }
        }

        /// <summary>
        /// The _current position
        /// </summary>
        private decimal _startingPosition;
        /// <summary>
        /// Gets or sets the current position.
        /// </summary>
        /// <value>
        /// The current position.
        /// </value>
        public decimal StartingPosition
        {
            get { return _startingPosition; }
            set { SetProperty(ref _startingPosition, value); }
        }

        /// <summary>
        /// The _current account symbol value
        /// </summary>
        private decimal _startingValue;
        /// <summary>
        /// Gets or sets the current account symbol value.
        /// </summary>
        /// <value>
        /// The current account symbol value.
        /// </value>
        public decimal StartingValue
        {
            get { return _startingValue; }
            set { SetProperty(ref _startingValue, value); }
        }

        /// <summary>
        /// The _account nav
        /// </summary>
        private decimal _accountNAV;
        /// <summary>
        /// Gets or sets the account nav.
        /// </summary>
        /// <value>
        /// The account nav.
        /// </value>
        public decimal AccountNAV
        {
            get { return _accountNAV; }
            set { SetProperty(ref _accountNAV, value); }
        }

        /// <summary>
        /// The _current percentage
        /// </summary>
        private decimal _startingPercentage;
        /// <summary>
        /// Gets or sets the current percentage.
        /// </summary>
        /// <value>
        /// The current percentage.
        /// </value>
        public decimal StartingPercentage
        {
            get { return _startingPercentage; }
            set { SetProperty(ref _startingPercentage, value); }
        }

        /// <summary>
        /// The _percentage change
        /// </summary>
        private decimal _percentageType;
        /// <summary>
        /// Gets or sets the percentage change.
        /// </summary>
        /// <value>
        /// The percentage change.
        /// </value>
        public decimal PercentageType
        {
            get { return _percentageType; }
            set { SetProperty(ref _percentageType, value); }
        }

        /// <summary>
        /// The _trade quantity
        /// </summary>
        private decimal _tradeQuantity;
        /// <summary>
        /// Gets or sets the trade quantity.
        /// </summary>
        /// <value>
        /// The trade quantity.
        /// </value>
        public decimal TradeQuantity
        {
            get { return _tradeQuantity; }
            set { SetProperty(ref _tradeQuantity, value); }
        }

        /// <summary>
        /// The _roundLot quantity
        /// </summary>
        private decimal _roundLots;
        /// <summary>
        /// Gets or sets the roundLot quantity.
        /// </summary>
        /// <value>
        /// The roundLot quantity.
        /// </value>
        public decimal RoundLots
        {
            get { return _roundLots; }
            set { SetProperty(ref _roundLots, value); }
        }

        /// <summary>
        /// The _end percentage
        /// </summary>
        private decimal _endingPercentage;
        /// <summary>
        /// Gets or sets the end percentage.
        /// </summary>
        /// <value>
        /// The end percentage.
        /// </value>
        public decimal EndingPercentage
        {
            get { return _endingPercentage; }
            set { SetProperty(ref _endingPercentage, value); }
        }

        /// <summary>
        /// The _end position
        /// </summary>
        private decimal _endingPosition;
        /// <summary>
        /// Gets or sets the end position.
        /// </summary>
        /// <value>
        /// The end position.
        /// </value>
        public decimal EndingPosition
        {
            get { return _endingPosition; }
            set { SetProperty(ref _endingPosition, value); }
        }

        /// <summary>
        /// The _end account symbol value
        /// </summary>
        private decimal _endingValue;
        /// <summary>
        /// Gets or sets the end account symbol value.
        /// </summary>
        /// <value>
        /// The end account symbol value.
        /// </value>
        public decimal EndingValue
        {
            get { return _endingValue; }
            set { SetProperty(ref _endingValue, value); }
        }

        /// <summary>
        /// The _percentage allocation
        /// </summary>
        private decimal _percentageAllocation;
        /// <summary>
        /// Gets or sets the percentage allocation.
        /// </summary>
        /// <value>
        /// The percentage allocation.
        /// </value>
        public decimal PercentageAllocation
        {
            get { return _percentageAllocation; }
            set { SetProperty(ref _percentageAllocation, value); }
        }

        /// <summary>
        /// The _FX rate
        /// </summary>
        private decimal _fxRate;
        /// <summary>
        /// Gets or sets the fx rate.
        /// </summary>
        /// <value>
        /// The fx rate.
        /// </value>
        public decimal FxRate
        {
            get { return _fxRate; }
            set { SetProperty(ref _fxRate, value); }
        }

        private List<PTTResponseObject> _childResponseObjectList;

        public List<PTTResponseObject> ChildResponseObjectList
        {
            get { return _childResponseObjectList; }
            set { _childResponseObjectList = value; }
        }

        /// <summary>
        /// The _order side
        /// </summary>
        private string _orderSide;
         
        public string OrderSide
        {
            get { return _orderSide; }
            set { SetProperty(ref _orderSide, value); }
        }

        /// <summary>
        /// Extracts the response object from data row.
        /// </summary>
        /// <param name="responseDataRow">The response data row.</param>
        public void ExtractResponseObjectFromDataRow(DataRow responseDataRow)
        {
            try
            {
                if (responseDataRow != null)
                {
                    if (responseDataRow["AccountId"] != DBNull.Value)
                    {
                        AccountId = Convert.ToInt32(responseDataRow["AccountId"].ToString());
                    }
                    if (responseDataRow["StartingPosition"] != DBNull.Value)
                    {
                        StartingPosition = Convert.ToDecimal(responseDataRow["StartingPosition"].ToString());
                    }
                    if (responseDataRow["StartingValue"] != DBNull.Value)
                    {
                        StartingValue = Convert.ToDecimal(responseDataRow["StartingValue"].ToString());
                    }
                    if (responseDataRow["AccountNAV"] != DBNull.Value)
                    {
                        AccountNAV = Convert.ToDecimal(responseDataRow["AccountNAV"].ToString());
                    }
                    if (responseDataRow["StartingPercentage"] != DBNull.Value)
                    {
                        StartingPercentage = Convert.ToDecimal(responseDataRow["StartingPercentage"].ToString());
                    }
                    // Changed 'PercentageChange' to 'PercentageType' as the column name in 'T_PTTDetails' has been updated
                    if (responseDataRow["PercentageType"] != DBNull.Value)
                    {
                        PercentageType = Convert.ToDecimal(responseDataRow["PercentageType"].ToString());
                    }
                    if (responseDataRow["TradeQuantity"] != DBNull.Value)
                    {
                        TradeQuantity = Convert.ToDecimal(responseDataRow["TradeQuantity"].ToString());
                    }
                    if (responseDataRow["EndingPercentage"] != DBNull.Value)
                    {
                        EndingPercentage = Convert.ToDecimal(responseDataRow["EndingPercentage"].ToString());
                    }
                    if (responseDataRow["EndingPosition"] != DBNull.Value)
                    {
                        EndingPosition = Convert.ToDecimal(responseDataRow["EndingPosition"].ToString());
                    }
                    if (responseDataRow["EndingValue"] != DBNull.Value)
                    {
                        EndingValue = Convert.ToDecimal(responseDataRow["EndingValue"].ToString());
                    }
                    if (responseDataRow["PercentageAllocation"] != DBNull.Value)
                    {
                        PercentageAllocation = Convert.ToDecimal(responseDataRow["PercentageAllocation"].ToString());
                    }
                    if (responseDataRow["OrderSideID"] != DBNull.Value)
                    {
                        OrderSide = responseDataRow["OrderSideID"].ToString();
                    }
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
            }
        }
    }
}