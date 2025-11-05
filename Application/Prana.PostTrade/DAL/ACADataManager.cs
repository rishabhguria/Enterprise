using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.IO;
using System.Data.Common;
using Prana.Utilities.XMLUtilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects;
using Prana.Interfaces;
using System.Data.SqlClient;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;

namespace Prana.PostTrade
{
    class ACADataManager
    {
       public static int SaveACAData(List<ACAData> acaDataList,List<DateSymbol> lstSymbols, bool isForSymbol)
        {
            int errorNumber = 0;
            string errorMessage = string.Empty;
            Database db = null;
            int affectedPositions = 0;
            string ACAXml = string.Empty;
            string symbolsxml = string.Empty;
            try
            {
                ACAXml = XMLUtilities.SerializeToXML(acaDataList);

                if (lstSymbols != null)
                {
                    symbolsxml = XMLUtilities.SerializeToXML(lstSymbols);
                }
                string sqlCommand;
                db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = null;
                if (isForSymbol)
                {
                    sqlCommand = "P_SaveACAData_symbol";
                    dbCommand = db.GetStoredProcCommand(sqlCommand);
                    db.AddInParameter(dbCommand, "@Xml", DbType.String, ACAXml);
                    db.AddInParameter(dbCommand, "@symbolsXml", DbType.String, symbolsxml);
                }
                else
                {
                    sqlCommand = "P_SaveACAData";
                    dbCommand = db.GetStoredProcCommand(sqlCommand);
                    db.AddInParameter(dbCommand, "@Xml", DbType.String, ACAXml);
                }
                //DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                //db.AddInParameter(dbCommand, "@Xml", DbType.String, ACAXml);
                affectedPositions = db.ExecuteNonQuery(dbCommand);
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return affectedPositions;
        }

        internal static Dictionary<DateTime, Dictionary<string, string>> GetAllCAs(CAOnProcessObjects caOnProcessObject)
        {
            Database db = null;
            DataSet ds = new DataSet();
            Dictionary<DateTime, Dictionary<string, string>> dictNameChangeData = new Dictionary<DateTime, Dictionary<string, string>>();
            try
            {
                CorporateActionType caType = caOnProcessObject.CAType;
                DateTime fromDate = caOnProcessObject.FromDate;
                DateTime toDate = caOnProcessObject.ToDate;
                bool isApplied = caOnProcessObject.IsApplied;
                db = DatabaseFactory.CreateDatabase();
                object[] parameter = new object[4];
                parameter[0] = Convert.ToInt32(caType);
                parameter[1] = fromDate.ToString();
                parameter[2] = toDate.ToString();
                parameter[3] = isApplied;
                string spName = "P_GetCADetailsForDateRange";
                ds = db.ExecuteDataSet(spName, parameter);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["EffectiveDate"] != System.DBNull.Value)
                    {
                        DateTime EffectiveDate = Convert.ToDateTime(dr["EffectiveDate"].ToString());

                        string Symbol = string.Empty;

                        string NewSymbol = string.Empty;

                        if (dr["Symbol"] != System.DBNull.Value)
                        {
                            Symbol = dr["Symbol"].ToString();
                        }
                        if (dr["NewSymbol"] != System.DBNull.Value)
                        {
                            NewSymbol = dr["NewSymbol"].ToString();
                        }

                        if (Symbol != string.Empty && NewSymbol != string.Empty)
                        {
                            if (dictNameChangeData.ContainsKey(EffectiveDate.Date))
                            {
                                dictNameChangeData[EffectiveDate.Date].Add(Symbol, NewSymbol);
                            }
                            else
                            {
                                Dictionary<string, string> dictSymbols = new Dictionary<string, string>();
                                dictSymbols.Add(Symbol, NewSymbol);
                                dictNameChangeData.Add(EffectiveDate, dictSymbols);
                            }
                        }
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                db = null;

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }

            }
            #endregion

            return dictNameChangeData;

        }

        private static Dictionary<DateTime, List<ACAData>> FillACAData(DataSet ds)
        {
            Dictionary<DateTime, List<ACAData>> dictACAData = new Dictionary<DateTime, List<ACAData>>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ACAData acaData = new ACAData();

                acaData.Symbol = dr["Symbol"].ToString();
                acaData.PositionQty = Convert.ToDouble(dr["PositionQty"].ToString());
                acaData.FundID = Convert.ToInt32(dr["FundID"].ToString());
                acaData.ACAAvgPrice = Convert.ToDouble(dr["ACAAvgPrice"].ToString());
                acaData.Date = Convert.ToDateTime(dr["Date"].ToString());
                acaData.PositionType = dr["PositionType"].ToString();
                acaData.ACAUnitCost = Convert.ToDouble(dr["ACAUnitCost"].ToString());


                if (dictACAData.ContainsKey(acaData.Date))
                {
                    dictACAData[acaData.Date].Add(acaData);
                }
                else
                {
                    List<ACAData> acbDataList = new List<ACAData>();
                    acbDataList.Add(acaData);
                    dictACAData.Add(acaData.Date, acbDataList);
                }
            }
            return dictACAData;
        }

        public static DateTime GetACALatestCalculationDate()
        {
            DateTime LatestcalculationDate = DateTime.MinValue;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                string sqlCommand = "P_GetACALatestCalculationDate";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(dbCommand))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];

                        reader.GetValues(row);
                        if (row != null)
                        {
                            int calculationDateIndex = 0;
                            if (row[calculationDateIndex] != System.DBNull.Value)
                            {
                                LatestcalculationDate = DateTime.Parse((row[calculationDateIndex]).ToString());
                            }
                            else
                            {
                                LatestcalculationDate = DateTime.MinValue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return LatestcalculationDate;
        }

        public static Dictionary<DateTime,List<ACAData>> GetACADataFromDB(List<DateSymbol> lstSymbols, DateTime fromdate, DateTime toDate, bool isForSymbol)
        {
            Dictionary<DateTime, List<ACAData>> dictACAData = new Dictionary<DateTime, List<ACAData>>();

            Database db = null;
            try
            {
                 db = DatabaseFactory.CreateDatabase();
                  string sqlCommand;
                  DbCommand dbCommand = null;
                  if (isForSymbol)
                  {
                      string SymbolsXml = string.Empty;
                      if (lstSymbols != null)
                      {

                          SymbolsXml = XMLUtilities.SerializeToXML(lstSymbols);
                          sqlCommand = "P_GetACAData_Symbol";
                          dbCommand = db.GetStoredProcCommand(sqlCommand);
                          db.AddInParameter(dbCommand, "@xml", DbType.String, SymbolsXml);
                      }
                      else
                      {
                          return dictACAData;
                      }
                  }
                  else
                  {
                      sqlCommand = "P_GetACADataForDateRange";
                      dbCommand = db.GetStoredProcCommand(sqlCommand);
                      db.AddInParameter(dbCommand, "@fromdate", DbType.DateTime, fromdate);
                      db.AddInParameter(dbCommand, "@todate", DbType.DateTime, toDate);
                  }
                  
                  DataSet ds = db.ExecuteDataSet(dbCommand);

                  dictACAData = FillACAData(ds);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return dictACAData;
        }
    }
}
