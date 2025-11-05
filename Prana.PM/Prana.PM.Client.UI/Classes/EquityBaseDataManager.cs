using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.PM.Client
{
    class EquityAndAccrualsDataManager
    {
        #region equity Base
        //Get company Base Equity Values
        public static clsBaseEquityValues GetCompanyBaseEquityValues()
        {
            clsBaseEquityValues baseEquityValues = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetCompanyBaseEquityValues";

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
            {
                while (reader.Read())
                {
                    baseEquityValues = new clsBaseEquityValues();

                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    if (row != null)
                    {
                        int CompanyBaseEquityValueID = 0;
                        int CompanyID = 1;
                        int BaseEquityValue = 2;
                        int BaseEquityDate = 3;

                        if (row[CompanyBaseEquityValueID] != System.DBNull.Value)
                        {
                            baseEquityValues.CompanyBaseEquityValueId = Convert.ToInt32(row[CompanyBaseEquityValueID]);
                        }
                        if (row[CompanyID] != System.DBNull.Value)
                        {
                            baseEquityValues.CompanyId = Convert.ToInt32(row[CompanyID].ToString());
                        }
                        if (row[BaseEquityValue] != System.DBNull.Value)
                        {
                            baseEquityValues.BaseEquityValue = Convert.ToDouble((row[BaseEquityValue]));
                        }
                        if (row[BaseEquityDate] != System.DBNull.Value)
                        {
                            baseEquityValues.BaseEquityDate = Convert.ToDateTime(row[BaseEquityDate].ToString());
                        }

                    }

                }
            }
            return baseEquityValues;
        }

        //save company Base Equity Values
        public static int SaveCompanyBaseEquityValues(int companyId, DateTime date, double baseEquityValue)
        {
            int result = int.MinValue;

            object[] parameter = new object[3];

            try
            {
                parameter[0] = companyId;
                parameter[1] = Convert.ToDateTime(date);
                parameter[2] = baseEquityValue;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("PMSaveCompanyBaseEquityValues", parameter).ToString());

            }
            #region Catch
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
            #endregion

            return result;
        }

        #endregion
    }
}
