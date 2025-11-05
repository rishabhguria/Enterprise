using Prana.LogManager;
using System;
using System.Data;
using System.Linq;

namespace Prana.CashManagement
{
    public class CashManagementValidations
    {
        #region Members

        /// <summary>
        /// The cash management validations
        /// </summary>
        private static CashManagementValidations _cashManagementValidations = new CashManagementValidations();

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets the get instance.
        /// </summary>
        /// <value>
        /// The get instance.
        /// </value>
        public static CashManagementValidations GetInstance
        {
            get
            {
                return _cashManagementValidations;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="CashManagementValidations"/> class from being created.
        /// </summary>
        private CashManagementValidations()
        {
            //Initialize();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Validates the activity journal mapping.
        /// </summary>
        /// <param name="dsUpdatedDataSet">The ds updated data set.</param>
        /// <returns></returns>
        public string validateActivityJournalMapping(DataSet dsUpdatedDataSet)
        {
            string ErrorMessage = string.Empty;
            try
            {
                foreach (DataTable dt in dsUpdatedDataSet.Tables)
                {
                    switch (dt.TableName)
                    {
                        case CashManagementConstants.TABLE_ACTIVITYAMOUNTTYPE:
                            break;
                        case CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING:
                            if (ValidateAmountType(dt))
                            {
                                ErrorMessage = "Activity journal mapping have duplicate entry for amount type";
                            }
                            break;
                        //case CashManagementConstants.TABLE_ACTIVITYTRANSACTIONMAPPING:
                        //    break;
                        case CashManagementConstants.TABLE_ACTIVITYTYPE:
                            break;
                            //case CashManagementConstants.TABLE_CASHTRANSACTIONTYPE:
                            //    break;
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
            return ErrorMessage;
        }

        /// <summary>
        /// Validates the type of the amount.
        /// </summary>
        /// <param name="dtActivityJournalMapping">The dt activity journal mapping.</param>
        /// <returns></returns>
        private bool ValidateAmountType(DataTable dtActivityJournalMapping)
        {
            bool isError = false;
            try
            {
                //delete the blank rows
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-5315
                dtActivityJournalMapping.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                    && int.Parse(row[CashManagementConstants.COLUMN_AMOUNTTYPEID_FK].ToString()) == int.MinValue
                    && int.Parse(row[CashManagementConstants.COLUMN_CREDITACCOUNT].ToString()) == int.MinValue
                    && int.Parse(row[CashManagementConstants.COLUMN_DEBITACCOUNT].ToString()) == int.MinValue
                    && string.IsNullOrWhiteSpace(row[CashManagementConstants.COLUMN_CASHVALUETYPE].ToString())
                    && string.IsNullOrWhiteSpace(row[CashManagementConstants.COLUMN_ACTIVITYDATETYPE].ToString())).ToList().ForEach(row => row.Delete());
                foreach (DataRow dataRow in dtActivityJournalMapping.Rows)
                {
                    if (dataRow.RowState == DataRowState.Deleted)
                        continue;
                    if (int.Parse(dataRow[CashManagementConstants.COLUMN_AMOUNTTYPEID_FK].ToString()) == int.MinValue)
                    {
                        isError = true;
                    }

                    //else if (int.Parse(dataRow[CashManagementConstants.COLUMN_DEBITACCOUNT].ToString()) == int.MinValue  )
                    //{
                    //    isError = true;
                    //}
                    //else if (int.Parse(dataRow[CashManagementConstants.COLUMN_CREDITACCOUNT].ToString()) == int.MinValue)
                    //{
                    //    isError = true;
                    //}
                    else if (dataRow.RowState.Equals(DataRowState.Added))
                    {
                        //DataRow[] rowsHavingActivityTypeAndAmountType = dtActivityJournalMapping.Select(CashManagementConstants.COLUMN_AMOUNTTYPEID_FK + " = " + dataRow[CashManagementConstants.COLUMN_AMOUNTTYPEID_FK].ToString() + " AND " + CashManagementConstants.COLUMN_ACTIVITYTYPEID_FK + " = " + dataRow[CashManagementConstants.COLUMN_ACTIVITYTYPEID_FK].ToString());
                        //if (rowsHavingActivityTypeAndAmountType.Length > 0)
                        //{
                        //    isError = true;
                        //}
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
            return isError;
        }

        #endregion Methods
    }
}
