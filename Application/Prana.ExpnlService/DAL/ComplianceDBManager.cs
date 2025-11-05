using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.ExpnlService
{
    public class ComplianceDBManager
    {
        /// <summary>
        /// Getting 16B rules from DB
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllSixteenBRules()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CA_GetAllSixteenBRules";
                return Prana.DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
                return null;
            }
        }


        public int SaveSixteenBRules(DataSet dsRule)
        {
            int rowAffected = 0;
            try
            {
                foreach (DataRow dr in dsRule.Tables[0].Rows)
                {
                    object[] parameters = new object[4];
                    parameters[0] = dr["RuleId"];
                    parameters[1] = dr["Symbol"];
                    parameters[2] = dr["StartDate"];
                    parameters[3] = dr["CreatedBy"];

                    rowAffected += Prana.DatabaseManager.DatabaseManager.ExecuteNonQuery("P_CA_SaveSixteenBRules", parameters);
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
            return rowAffected;
        }


        /// <summary>
        /// Returns first taxlots containing columns as Symbol, TradeDate, Side, StartDate
        /// </summary>
        /// <param name="values">Needs columns as Symbol, StartDate</param>
        /// <returns>Dateset containing table of inforamtion at index[0]</returns>
        public DataSet GetFirstTaxlotsAfterDate(DataSet values)
        {
            DataSet resultDataSet = new DataSet();
            DataTable dt = new DataTable("RuleFeed");
            dt.Columns.Add("Symbol");
            dt.Columns.Add("orderSideTagValue");
            dt.Columns.Add("auecLocalDate");

            DataTable dtList = values.Tables[0];

            try
            {
                foreach (DataRow dr in dtList.Rows)
                {
                    object[] parameters = new object[2];
                    parameters[0] = dr["Symbol"];
                    parameters[1] = dr["StartDate"];
                    DataSet dtTemp = Prana.DatabaseManager.DatabaseManager.ExecuteDataSet("P_CA_GetFirstTaxlotsAfterDate", parameters);
                    if (dtTemp.Tables.Count > 0)
                    {
                        foreach (DataRow drTemp in dtTemp.Tables[0].Rows)
                        {
                            dt.Rows.Add(drTemp.ItemArray);
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

            //object[] parameters = new object[1];
            //parameters[0] = sbTemp.ToString();
            //parameters[1] = dr["StartDate"];
            resultDataSet.Tables.Add(dt);
            //return db.ExecuteDataSet(procedureName, parameters);
            return resultDataSet;
        }
    }
}