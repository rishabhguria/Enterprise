#region Using namespaces

using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;

#endregion

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for FutureManager.
    /// </summary>
    public class FutureManager
    {
        public FutureManager()
        {
        }
        #region ContractListingType
        public static ContractListingType FillContractListingType(object[] row, int offSet)
        {
            int CONTRACTLISTING_TYPE_ID = 0 + offSet;
            int CONTRACTLISTING_TYPE = 1 + offSet;

            ContractListingType contractListingType = new ContractListingType();
            try
            {
                if (row[CONTRACTLISTING_TYPE_ID] != null)
                {
                    contractListingType.ContractListingTypeID = int.Parse(row[CONTRACTLISTING_TYPE_ID].ToString());
                }
                if (row[CONTRACTLISTING_TYPE] != null)
                {
                    contractListingType.Type = row[CONTRACTLISTING_TYPE].ToString();
                }
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

            return contractListingType;
        }

        public static ContractListingTypes GetContractListingTypes()
        {
            ContractListingTypes contractListingTypes = new ContractListingTypes();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllContractListingTypes";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        contractListingTypes.Add(FillContractListingType(row, 0));
                    }
                }
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
            return contractListingTypes;
        }

        public static int SaveContractListingType(ContractListingType contractListingType)
        {
            int result = int.MinValue;
            try
            {
                object[] parameter = new object[3];
                parameter[0] = contractListingType.ContractListingTypeID;
                parameter[1] = contractListingType.Type;
                parameter[2] = result;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveContractListingType", parameter).ToString());

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

        public static bool DeleteContractListingType(int contractListingTypeID, bool deleteForceFully)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[2];
                parameter[0] = contractListingTypeID;
                parameter[1] = (deleteForceFully == true ? 1 : 0);
                //TODO: Modify the delete currency type procedure.
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteContractListingType", parameter) > 0)
                {
                    result = true;
                }

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
        #region FutureMonthCode
        public static FutureMonthCode FillFutureMonthCode(object[] row, int offSet)
        {
            int FUTUREMONTH_CODE_ID = 0 + offSet;
            int FUTUREMONTH = 1 + offSet;
            int ABBREVIATION = 2 + offSet;

            FutureMonthCode futureMonthCode = new FutureMonthCode();
            try
            {
                if (row[FUTUREMONTH_CODE_ID] != null)
                {
                    futureMonthCode.FutureMonthCodeID = int.Parse(row[FUTUREMONTH_CODE_ID].ToString());
                }
                if (row[FUTUREMONTH] != null)
                {
                    futureMonthCode.FutureMonth = row[FUTUREMONTH].ToString();
                }
                if (row[ABBREVIATION] != null)
                {
                    futureMonthCode.Abbreviation = row[ABBREVIATION].ToString();
                }
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

            return futureMonthCode;
        }

        public static FutureMonthCodes GetFutureMonthCodes()
        {
            FutureMonthCodes futureMonthCodes = new FutureMonthCodes();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllFutureMonthCodes";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        futureMonthCodes.Add(FillFutureMonthCode(row, 0));
                    }
                }
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
            return futureMonthCodes;
        }

        public static int SaveFutureMonthCode(FutureMonthCode futureMonthCode)
        {
            int result = int.MinValue;
            try
            {
                object[] parameter = new object[4];
                parameter[0] = futureMonthCode.FutureMonthCodeID;
                parameter[1] = futureMonthCode.FutureMonth;
                parameter[2] = futureMonthCode.Abbreviation;
                parameter[3] = result;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveFutureMonthCode", parameter).ToString());

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

        public static bool DeleteFutureMonthCode(int futureMonthCodeID, bool deleteForceFully)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[2];
                parameter[0] = futureMonthCodeID;
                parameter[1] = (deleteForceFully == true ? 1 : 0);
                //TODO: Modify the delete FutureMonthCode procedure.
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteFutureMonthCode", parameter) > 0)
                {
                    result = true;
                }

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
