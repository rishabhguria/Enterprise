using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.ClientCommon
{
    public class ValueListHelper : IDisposable
    {

        private static ValueListHelper _instance = null;
        static object _locker = new object();
        private ValueListHelper()
        {
            try
            {
                FillValueLists();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static ValueListHelper GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new ValueListHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        private const string TABLE_ACTIVITYTYPE = "ActivityType";
        private const string TABLE_ACTIVITYAMOUNTTYPE = "AmountType";
        //private const string TABLE_CASHTRANSACTIONTYPE = "CashTransactionType";
        private const string TABLE_ACTIVITYJOURNALMAPPING = "ActivityJournalMapping";
        private const string TABLE_SUBACCOUNTS = "SubAccounts";
        private const string TABLE_ACTIVITYDATETYPE = "ActivityDateType";
        ValueList _vlActivityType = new ValueList();
        ValueList _vlCashActivityType = new ValueList();
        ValueList _vlAmountType = new ValueList();
        ValueList _vlSubAccount = new ValueList();
        ValueList _vlCashTransactionType = new ValueList();
        ValueList _vlCashTransactionDateType = new ValueList();
        ValueList _vlActivitySourceType = new ValueList();
        ValueList _vlCashValueType = new ValueList();


        //Narendra Kumar Jangir Aug 26,2013
        //Get Value list for Transaction Type field
        public ValueList GetTransactionTypeValueList()
        {
            ValueList transactionTypeValueList = new ValueList();
            try
            {
                Dictionary<string, string> dictTransactionType = CachedDataManager.GetInstance.DictTransactionType;
                foreach (KeyValuePair<string, string> kvp in dictTransactionType)
                {
                    transactionTypeValueList.ValueListItems.Add(kvp.Key, kvp.Value);

                    // this dictionary is used to get transaction type acromyn from transaction type name
                    // key is transaction type name and vaule is Acronym
                    if (!CachedDataManager.GetInstance.DictTransactionType_Acronym.ContainsKey(kvp.Value))
                    {
                        CachedDataManager.GetInstance.DictTransactionType_Acronym.Add(kvp.Value, kvp.Key);
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
            return transactionTypeValueList;
        }

        /// <summary>
        /// Fills the value lists.
        /// </summary>
        public void FillValueLists()
        {
            try
            {
                List<string> activityTypesForSymbolLevelAccrual = new List<string>() 
                { "Bond_Trading_Interest", "Bond_Interest_Received", "Bond_Interest_Paid", "Bond_Trading_Payable" };
                
                DataSet dsActivities = CachedDataManager.GetInstance.GetAllActivityTables();
                _vlActivityType.Reset();
                _vlActivityType.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                //since _vlActivityType and _vlCashActivityType have same data but they are used in different grid so separate value list required
                //http://www.infragistics.com/community/forums/t/8298.aspx
                _vlCashActivityType.Reset();
                _vlCashActivityType.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                _vlAmountType.Reset();
                _vlAmountType.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                _vlSubAccount.Reset();
                _vlSubAccount.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                _vlCashTransactionType.Reset();
                _vlCashTransactionDateType.Reset();
                _vlActivitySourceType.Reset();
                _vlCashValueType.Reset();
                //_vlCashTransactionType.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);

                foreach (DataRow row in dsActivities.Tables[TABLE_ACTIVITYTYPE].Rows)
                {
                    _vlActivityType.ValueListItems.Add(Convert.ToInt32(row[0]), row[1].ToString());
                }
                foreach (DataRow row in dsActivities.Tables[TABLE_ACTIVITYTYPE].Rows)
                {
                    _vlCashActivityType.ValueListItems.Add(Convert.ToInt32(row[0]), row[1].ToString());
                }
                foreach (DataRow row in dsActivities.Tables[TABLE_ACTIVITYAMOUNTTYPE].Rows)
                {
                    _vlAmountType.ValueListItems.Add(Convert.ToInt32(row[0]), row[1].ToString());
                }
                foreach (DataRow row in dsActivities.Tables[TABLE_SUBACCOUNTS].Rows)
                {
                    _vlSubAccount.ValueListItems.Add(Convert.ToInt32(row[0]), row[1].ToString());
                }
                foreach (DataRow row in dsActivities.Tables[TABLE_ACTIVITYTYPE].Rows)
                {
                    //Bind only cash transaction activities to value list
                    if (row[4].ToString().Equals(((int)(ActivitySource.NonTrading)).ToString()) || row[4].ToString().Equals(((int)(ActivitySource.Dividend)).ToString()) || activityTypesForSymbolLevelAccrual.Contains(row[1].ToString()))
                    {
                        _vlCashTransactionType.ValueListItems.Add(Convert.ToInt32(row[0]), row[1].ToString());
                    }
                }
                //fill value list of activity date type from datatable
                foreach (DataRow row in dsActivities.Tables[TABLE_ACTIVITYDATETYPE].Rows)
                {
                    _vlCashTransactionDateType.ValueListItems.Add(Convert.ToInt32(row[0]), row[1].ToString());
                }
                //bind cashvaluetype enum to value list
                string[] cashValueTypes = Enum.GetNames(typeof(CashValueType));
                for (int counter = 0; counter < cashValueTypes.Length; counter++)
                {
                    CashValueType cashValueType = (CashValueType)Enum.Parse(typeof(CashValueType), cashValueTypes[counter]);
                    _vlCashValueType.ValueListItems.Add(Convert.ToInt32(cashValueType), cashValueType.ToString());
                }
                //bind activitysource enum to value list
                string[] activitySourceTypes = Enum.GetNames(typeof(ActivitySource));
                for (int counter = 0; counter < activitySourceTypes.Length; counter++)
                {
                    ActivitySource activitySourceType = (ActivitySource)Enum.Parse(typeof(ActivitySource), activitySourceTypes[counter]);
                    _vlActivitySourceType.ValueListItems.Add(Convert.ToInt32(activitySourceType), activitySourceType.ToString());
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
        /// Added By Atul Dislay
        /// Returns Value List of user account (after removing -select-)
        /// </summary>
        /// <returns> ValueList convertedAccounts </returns>
        public ValueList GetUserAccountsAsValueList()
        {
            AccountCollection userAccounts = CachedDataManager.GetInstance.GetUserAccounts();
            ValueList convertedAccounts = new ValueList();
            try
            {
                if (userAccounts != null)
                {
                    foreach (Account account in userAccounts)
                    {
                        if (!convertedAccounts.ValueListItems.Contains(account.Name) && account.AccountID != int.MinValue)
                        {
                            convertedAccounts.ValueListItems.Add(account.AccountID, account.Name.ToString());
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
            return convertedAccounts;
        }

        public ValueList GetAmountTypeValueList()
        {
            return _vlAmountType;
        }

        public ValueList GetCashActivityTypeValueList()
        {
            return _vlCashActivityType;
        }

        public ValueList GetCashTransactionTypeValueList()
        {
            return _vlCashTransactionType;
        }

        public ValueList GetCashTransactionDateTypeValueList()
        {
            return _vlCashTransactionDateType;
        }

        public ValueList GetCashValueTypeValueList()
        {
            return _vlCashValueType;
        }

        public ValueList GetActivityTypeValueList()
        {
            return _vlActivityType;
        }

        public ValueList GetSubAccountTypeValueList()
        {
            return _vlSubAccount;
        }

        public ValueList GetActivitySourceTypeValueList()
        {
            return _vlActivitySourceType;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _vlActivitySourceType.Dispose();
                _vlActivityType.Dispose();
                _vlAmountType.Dispose();
                _vlCashActivityType.Dispose();
                _vlCashTransactionDateType.Dispose();
                _vlCashTransactionType.Dispose();
                _vlCashValueType.Dispose();
                _vlSubAccount.Dispose();
            }
        }

        #endregion

    }
}
