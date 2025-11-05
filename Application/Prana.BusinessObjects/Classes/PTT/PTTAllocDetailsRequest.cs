using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.PTT;
using Prana.LogManager;
using System;
using System.Data;
using System.Text;
namespace Prana.BusinessObjects
{
    /// <summary>
    /// The PTT request object model for view allocation details window
    /// </summary>
    public class PTTAllocDetailsRequest : BindableBase
    {

        private string _symbol;
        private string _target;
        private string _addOrSet;
        private string _masterFundOrAccount;
        private string _selectedFeedPrice;
        private string _type;
        private bool _isCombined;
        private bool _isTradeBreak;
        private bool _isRoundLot;

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        public string Symbol
        {
            get { return _symbol; }
            set { SetProperty(ref _symbol, value); }
        }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public string Target
        {
            get { return _target; }
            set { SetProperty(ref _target, value); }
        }

        /// <summary>
        /// Gets or sets the of or to.
        /// </summary>
        /// <value>
        /// The of or to.
        /// </value>
        public string AddOrSet
        {
            get { return _addOrSet; }
            set { SetProperty(ref _addOrSet, value); }
        }

        /// <summary>
        /// Gets or sets the calculation value.
        /// </summary>
        /// <value>
        /// The calculation value.
        /// </value>
        public string MasterFundOrAccount
        {
            get { return _masterFundOrAccount; }
            set { SetProperty(ref _masterFundOrAccount, value); }
        }

        /// <summary>
        /// Gets or sets the selected feed price.
        /// </summary>
        /// <value>
        /// The selected feed price.
        /// </value>
        public string SelectedFeedPrice
        {
            get { return _selectedFeedPrice; }
            set { SetProperty(ref _selectedFeedPrice, value); }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        public bool IsCombined
        {
            get { return _isCombined; }
            set { SetProperty(ref _isCombined, value); }
        }
        public bool IsTradeBreak
        {
            get { return _isTradeBreak; }
            set { SetProperty(ref _isTradeBreak, value); }
        }
        public bool IsRoundLot
        {
            get { return _isRoundLot; }
            set { SetProperty(ref _isRoundLot, value); }
        }

        /// <summary>
        /// Extracts the request object from data table.
        /// </summary>
        /// <param name="requestDataTable">The request data table.</param>
        public void ExtractRequestObjectFromDataTable(DataTable requestDataTable, string NewSymbol, StringBuilder errorMessage)
        {
            try
            {
                if (requestDataTable == null || (requestDataTable != null && requestDataTable.Rows.Count == 0))
                {
                    errorMessage.Append(PTTBusinessConstants.MSG_ALLOCDETAILSNOTAVAIL);
                }
                else
                {
                    DataRow requestRow = requestDataTable.Rows[0];

                    if (requestRow[PTTBusinessConstants.COL_SYMBOL] != DBNull.Value)
                    {
                        if (NewSymbol.Equals(requestRow[PTTBusinessConstants.COL_SYMBOL].ToString()))
                            Symbol = requestRow[PTTBusinessConstants.COL_SYMBOL].ToString();
                        else
                            Symbol = NewSymbol;
                    }
                    if (requestRow[PTTBusinessConstants.COL_TARGET] != DBNull.Value)
                    {

                        Target = double.Parse(requestRow[PTTBusinessConstants.COL_TARGET].ToString()).ToString();
                    }
                    if (requestRow[PTTBusinessConstants.COL_ADD_SET] != DBNull.Value)
                    {
                        var memInfo = typeof(PTTChangeType).GetMember(((PTTChangeType)Enum.Parse(typeof(PTTChangeType), requestRow[PTTBusinessConstants.COL_ADD_SET].ToString())).ToString());
                        var attributes = memInfo[0].GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
                        AddOrSet = ((EnumDescriptionAttribute)attributes[0]).Description;
                    }
                    if (requestRow[PTTBusinessConstants.COL_TYPE] != DBNull.Value)
                    {
                        var memInfo = typeof(PTTType).GetMember(((PTTType)Enum.Parse(typeof(PTTType), requestRow[PTTBusinessConstants.COL_TYPE].ToString())).ToString());
                        var attributes = memInfo[0].GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
                        Type = ((EnumDescriptionAttribute)attributes[0]).Description;
                    }
                    if (requestRow[PTTBusinessConstants.COL_MASTERFUNDORACCOUNT] != DBNull.Value)
                    {
                        var memInfo = typeof(PTTMasterFundOrAccount).GetMember(((PTTMasterFundOrAccount)Enum.Parse(typeof(PTTMasterFundOrAccount), requestRow[PTTBusinessConstants.COL_MASTERFUNDORACCOUNT].ToString())).ToString());
                        if (memInfo.Length > 0)
                        {
                            var attributes = memInfo[0].GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
                            MasterFundOrAccount = ((EnumDescriptionAttribute)attributes[0]).Description;
                        }
                    }
                    if (requestRow[PTTBusinessConstants.COL_PRICE] != DBNull.Value)
                    {
                        SelectedFeedPrice = double.Parse(requestRow[PTTBusinessConstants.COL_PRICE].ToString()).ToString();
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

        /// <summary>
        /// Extracts the request object from data table in case of PST.
        /// </summary>
        /// <param name="requestDataTable"></param>
        /// <param name="NewSymbol"></param>
        /// <param name="errorMessage"></param>
        public void PSTExtractRequestObjectFromDataTable(DataTable requestDataTable, string NewSymbol, StringBuilder errorMessage)
        {
            try
            {
                if (requestDataTable == null || (requestDataTable != null && requestDataTable.Rows.Count == 0))
                {
                    errorMessage.Append(PTTBusinessConstants.MSG_ALLOCDETAILSNOTAVAIL);
                }
                else
                {
                    DataRow requestRow = requestDataTable.Rows[0];

                    if (requestRow[PTTBusinessConstants.COL_SYMBOL] != DBNull.Value)
                    {
                        if (NewSymbol.Equals(requestRow[PTTBusinessConstants.COL_SYMBOL].ToString()))
                            Symbol = requestRow[PTTBusinessConstants.COL_SYMBOL].ToString();
                        else
                            Symbol = NewSymbol;
                    }
                    if (requestRow[PTTBusinessConstants.COL_TARGET] != DBNull.Value)
                    {

                        Target = double.Parse(requestRow[PTTBusinessConstants.COL_TARGET].ToString()).ToString();
                    }
                    if (requestRow[PTTBusinessConstants.COL_ADD_SET] != DBNull.Value)
                    {
                        var memInfo = typeof(PSTChangeType).GetMember(((PSTChangeType)Enum.Parse(typeof(PSTChangeType), requestRow[PTTBusinessConstants.COL_ADD_SET].ToString())).ToString());
                        var attributes = memInfo[0].GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
                        AddOrSet = ((EnumDescriptionAttribute)attributes[0]).Description;
                    }
                    if (requestRow[PTTBusinessConstants.COL_TYPE] != DBNull.Value)
                    {
                        var memInfo = typeof(PSTType).GetMember(((PSTType)Enum.Parse(typeof(PSTType), requestRow[PTTBusinessConstants.COL_TYPE].ToString())).ToString());
                        var attributes = memInfo[0].GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
                        Type = ((EnumDescriptionAttribute)attributes[0]).Description;
                    }
                    if (requestRow[PTTBusinessConstants.COL_MASTERFUNDORACCOUNT] != DBNull.Value)
                    {
                        var memInfo = typeof(PSTMasterFundOrAccount).GetMember(((PSTMasterFundOrAccount)Enum.Parse(typeof(PSTMasterFundOrAccount), requestRow[PTTBusinessConstants.COL_MASTERFUNDORACCOUNT].ToString())).ToString());
                        var attributes = memInfo[0].GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
                        MasterFundOrAccount = ((EnumDescriptionAttribute)attributes[0]).Description;
                    }
                    if (requestRow[PTTBusinessConstants.COL_PRICE] != DBNull.Value)
                    {
                        SelectedFeedPrice = double.Parse(requestRow[PTTBusinessConstants.COL_PRICE].ToString()).ToString();
                    }
                    if (requestRow[PTTBusinessConstants.COL_IS_COMBINED] != DBNull.Value)
                    {
                        IsCombined =Convert.ToBoolean(requestRow[PTTBusinessConstants.COL_IS_COMBINED]);
                    }
                    if (requestRow[PTTBusinessConstants.COL_IS_TRADE_BREAK] != DBNull.Value)
                    {
                        IsTradeBreak = Convert.ToBoolean(requestRow[PTTBusinessConstants.COL_IS_TRADE_BREAK]);
                    }
                    if (requestRow[PTTBusinessConstants.COL_IS_ROUNDLOT] != DBNull.Value)
                    {
                        IsRoundLot = Convert.ToBoolean(requestRow[PTTBusinessConstants.COL_IS_ROUNDLOT]);
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
