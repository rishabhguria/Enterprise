using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.Allocation.Client.Controls.Preferences.ViewModels
{
    public class PreferenceAccountStrategyControlViewModel : ViewModelBase
    {
        #region Members

        /// <summary>
        /// The _account startegy table
        /// </summary>
        private DataTable _accountStartegyTable;

        /// <summary>
        /// The _account value dictionary
        /// </summary>
        private SerializableDictionary<int, AccountValue> _accountValueDictionary;

        /// <summary>
        /// The _is refresh summary
        /// </summary>
        private bool _isRefreshSummary;

        /// <summary>
        /// The _precision format
        /// </summary>
        private string _precisionFormat;

        /// <summary>
        /// The _end edit mode
        /// </summary>
        private bool _endEditMode;

        /// <summary>
        /// The _is preference account strategy enabled
        /// </summary>
        private bool _isPrefAccountStrategyEnabled = true;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the account startegy table.
        /// </summary>
        /// <value>
        /// The account startegy table.
        /// </value>
        public DataTable AccountStartegyTable
        {
            get { return _accountStartegyTable; }
            set
            {
                _accountStartegyTable = value;
                RaisePropertyChangedEvent("AccountStartegyTable");
            }
        }

        /// <summary>
        /// Gets or sets the account value dictionary.
        /// </summary>
        /// <value>
        /// The account value dictionary.
        /// </value>
        public SerializableDictionary<int, AccountValue> AccountValueDictionary
        {
            get { return _accountValueDictionary; }
            set
            {
                _accountValueDictionary = value;
                RaisePropertyChangedEvent("AccountValueDictionary");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [end edit mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [end edit mode]; otherwise, <c>false</c>.
        /// </value>
        public bool EndEditMode
        {
            get { return _endEditMode; }
            set
            {
                _endEditMode = value;
                RaisePropertyChangedEvent("EndEditMode");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is refresh summary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is refresh summary; otherwise, <c>false</c>.
        /// </value>
        public bool IsRefreshSummary
        {
            get { return _isRefreshSummary; }
            set
            {
                _isRefreshSummary = value;
                RaisePropertyChangedEvent("IsRefreshSummary");
            }
        }

        /// <summary>
        /// Gets or sets the precision format.
        /// </summary>
        /// <value>
        /// The precision format.
        /// </value>
        public string PrecisionFormat
        {
            get { return _precisionFormat; }
            set
            {
                _precisionFormat = value;
                RaisePropertyChangedEvent("PrecisionFormat");
            }
        }

        /// <summary>
        /// Gets or sets the is preference account strategy enabled.
        /// </summary>
        /// <value>
        /// The is preference account strategy enabled.
        /// </value>
        public bool IsPrefAccountStrategyEnabled
        {
            get { return _isPrefAccountStrategyEnabled; }
            set
            {
                _isPrefAccountStrategyEnabled = value;
                RaisePropertyChangedEvent("IsPrefAccountStrategyEnabled");
            }
        }

        public bool _exportAccountAndStrategyGrid;

        public bool ExportAccountAndStrategyGrid
        {
            get { return _exportAccountAndStrategyGrid; }
            set
            {
                _exportAccountAndStrategyGrid = value;
                RaisePropertyChangedEvent("ExportAccountAndStrategyGrid");
            }
        }

        private string _exportPathForAutomation;
        public string ExportPathForAutomation
        {
            get { return _exportPathForAutomation; }
            set { _exportPathForAutomation = value; RaisePropertyChangedEvent("ExportPathForAutomation"); }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PreferenceAccountStrategyControlViewModel"/> class.
        /// </summary>
        public PreferenceAccountStrategyControlViewModel()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the account strategy values.
        /// </summary>
        /// <returns></returns>
        internal SerializableDictionary<int, AccountValue> GetAccountStrategyValues()
        {
            SerializableDictionary<int, AccountValue> accountStrategyValues = new SerializableDictionary<int, AccountValue>();
            try
            {
                if (AccountValueDictionary != null)
                {
                    EndEditMode = true;
                    accountStrategyValues = DeepCopyHelper.Clone(AccountValueDictionary);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return accountStrategyValues;
        }

        /// <summary>
        /// Called when [load preference Account strategy control].
        /// </summary>
        internal void OnLoadPreferenceAccountStrategyControl(Dictionary<int, string> accountList, AllocationCompanyWisePref pref)
        {
            try
            {
                AccountValueDictionary = new SerializableDictionary<int, AccountValue>();
                AccountStartegyTable = SetUpDataTable(accountList, CachedDataManager.GetInstance.GetUserPermittedStrategies());
                PrecisionFormat = CommonAllocationMethods.SetPrecisionFormat(pref.PrecisionDigit);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets up data table.
        /// </summary>
        /// <param name="accountList">The account list.</param>
        /// <param name="strategyCollection">The strategy collection.</param>
        /// <returns></returns>
        public DataTable SetUpDataTable(Dictionary<int, string> accountList, StrategyCollection strategyCollection)
        {
            DataTable dtTemp = new DataTable();
            try
            {
                DataColumn accountId = new DataColumn();
                accountId.ColumnName = AllocationUIConstants.ACCOUNT_ID;
                accountId.DataType = typeof(int);
                dtTemp.Columns.Add(accountId);

                DataColumn account = new DataColumn();
                account.ColumnName = AllocationUIConstants.ACCOUNT;
                account.Caption = "Name";
                dtTemp.Columns.Add(account);

                DataColumn perAccount = new DataColumn();
                perAccount.ColumnName = AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE;
                perAccount.DataType = typeof(Double);
                perAccount.Caption = "%";
                perAccount.DefaultValue = 0;
                dtTemp.Columns.Add(perAccount);

                if (strategyCollection != null)
                {
                    foreach (Strategy strategy in strategyCollection)
                    {
                        if (strategy.StrategyID != int.MinValue)
                        {
                            DataColumn perColumn = new DataColumn();
                            perColumn.ColumnName = AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE;
                            perColumn.DataType = typeof(Double);
                            perColumn.Caption = "%";
                            perColumn.DefaultValue = 0;
                            dtTemp.Columns.Add(perColumn);
                        }
                    }
                }
                foreach (int id in accountList.Keys)
                {
                    if (id != -1)
                    {
                        dtTemp.Rows.Add(new object[] { id, accountList[id].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dtTemp;
        }

        /// <summary>
        /// Set the account strategy grid values
        /// </summary>
        /// <param name="allocationOperationPref"></param>
        internal void SetAccountStrategyGrid(SerializableDictionary<int, AccountValue> TargetPercentage, List<int> accountList)
        {
            try
            {
                ClearAccountStrategyTable();
                if (TargetPercentage != null)
                {
                    SerializableDictionary<int, AccountValue> targetPercentage = TargetPercentage;
                    StrategyCollection strategies = CommonDataCache.CachedDataManager.GetInstance.GetUserPermittedStrategies();
                    List<int> strategyList = (from Strategy strategy in strategies select strategy.StrategyID).ToList();
                    foreach (int accountId in accountList)
                    {
                        DataRow dr = AccountStartegyTable.Select(AllocationUIConstants.ACCOUNT_ID + " = " + accountId)[0];
                        if (targetPercentage.Keys.Contains(accountId))
                        {
                            dr[AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE] = targetPercentage[accountId].Value;
                            if (targetPercentage[accountId].StrategyValueList != null)
                            {
                                foreach (StrategyValue strategy in targetPercentage[accountId].StrategyValueList)
                                {
                                    if (!strategyList.Contains(strategy.StrategyId))
                                        continue;
                                    string strategyName = CommonDataCache.CachedDataManager.GetInstance.GetStrategyText(strategy.StrategyId);
                                    dr[AllocationUIConstants.STRATEGY_PREFIX + strategyName + AllocationUIConstants.PERCENTAGE] = strategy.Value;
                                }
                            }
                        }
                    }
                    UpdateAccountValueDictionary();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clears the Account strategy table.
        /// </summary>
        internal void ClearAccountStrategyTable()
        {
            try
            {
                StrategyCollection strategyCollection = CommonDataCache.CachedDataManager.GetInstance.GetUserPermittedStrategies();
                if (AccountStartegyTable != null)
                {
                    AccountStartegyTable.AsEnumerable().Where(x => !x[AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE].Equals(0.0)).ToList().ForEach(row =>
                    {
                        row[AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE] = 0;
                        if (strategyCollection != null)
                        {
                            strategyCollection.Cast<Strategy>().Where(strategy => strategy.StrategyID != int.MinValue).ToList().ForEach(strategy =>
                            {
                                row[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE] = 0;
                            });
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the account value dictionary.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private void UpdateAccountValueDictionary()
        {
            try
            {
                SerializableDictionary<int, AccountValue> accountValues = new SerializableDictionary<int, AccountValue>();
                foreach (DataRow row in AccountStartegyTable.Rows)
                {
                    if (Convert.ToDouble(row[AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE]) != 0)
                    {
                        AccountValue accountValue = CommonAllocationMethods.GetAccountValue(row, false);

                        if (accountValue.Value != 0)
                            accountValues.Add(accountValue.AccountId, accountValue);
                    }
                }
                AccountValueDictionary = accountValues;
                IsRefreshSummary = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clears the data account strategy grid.
        /// </summary>
        internal void ClearDataAccountStrategyGrid()
        {
            try
            {
                if (AccountStartegyTable != null)
                    AccountStartegyTable.Clear();
                AccountValueDictionary = new SerializableDictionary<int, AccountValue>();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Methods

        #region Dispose Methods and Unwire Events

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        internal void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    _accountStartegyTable = null;
                    _accountValueDictionary = null;
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion
    }
}
