using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BusinessObjects
{
    public class ClosingTemplate : IDisposable
    {

        #region Closing Date Range
        /// <summary>
        /// The From Closing Date
        /// </summary>
        private DateTime _fromDate = DateTime.UtcNow;

        public DateTime FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; }
        }


        /// <summary>
        /// The To Closing Date
        /// </summary>
        private DateTime _toDate = DateTime.UtcNow;

        public DateTime ToDate
        {
            get { return _toDate; }
            set { _toDate = value; }
        }



        private string _templateName;

        public string TemplateName
        {
            get { return _templateName; }
            set { _templateName = value; }
        }


        private bool _useCurrentDate;

        public bool UseCurrentDate
        {
            get { return _useCurrentDate; }
            set { _useCurrentDate = value; }
        }



        #endregion

        #region StandardFilters
        /// <summary>
        /// The Selected Accounts filtered in for fetching data for closing...
        /// </summary>
        private List<int> _listAccountFilters = new List<int>();

        public List<int> ListAccountFliters
        {
            get { return _listAccountFilters; }
            set { _listAccountFilters = value; }
        }

        private ClosingType _closingType;
        public ClosingType ClosingType
        {
            get { return _closingType; }
            set { _closingType = value; }
        }

        /// <summary>
        /// The selected MasterFunds filtered in for fetching data for closing..
        /// </summary>

        private List<int> _listMasterFundFilters = new List<int>();

        public List<int> ListMasterFundFilters
        {
            get { return _listMasterFundFilters; }
            set { _listMasterFundFilters = value; }
        }


        /// <summary>
        /// The selected Assets filtered in for fetching data for closing..
        /// </summary>

        private List<int> _listAssetFilters = new List<int>();

        public List<int> ListAssetFilters
        {
            get { return _listAssetFilters; }
            set { _listAssetFilters = value; }
        }


        #endregion

        #region Custom Filters


        /// <summary>
        /// Custom filter Conditions defined by user like for symbol, asset,account etc...
        /// </summary>

        private SerializableDictionary<string, List<CustomCondition>> _dictCustomConditions = new SerializableDictionary<string, List<CustomCondition>>();

        public SerializableDictionary<string, List<CustomCondition>> DictCustomConditions
        {
            get { return _dictCustomConditions; }
            set { _dictCustomConditions = value; }
        }


        #endregion

        #region Closing Methods

        private ClosingMethodology _closingMeth = new ClosingMethodology();

        public ClosingMethodology ClosingMeth
        {
            get { return _closingMeth; }
            set { _closingMeth = value; }
        }

        #endregion





        #region Functions

        public string GetCommaSeparatedAccounts(Dictionary<int, List<int>> dictMasterFundSubAccountAssociation)
        {
            List<int> listAccountFilters = new List<int>();
            //listAccountFilters.AddRange(_listAccountFilters);

            StringBuilder commaSeparatedAccountIDs = new StringBuilder();
            try
            {

                if (_listMasterFundFilters.Count != 0)
                {

                    foreach (int materAccountID in _listMasterFundFilters)
                    {
                        if (dictMasterFundSubAccountAssociation.ContainsKey(materAccountID))
                        {
                            List<int> listSubAccountIDs = dictMasterFundSubAccountAssociation[materAccountID];

                            foreach (int subAccountID in listSubAccountIDs)
                            {
                                if (_listAccountFilters.Contains(subAccountID))
                                {
                                    listAccountFilters.Add(subAccountID);
                                }
                            }
                        }
                    }
                }
                else  //count is zero so fetch data for all master funds filtered by individual account filter if any..
                {
                    listAccountFilters.AddRange(_listAccountFilters);
                }

                foreach (int accountID in listAccountFilters)
                {
                    commaSeparatedAccountIDs.Append(accountID);
                    commaSeparatedAccountIDs.Append(',');
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
            return commaSeparatedAccountIDs.ToString();
        }

        public string GetCommaSeparatedAssets()
        {
            StringBuilder commaSeparatedAssetIDs = new StringBuilder();
            try
            {

                foreach (int assetID in _listAssetFilters)
                {
                    commaSeparatedAssetIDs.Append(assetID);
                    commaSeparatedAssetIDs.Append(',');
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

            return commaSeparatedAssetIDs.ToString();


        }


        public string GetCommaSeparatedSymbols()
        {
            StringBuilder commaSeparatedSymbols = new StringBuilder();
            try
            {
                if (_dictCustomConditions.ContainsKey("Symbol"))
                {
                    List<CustomCondition> listConditions = _dictCustomConditions["Symbol"];
                    foreach (CustomCondition condition in listConditions)
                    {
                        if (condition.ConditionOperatorType == EnumDescriptionAttribute.ConditionOperator.Equals)
                        {
                            if (!string.IsNullOrEmpty(condition.CompareValue))
                            {
                                commaSeparatedSymbols.Append(condition.compareValue);
                                commaSeparatedSymbols.Append(',');
                            }
                        }
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

            return commaSeparatedSymbols.ToString();
        }




        #endregion


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (ClosingMeth != null)
                        ClosingMeth.Dispose();
                    if (_closingMeth != null)
                        _closingMeth.Dispose();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
