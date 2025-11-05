using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Prana.PostTrade
{
    public class ClosingPrefDataManager
    {
        public static ClosingPreferences GetPreferences()
        {
            ClosingPreferences preferences = new ClosingPreferences();

            try
            {
                bool isShortwithBuyandBuyToClose = false;
                bool isSellWithBuyToClose = false;
                DataSet AccountingMethodsTable = new DataSet();
                bool overrideGlobal = false;
                int closingMethodology = -1;
                int globalClosingAlgo = (int)PostTradeEnums.CloseTradeAlogrithm.FIFO;
                int secondarySort = (int)PostTradeEnums.SecondarySortCriteria.None;
                //http://jira.nirvanasolutions.com:8080/browse/SS-55
                bool isFetchDataAutomatically = true;
                bool isAutoCloseStrategy = false;
                double longTermTaxRate = 1;
                double shortTermTaxRate = 1;
                int priceRoundOffDigits = 4;
                int qtyRoundOffDigits = 8;
                int globalClosingField = (int)PostTradeEnums.ClosingField.Default;
                bool SplitunderlyingBasedOnPosition = false;
                decimal autoOptExerciseValue = 0.01m;
                bool copyOpeningTradeAttributes = false;

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetClosingPreferences";
                queryData.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["HeavyGetTimeout"]);

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (!row[0].Equals(System.DBNull.Value))
                        {
                            byte[] data = (byte[])row[0];

                            MemoryStream stream = new MemoryStream(data);
                            BinaryFormatter bf = new BinaryFormatter();
                            DataSet ds = (DataSet)bf.Deserialize(stream);

                            AccountingMethodsTable = ds;
                        }
                        isShortwithBuyandBuyToClose = row[1] != DBNull.Value ? Convert.ToBoolean(Convert.ToString(row[1])) : isShortwithBuyandBuyToClose;
                        isSellWithBuyToClose = row[2] != DBNull.Value ? Convert.ToBoolean(Convert.ToString(row[2])) : isSellWithBuyToClose;
                        overrideGlobal = row[3] != DBNull.Value ? Convert.ToBoolean(Convert.ToString(row[3])) : overrideGlobal;
                        globalClosingAlgo = row[4] != DBNull.Value ? Convert.ToInt32(Convert.ToString(row[4])) : globalClosingAlgo;
                        secondarySort = row[5] != DBNull.Value ? Convert.ToInt32(Convert.ToString(row[5])) : secondarySort;
                        closingMethodology = row[6] != DBNull.Value ? Convert.ToInt32(Convert.ToString(row[6])) : closingMethodology;
                        isFetchDataAutomatically = row[7] != DBNull.Value ? Convert.ToBoolean(Convert.ToString(row[7])) : isFetchDataAutomatically;

                        longTermTaxRate = row[8] != DBNull.Value ? Convert.ToDouble(Convert.ToString(row[8])) : longTermTaxRate;
                        shortTermTaxRate = row[9] != DBNull.Value ? Convert.ToDouble(Convert.ToString(row[9])) : shortTermTaxRate;
                        qtyRoundOffDigits = row[10] != DBNull.Value ? Convert.ToInt32(Convert.ToString(row[10])) : qtyRoundOffDigits;
                        priceRoundOffDigits = row[11] != DBNull.Value ? Convert.ToInt32(Convert.ToString(row[11])) : priceRoundOffDigits;
                        isAutoCloseStrategy = row[12] != DBNull.Value ? Convert.ToBoolean(Convert.ToString(row[12])) : isAutoCloseStrategy;
                        globalClosingField = row[13] != DBNull.Value ? Convert.ToInt32(Convert.ToString(row[13])) : globalClosingField;
                        SplitunderlyingBasedOnPosition = row[14] != DBNull.Value ? Convert.ToBoolean(Convert.ToString(row[14])) : SplitunderlyingBasedOnPosition;
                        autoOptExerciseValue = row[15] != DBNull.Value ? Convert.ToDecimal(Convert.ToString(row[15])) : autoOptExerciseValue;
                        copyOpeningTradeAttributes = row[16] != DBNull.Value ? Convert.ToBoolean(Convert.ToString(row[1])) : copyOpeningTradeAttributes;
                    }
                }

                if (AccountingMethodsTable.Tables.Count > 0)
                {
                    if (AccountingMethodsTable.Tables[0] != null && !AccountingMethodsTable.Tables[0].Columns.Contains("ClosingField"))
                    {
                        DataColumn colClosingField = new DataColumn("ClosingField", typeof(int));
                        colClosingField.DefaultValue = (int)PostTradeEnums.ClosingField.Default;
                        AccountingMethodsTable.Tables[0].Columns.Add(colClosingField);
                    }
                    preferences.ClosingMethodology.AccountingMethodsTable = AccountingMethodsTable;
                }
                else
                {
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    AccountingMethods.SetDefaultTableAndSchema(CachedDataManager.GetInstance.GetAccounts(), CachedDataManager.GetInstance.GetAllClosingAssets(), ds);
                    preferences.ClosingMethodology.AccountingMethodsTable = ds;
                }

                preferences.ClosingMethodology.OverrideGlobal = overrideGlobal;
                preferences.ClosingMethodology.GlobalClosingMethodology = closingMethodology;
                preferences.ClosingMethodology.ClosingAlgo = (PostTradeEnums.CloseTradeAlogrithm)globalClosingAlgo;
                preferences.ClosingMethodology.SecondarySort = (PostTradeEnums.SecondarySortCriteria)secondarySort;
                preferences.ClosingMethodology.IsShortWithBuyandBTC = isShortwithBuyandBuyToClose;
                preferences.ClosingMethodology.IsSellWithBTC = isSellWithBuyToClose;
                preferences.ClosingMethodology.LongTermTaxRate = longTermTaxRate;
                preferences.ClosingMethodology.ShortTermTaxRate = shortTermTaxRate;
                preferences.IsFetchDataAutomatically = isFetchDataAutomatically;

                preferences.PriceRoundOffDigits = priceRoundOffDigits;
                preferences.QtyRoundoffDigits = qtyRoundOffDigits;
                preferences.AutoOptExerciseValue = autoOptExerciseValue;
                preferences.ClosingMethodology.IsAutoCloseStrategy = isAutoCloseStrategy;
                preferences.ClosingMethodology.ClosingField = (PostTradeEnums.ClosingField)globalClosingField;
                preferences.ClosingMethodology.SplitunderlyingBasedOnPosition = SplitunderlyingBasedOnPosition;
                //get closingDateType from App Settingns xml
                if (ConfigurationManager.AppSettings["ClosingDate"] != null)
                {
                    PostTradeEnums.DateType dateType = (PostTradeEnums.DateType)Convert.ToInt32(ConfigurationManager.AppSettings["ClosingDate"].ToString());
                    preferences.DateType = dateType;
                }
                preferences.CopyOpeningTradeAttributes = copyOpeningTradeAttributes;
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
            return preferences;
        }

        public static void SavePreferences(ClosingPreferences preferences)
        {
            object[] parameter = new object[17];
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            #region try
            try
            {
                if (preferences.ClosingMethodology.AccountingMethodsTable != null)
                {
                    bf.Serialize(stream, preferences.ClosingMethodology.AccountingMethodsTable);
                    byte[] data = new byte[stream.Length];
                    stream.Write(data, 0, data.Length);
                    stream.Seek(0, 0);

                    parameter[0] = stream.ToArray(); //Convert.ToBase64String(data);
                    parameter[1] = preferences.ClosingMethodology.IsShortWithBuyandBTC;
                    parameter[2] = preferences.ClosingMethodology.IsSellWithBTC;
                    parameter[3] = preferences.ClosingMethodology.OverrideGlobal;
                    parameter[4] = (int)preferences.ClosingMethodology.ClosingAlgo;
                    parameter[5] = (int)preferences.ClosingMethodology.SecondarySort;
                    parameter[6] = (int)preferences.ClosingMethodology.GlobalClosingMethodology;
                    //http://jira.nirvanasolutions.com:8080/browse/SS-55
                    parameter[7] = preferences.IsFetchDataAutomatically;
                    parameter[8] = preferences.ClosingMethodology.LongTermTaxRate;
                    parameter[9] = preferences.ClosingMethodology.ShortTermTaxRate;
                    parameter[10] = preferences.QtyRoundoffDigits;
                    parameter[11] = preferences.PriceRoundOffDigits;
                    parameter[12] = preferences.ClosingMethodology.IsAutoCloseStrategy;
                    parameter[13] = preferences.ClosingMethodology.ClosingField;
                    parameter[14] = preferences.ClosingMethodology.SplitunderlyingBasedOnPosition;
                    parameter[15] = preferences.AutoOptExerciseValue;
                    parameter[16] = preferences.CopyOpeningTradeAttributes;
                    DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveClosingPreferences", parameter);
                }
            }
            # endregion
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
        }
    }
}
