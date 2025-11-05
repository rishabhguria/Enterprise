using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ClosingMethodology : IDisposable
    {

        private DataSet _accountingMethodsTable = new DataSet();
        public DataSet AccountingMethodsTable
        {
            set
            {
                _accountingMethodsTable = value;
                UpdateAccountingMethodsDictionary();
            }
            get { return _accountingMethodsTable; }
        }

        private bool _overrideGlobal = false;
        public bool OverrideGlobal
        {
            get { return _overrideGlobal; }
            set { _overrideGlobal = value; }
        }



        /// <summary>
        /// The globalClosing type like Automatic or Manual...
        /// </summary>

        private int _globalClosingMethodology = (int)PostTradeEnums.CloseTradeMethodology.Automatic;
        public int GlobalClosingMethodology
        {
            get { return _globalClosingMethodology; }
            set { _globalClosingMethodology = value; }
        }



        /// <summary>
        /// The Closing Algo selected like FIFO, HIFO etc..
        /// </summary>
        private PostTradeEnums.CloseTradeAlogrithm _closingAlgo;

        public PostTradeEnums.CloseTradeAlogrithm ClosingAlgo
        {
            get { return _closingAlgo; }
            set { _closingAlgo = value; }
        }


        /// <summary>
        /// The Secondary Sort Criteria like AvgPxAscending etc.
        /// Its only applicable when the primary criteria for sorting of taxlots is same...
        /// </summary>
        private PostTradeEnums.SecondarySortCriteria _secondarySort = PostTradeEnums.SecondarySortCriteria.None;

        public PostTradeEnums.SecondarySortCriteria SecondarySort
        {
            get { return _secondarySort; }
            set { _secondarySort = value; }
        }


        private PostTradeEnums.ClosingField _closingField;
        public PostTradeEnums.ClosingField ClosingField
        {
            get { return _closingField; }
            set { _closingField = value; }
        }

        /// <summary>
        /// The Average Cost Accounting..(Positions are closed on prorata basis rather than considering sorting at taxlot level)...
        /// </summary>
        //private bool _isACA;

        //public bool IsACA
        //{
        //    get { return _isACA; }
        //    set { _isACA = value; }
        //}

        /// <summary>
        /// IsSellShort Allowed to close with Buy/BuyToClose...This is force condition to close wrong sides..
        /// This property is at global level and not AssetSpecific...
        /// </summary>

        private bool _isShortWithBuyandBTC = false;
        public bool IsShortWithBuyandBTC
        {
            get { return _isShortWithBuyandBTC; }
            set { _isShortWithBuyandBTC = value; }
        }



        /// <summary>
        /// IsSell Allowed to close with BuyToClose...This is force condition to close wrong sides..
        /// This property is at global level and not AssetSpecific...
        /// </summary>
        private bool _isSellWithBTC = false;
        public bool IsSellWithBTC
        {
            get { return _isSellWithBTC; }
            set { _isSellWithBTC = value; }
        }

        //private int _globalClosingMethodology;
        //public int GlobalClosingMethodology
        //{
        //    get { return _globalClosingMethodology; }
        //    set { _globalClosingMethodology = value; }
        //}

        //private int _globalClosingAlgo;
        //public int GlobalClosingAlgo
        //{
        //    get { return _globalClosingAlgo; }
        //    set { _globalClosingAlgo = value; }
        //}



        /// <summary>
        /// LongTerm tax rate for Tax Advantage Accounting Method...
        /// </summary>
        private double _longTermTaxRate = 1;
        public double LongTermTaxRate
        {
            get { return _longTermTaxRate; }
            set { _longTermTaxRate = value; }
        }


        /// <summary>
        /// Short Term Tax rate for TaxAdvantage Accounting Method..
        /// </summary>
        private double _shortTermTaxRate = 1;
        public double ShortTermTaxRate
        {
            get { return _shortTermTaxRate; }
            set { _shortTermTaxRate = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is exercised assigment for box position.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is exercised assigment for box position; otherwise, <c>false</c>.
        /// </value>
        public bool SplitunderlyingBasedOnPosition { get; set; }

        /// <summary>
        /// This dictionary maintains mapping of Asset and Closing Methodology..
        /// Useful when user overrides any method for specific Asset..
        /// </summary>

        [XmlIgnore]
        [NonSerialized]
        private Dictionary<string, DataRow> _dictAccountingMethods = new Dictionary<string, DataRow>();


        [XmlIgnore]
        public Dictionary<string, DataRow> DictAccountingMethods
        {
            get
            {
                if (_dictAccountingMethods == null)
                {
                    UpdateAccountingMethodsDictionary();
                }
                return _dictAccountingMethods;
            }
            set { _dictAccountingMethods = value; }
        }


        private void UpdateAccountingMethodsDictionary()
        {
            try
            {
                if (_dictAccountingMethods == null)
                {
                    _dictAccountingMethods = new Dictionary<string, DataRow>();
                }
                _dictAccountingMethods.Clear();
                if (_accountingMethodsTable.Tables.Count > 0)
                {
                    foreach (DataRow row in _accountingMethodsTable.Tables[0].Rows)
                    {
                        string Key = row["FundID"].ToString() + Seperators.SEPERATOR_13 + row["AssetName"].ToString();

                        if (!string.IsNullOrEmpty(Key))
                        {
                            if (!_dictAccountingMethods.ContainsKey(Key))
                            {
                                _dictAccountingMethods.Add(Key, row);
                            }

                        }
                    }
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


        public int GetClosingAlgoForAccountAndAsset(string accountID, string AssetName)
        {
            int closingAlgo = int.MinValue;

            try
            {
                if (!OverrideGlobal)
                {
                    return (int)ClosingAlgo;
                }
                else
                {

                    string key = accountID + Seperators.SEPERATOR_13 + AssetName;

                    if (!string.IsNullOrEmpty(key))
                    {
                        if (DictAccountingMethods.ContainsKey(key))
                        {
                            DataRow dr = DictAccountingMethods[key];
                            closingAlgo = System.Convert.ToInt32(dr["ClosingAlgo"].ToString());

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
            return closingAlgo;
        }

        public PostTradeEnums.SecondarySortCriteria GetSecondarySortingCriteriaForAccountAndAsset(string accountID, string AssetName)
        {
            PostTradeEnums.SecondarySortCriteria secondarySort = PostTradeEnums.SecondarySortCriteria.None;

            try
            {
                if (!OverrideGlobal)
                {
                    return this.SecondarySort;
                }
                else
                {

                    string key = accountID + Seperators.SEPERATOR_13 + AssetName;

                    if (!string.IsNullOrEmpty(key))
                    {
                        if (DictAccountingMethods.ContainsKey(key))
                        {
                            DataRow dr = DictAccountingMethods[key];
                            secondarySort = (PostTradeEnums.SecondarySortCriteria)System.Convert.ToInt32(dr["SecondarySort"].ToString());

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
            return secondarySort;
        }

        public PostTradeEnums.ClosingField GetClosingFieldForAccountAndAsset(string accountID, string AssetName)
        {
            PostTradeEnums.ClosingField closingField = PostTradeEnums.ClosingField.Default;

            try
            {
                if (!OverrideGlobal)
                {
                    return this.ClosingField;
                }
                else
                {

                    string key = accountID + Seperators.SEPERATOR_13 + AssetName;

                    if (!string.IsNullOrEmpty(key))
                    {
                        if (DictAccountingMethods.ContainsKey(key))
                        {
                            DataRow dr = DictAccountingMethods[key];
                            if (dr.Table.Columns.Contains("ClosingField"))
                                closingField = (PostTradeEnums.ClosingField)System.Convert.ToInt32(dr["ClosingField"].ToString());

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
            return closingField;
        }


        //modified by omshiv, added prerefernce for ignore matching strategy
        public bool IsAutoCloseStrategy { get; set; }

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
                    if (_accountingMethodsTable != null)
                        _accountingMethodsTable.Dispose();
                    if (AccountingMethodsTable != null)
                        AccountingMethodsTable.Dispose();
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
