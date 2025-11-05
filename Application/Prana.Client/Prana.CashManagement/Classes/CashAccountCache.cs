using Infragistics.Win;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.CashManagement
{
    public class CashAccountCache
    {
        #region Members

        /// <summary>
        /// The cash account cache
        /// </summary>
        private static CashAccountCache _cashAccountCache = new CashAccountCache();

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets the get instance.
        /// </summary>
        /// <value>
        /// The get instance.
        /// </value>
        public static CashAccountCache GetInstance
        {
            get
            {
                return _cashAccountCache;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="CashAccountCache"/> class from being created.
        /// </summary>
        private CashAccountCache()
        {
            Initialize();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the accruals value for give date.
        /// </summary>
        /// <param name="fromdate">The fromdate.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        public DataSet GetAccrualsValueForGiveDate(DateTime fromdate, DateTime toDate)
        {
            DataSet ds = CashAccountDataManager.GetAccrualsValueForGiveDate(fromdate, toDate);

            return ds;
        }

        /// <summary>
        /// Gets all sub accounts.
        /// </summary>
        /// <returns></returns>
        public ValueList GetAllSubAccounts()
        {
            return ValueListHelper.GetInstance.GetSubAccountTypeValueList(); ;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// Saves the cash dividend value.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public int SaveCashDividendValue(string xml)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = CashAccountDataManager.SaveCashDividendValue(xml);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return rowsAffected;
        }

        /// <summary>
        /// Saves the sub account cash value.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public int SaveSubAccountCashValue(string xml)
        {
            int rowsAffected = 0;
            try
            {

                rowsAffected = CashAccountDataManager.SaveSubAccountCashValue(xml);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return rowsAffected;
        }

        /// <summary>
        /// Updates the accrual values.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public int UpdateAccrualValues(DataSet ds)
        {
            return CashAccountDataManager.UpdateAccrualsValuesInDB(ds);
        }

        #endregion Methods
    }
}
