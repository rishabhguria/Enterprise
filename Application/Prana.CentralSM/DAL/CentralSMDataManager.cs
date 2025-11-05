using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace Prana.CentralSM
{
    public class CentralSMDataManager
    {
        public CentralSMDataManager()
        {

        }
        private static int _errorNumber = 0;

        private static string _errorMessage = string.Empty;

        private const string CONST_connStringName = "SMConnectionString";
        private const string CONST_ClientConnStringName = "PranaConnectionString";

        /// <summary>
        /// Picks up the max id from . This id is further used to generate the new distinct ids.
        /// </summary>
        /// <returns></returns>
        public static Int64 GetMaxSymbolPKIDFromDB()
        {
            Database db = DatabaseFactory.CreateDatabase();
            Int64 maxGeneratedID = 0;
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(System.Data.CommandType.StoredProcedure, "P_GetMaxSymbolPKNumber"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != DBNull.Value && row[0].ToString() != string.Empty)
                        {
                            maxGeneratedID = Int64.Parse(row[0].ToString());
                        }
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return maxGeneratedID;
        }

        public static List<SecMasterBaseObj> GetSecMasterDataFromDB_XML(SecMasterRequestObj secMasterRequestObj)
        {
            Database db = null;
            List<SecMasterBaseObj> secMasterObjList = new List<SecMasterBaseObj>();

            try
            {

                //if (secMasterRequestObj.Count == 1)
                //{
                //    if (_notFoundInDb.Contains(secMasterRequestObj.SymbolDataRowCollection[0].PrimarySymbol))
                //    {
                //        return secMasterObjList;
                //    }
                //}


                db = DatabaseFactory.CreateDatabase(CONST_connStringName);
                object[] parameter = new object[1];

                parameter[0] = secMasterRequestObj.CreateXml();
                ///parameter[1] = secMasterRequestObj.RequestedDate;
                // Logger.Write("Request for  " + parameter[0].ToString(), "Category_Tracing");
                string spName = "P_SMGetSecurityMasterData_XML";

                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(spName, parameter))
                {
                    while (reader.Read())
                    {


                        SecMasterBaseObj secMasterObj = GetSecMasterObj(reader);
                        if (secMasterObj != null)
                        {
                            secMasterObjList.Add(secMasterObj);


                        }
                    }
                }
                // if (secMasterRequestObj.Count == 1 && secMasterObjList.Count==0)
                //{
                //    _notFoundInDb.Add(secMasterRequestObj.SymbolDataRowCollection[0].PrimarySymbol);
                //}

                db = null;
            }
            #region Catch
            catch (Exception ex)
            {
                db = null;

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);


                if (rethrow)
                {
                    throw;
                }

            }
            #endregion

            return secMasterObjList;
        }

        internal static void UpdateSymbolToSecurityMaster_Import(string secMasterXml, int dataSource)
        {
            Database db = null;
            int affectedPositions = 0;

            try
            {
                db = DatabaseFactory.CreateDatabase(CONST_connStringName);
                DbCommand cmd = new SqlCommand();
                cmd.CommandText = "P_UpdateSecurityMasterDataForSymbol_Import";
                cmd.CommandType = CommandType.StoredProcedure;
                db.AddInParameter(cmd, "@Xml", DbType.String, secMasterXml);
                db.AddInParameter(cmd, "@dataSource", DbType.Int16, dataSource);

                XMLSaveManager.AddOutErrorParameters(db, cmd);

                affectedPositions = db.ExecuteNonQuery(cmd);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, cmd);

            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                ExceptionPolicy.HandleException(new Exception("Error Message=" + _errorMessage + "Error in Saving Xml:=" + secMasterXml + "\n Error :=" + _errorMessage), ApplicationConstants.POLICY_LOGANDSHOW);
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Get Future Root Data from DB
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static DataSet GetFutureRootData(String connString)
        {
            //System.Collections.Generic.Dictionary<string, FutureRootData> ContractMultipliers
            //    = new Dictionary<string, FutureRootData>();
            if (connString != string.Empty) // for Automation with Connection String 
            {
                SqlConnection con = new SqlConnection(connString);
                DataSet ds = new DataSet();
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("P_GetContractMultipliers", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(ds);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    con.Close();
                }
                return ds;
            }
            else
            {
                Database db = DatabaseFactory.CreateDatabase("SMConnectionString");
                try
                {
                    DataSet ds = db.ExecuteDataSet("P_GetContractMultipliers");
                    return ds;
                }
                #region Catch
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                    if (rethrow)
                    {
                        throw;
                    }
                }
                #endregion
                return null;
            }
        }
        /// <summary>
        /// Save Future Root Data to DB
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string SaveFutureRootData(DataTable dt)
        {
            Database db = null;
            int affectedPositions = 0;
            string secMasterXml = string.Empty;
            try
            {
                MemoryStream stream = new MemoryStream();
                dt.WriteXml(stream);

                byte[] bytes = stream.ToArray();
                secMasterXml = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
                db = DatabaseFactory.CreateDatabase(CONST_connStringName);
                DbCommand cmd = new SqlCommand();
                cmd.CommandText = "P_SaveFutureRootData";
                cmd.CommandType = CommandType.StoredProcedure;
                db.AddInParameter(cmd, "@Xml", DbType.String, secMasterXml);

                XMLSaveManager.AddOutErrorParameters(db, cmd);

                affectedPositions = db.ExecuteNonQuery(cmd);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, cmd);
                return _errorMessage;
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                ExceptionPolicy.HandleException(new Exception("Error Message=" + _errorMessage + "Error in Saving Xml:=" + secMasterXml + "\n Error :=" + _errorMessage), ApplicationConstants.POLICY_LOGANDSHOW);
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _errorMessage;
            #endregion
        }

        // Modified by Bhavana on JULY 2014
        // Purpose : Datasource field is included in secMasterXml
        internal static void SaveNewSymbolResponsetoSecurityMaster(string secMasterXml, string connString)
        {
            Database db = null;
            int affectedPositions = 0;

            try
            {
                if (connString == string.Empty)
                {
                    db = DatabaseFactory.CreateDatabase(CONST_connStringName);
                    DbCommand cmd = new SqlCommand();
                    cmd.CommandText = "P_SaveSecurityMasterDataForSymbol";
                    cmd.CommandTimeout = 300;
                    cmd.CommandType = CommandType.StoredProcedure;
                    db.AddInParameter(cmd, "@Xml", DbType.String, secMasterXml);
                    //db.AddInParameter(cmd, "@dataSource", DbType.Int16, dataSource);
                    XMLSaveManager.AddOutErrorParameters(db, cmd);
                    affectedPositions = db.ExecuteNonQuery(cmd);
                    XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, cmd);

                }
                else
                {
                    SqlConnection con = new SqlConnection(connString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("P_SaveSecurityMasterDataForSymbol", con);
                    cmd.CommandTimeout = 300;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Xml", secMasterXml);
                    //cmd.Parameters.AddWithValue("@dataSource", dataSource);
                    cmd.Parameters.AddWithValue("@ErrorMessage", _errorMessage);
                    cmd.Parameters.AddWithValue("@ErrorNumber", _errorNumber);

                    affectedPositions = cmd.ExecuteNonQuery();
                    XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, cmd);
                    con.Close();
                }


            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                ExceptionPolicy.HandleException(new Exception("Error Message=" + _errorMessage + "Error in Saving Xml:=" + secMasterXml + "\n Error :=" + _errorMessage), ApplicationConstants.POLICY_LOGANDSHOW);
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }
        /// <summary>
        /// Get SymbolLookup Requested Data  normal filter
        /// </summary>
        /// <param name="symbollookXml"></param>
        /// <returns></returns>
        public static DataSet GetSymbolLookupRequestedData(string symbollookXml)
        {
            DataSet ds = new DataSet();

            try
            {
                Database db = DatabaseFactory.CreateDatabase(CONST_connStringName);
                object[] parameter = new object[1];

                parameter[0] = symbollookXml;

                string spName = "P_SearchSecMaster_New";

                ds = db.ExecuteDataSet(spName, parameter);

            }
            #region Catch
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }

            }
            #endregion

            return ds;
        }

        internal static void SaveNewSymbolResponsetoSecurityMaster_Import(string secMasterXml, int dataSource)
        {
            Database db = null;
            int affectedPositions = 0;

            try
            {
                db = DatabaseFactory.CreateDatabase(CONST_connStringName);
                DbCommand cmd = new SqlCommand();
                cmd.CommandText = "P_SaveSecurityMasterDataForSymbol_Import";
                cmd.CommandType = CommandType.StoredProcedure;
                db.AddInParameter(cmd, "@Xml", DbType.String, secMasterXml);
                db.AddInParameter(cmd, "@dataSource", DbType.Int16, dataSource);

                XMLSaveManager.AddOutErrorParameters(db, cmd);

                affectedPositions = db.ExecuteNonQuery(cmd);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, cmd);

            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                ExceptionPolicy.HandleException(new Exception("Error Message=" + _errorMessage + "Error in Saving Xml:=" + secMasterXml + "\n Error :=" + _errorMessage), ApplicationConstants.POLICY_LOGANDSHOW);
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        public static List<SecMasterBaseObj> GetSecMasterDataFromDB_XML(SecMasterRequestObj secMasterRequestObj,  string connString)
        {
            List<SecMasterBaseObj> secMasterObjList = new List<SecMasterBaseObj>();
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("P_SMGetSecurityMasterData_XML", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@xml", secMasterRequestObj.CreateXml());
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SecMasterBaseObj secMasterObj = GetSecMasterObj(reader);
                    if (secMasterObj != null)
                    {
                        secMasterObjList.Add(secMasterObj);
                    }
                }
                //conn.Close();


            }
            #region Catch
            catch (Exception ex)
            {


                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);


                if (rethrow)
                {
                    throw;
                }

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            #endregion

            return secMasterObjList;
        }
        public static SecMasterBaseObj GetSecMasterObj(SqlDataReader reader)
        {
            SecMasterBaseObj secMasterObj = null;
            try
            {
                object[] row = new object[reader.FieldCount];
                reader.GetValues(row);
                int AssetId = int.Parse(reader["AssetID"].ToString());
                secMasterObj = CentralSMDataCache.SecurityMasterFactory.GetSecmasterObject((AssetCategory)AssetId);
                int offset = 9; // 6 basic data elements coming + symbology codes are coming 
                offset = offset + ApplicationConstants.SymbologyCodesCount;
                if (secMasterObj != null)
                {
                    switch ((AssetCategory)AssetId)
                    {
                        case AssetCategory.FXForward:
                            offset += 13;
                            secMasterObj.FillData(row, offset);
                            break;
                        default:
                            AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory((AssetCategory)AssetId);
                            if (secMasterObj != null)
                            {

                                switch (baseAssetCategory)
                                {
                                    case AssetCategory.Equity:
                                        secMasterObj.FillData(row, offset);
                                        break;
                                    case AssetCategory.Option:
                                        offset += 1; // 1 equity element before
                                        secMasterObj.FillData(row, offset);

                                        break;
                                    case AssetCategory.Future:
                                        offset += 6; // 5 option elements are there before.
                                        secMasterObj.FillData(row, offset);
                                        break;
                                    case AssetCategory.FX:
                                        offset += 10; // 3 future elements are there before.
                                        secMasterObj.FillData(row, offset);
                                        break;

                                    case AssetCategory.FixedIncome:
                                        offset += 19;
                                        secMasterObj.FillData(row, offset);
                                        break;
                                    case AssetCategory.Indices:
                                        offset += 7;
                                        secMasterObj.FillData(row, offset);
                                        break;
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return secMasterObj;
        }

        internal static DataSet GetUnderLyingSymbolDetails(string UnderLyingSymbol, string connString)
        {
            DataSet ds = new DataSet();

            try
            {
                if (connString == string.Empty)
                {
                    Database db = DatabaseFactory.CreateDatabase(CONST_connStringName);
                    object[] parameter = new object[1];

                    parameter[0] = UnderLyingSymbol;

                    string spName = "P_SearchUnderLyingSymbol";

                    ds = db.ExecuteDataSet(spName, parameter);
                }
                else
                {
                    SqlConnection con = null;
                    try
                    {
                        con = new SqlConnection(connString);
                        con.Open();
                        SqlCommand sqlCmd = new SqlCommand("P_SearchUnderLyingSymbol", con);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@UnderLyingSymbol", UnderLyingSymbol);
                        SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                        da.Fill(ds);
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    finally
                    {
                        if (con != null)
                        {
                            con.Close();
                        }
                    }

                }

            }
            #region Catch
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }

            }
            #endregion

            return ds;
        }
    }
}