using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Prana.ThirdPartyManager
{
    public class ThirdPartyFlatFileManager
    {
        private ThirdPartyFlatFileManager()
        {
        }


        /// <summary>
        /// To fill the object of ThirdPartyFlatFileDetail with the third party details.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static ThirdPartyFlatFileDetail FillTPFlatFileDetail(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            ThirdPartyFlatFileDetail thirdPartyFlatFileDetail = new ThirdPartyFlatFileDetail();

            if (row != null)
            {
                int THIRDPARTYID = offset + 0;
                int THIRDPARTY = offset + 1;
                int THIRDPARTYTYPEID = offset + 2;
                int THIRDPARTYTYPE = offset + 3;
                int SAVEPATH = offset + 4;
                int NAMINGCONVENTION = offset + 5;
                int COMPANYIDENTIFIER = offset + 6;

                thirdPartyFlatFileDetail.ThirdPartyID = Convert.ToInt32(row[THIRDPARTYID]);
                thirdPartyFlatFileDetail.ThirdParty = Convert.ToString(row[THIRDPARTY]);
                thirdPartyFlatFileDetail.ThirdPartyTypeID = Convert.ToInt32(row[THIRDPARTYTYPEID]);
                thirdPartyFlatFileDetail.ThirdPartyType = Convert.ToString(row[THIRDPARTYTYPE]);
                thirdPartyFlatFileDetail.SavePath = Convert.ToString(row[SAVEPATH]);
                thirdPartyFlatFileDetail.NamingConvention = Convert.ToString(row[NAMINGCONVENTION]);
                thirdPartyFlatFileDetail.CompanyIdentifier = Convert.ToString(row[COMPANYIDENTIFIER]);

            }
            return thirdPartyFlatFileDetail;
        }

        /// <summary>
        /// The method to fetch the thirdparty for flat file.
        /// </summary>
        /// <param name="thirdpartyID"></param>
        /// <returns></returns>
        public static ThirdPartyFlatFileDetail GetFFThirdPartDetails(int thirdPartyID)
        {
            ThirdPartyFlatFileDetail thirdPartyFlatFileDetail = new ThirdPartyFlatFileDetail();

            Object[] parameter = new object[1];
            parameter[0] = thirdPartyID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_FFGetThirdPartyDetails", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        thirdPartyFlatFileDetail = FillTPFlatFileDetail(row, 0);
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion Catch
            return thirdPartyFlatFileDetail;
        }

        /// <summary>
        /// the method is to fetch the Thirdparty Account details.
        /// </summary>
        /// <param name="thirdpartyID"></param>
        /// <param name="companyFundIDs"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified By: Ankit Gupta
        /// ON January 22, 2014
        /// Purpose: To fetch data based on the FileFormatID
        /// </remarks>
        public static ThirdPartyFlatFileDetailCollection GetFFThirdPartyAccountDetails(int thirdpartyID, StringBuilder companyFundIDs, DateTime inputDate, int companyID, bool l2DataReq, StringBuilder auecIds, bool exportOnly, int thirdPartyTypeID, int dateType, int fileFormatID, bool includeSent)
        {
            ThirdPartyFlatFileDetailCollection thirdPartyFlatFileDetailCollection = new ThirdPartyFlatFileDetailCollection();
            //List<ThirdPartyFlatFileDetail> thirdPartyFlatFileDetailCollection = new List<ThirdPartyFlatFileDetail>();
            Object[] parameter = new object[9];
            parameter[0] = thirdpartyID;
            parameter[1] = companyFundIDs.ToString();
            parameter[2] = inputDate;
            //parameter[2] = inputDate.ToString("yyyy/MM/dd");
            parameter[3] = companyID;
            parameter[4] = auecIds.ToString();
            parameter[5] = thirdPartyTypeID;
            parameter[6] = dateType;
            parameter[7] = fileFormatID;
            parameter[8] = includeSent;

            string strSPName = string.Empty;

            if (!exportOnly)
            {
                if (l2DataReq.Equals(true))
                {
                    strSPName = "P_FFGetThirdPartyLevel2Allocation";
                }
                else
                {
                    strSPName = "P_FFGetThirdPartyFundsDetails";
                }
            }
            else
            {
                if (l2DataReq.Equals(true))
                {
                    strSPName = "P_FFGetThirdPartyLevel2AllocationExportOnly";
                }
                else
                {
                    strSPName = "P_FFGetThirdPartyFundsDetailsExportOnly";
                }
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[ApplicationConstants.PranaConnectionString].ConnectionString))
                {
                    using (DbCommand cmd = new SqlCommand(strSPName, conn))
                    {
                        cmd.CommandText = strSPName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 5000;
                        cmd.Parameters.Add(new SqlParameter("@thirdPartyID", thirdpartyID) { DbType = DbType.Int32 });
                        cmd.Parameters.Add(new SqlParameter("@companyFundIDs", companyFundIDs.ToString()) { DbType = DbType.String });
                        cmd.Parameters.Add(new SqlParameter("@inputDate", inputDate) { DbType = DbType.DateTime });
                        cmd.Parameters.Add(new SqlParameter("@companyID", companyID) { DbType = DbType.Int32 });
                        cmd.Parameters.Add(new SqlParameter("@auecIDs", auecIds.ToString()) { DbType = DbType.String });
                        cmd.Parameters.Add(new SqlParameter("@TypeID", thirdPartyTypeID) { DbType = DbType.Int32 });
                        cmd.Parameters.Add(new SqlParameter("@dateType", dateType) { DbType = DbType.Int32 });
                        cmd.Parameters.Add(new SqlParameter("@fileFormatID", fileFormatID) { DbType = DbType.Int32 });
                        cmd.Parameters.Add(new SqlParameter("@includeSent", includeSent) { DbType = DbType.Boolean });
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                object[] row = new object[reader.FieldCount];
                                reader.GetValues(row);
                                thirdPartyFlatFileDetailCollection.AddItem(FillThirdPartyFlatFileDetail(row, 0));
                            }
                        }
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
            return thirdPartyFlatFileDetailCollection;
        }


        /// <summary>
        /// Get third Party details --> direct stored procedure call
        /// </summary>
        /// <returns></returns>
        /// Modified By: Ankit Gupta
        /// ON January 22, 2014
        /// Purpose: To fetch data based on the FileFormatID
        /// </remarks>
        public static DataSet GetFFThirdPartyAccountDetails_SPCall(string spName, int thirdpartyID, StringBuilder thirdPartyAccountID, DateTime inputDate, int companyID, StringBuilder auecIds, int thirdPartyTypeID, int dateType, int fileFormatID)
        {
            DataSet ds = null;

            //string strSPName = spName;
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = spName;
            queryData.CommandTimeout = 5000;
            queryData.DictionaryDatabaseParameter.Add("@thirdPartyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@thirdPartyID",
                ParameterType = DbType.Int32,
                ParameterValue = thirdpartyID
            });
            queryData.DictionaryDatabaseParameter.Add("@companyFundIDs", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@companyFundIDs",
                ParameterType = DbType.String,
                ParameterValue = thirdPartyAccountID.ToString()
            });
            queryData.DictionaryDatabaseParameter.Add("@inputDate", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@inputDate",
                ParameterType = DbType.DateTime,
                ParameterValue = inputDate
            });
            queryData.DictionaryDatabaseParameter.Add("@companyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@companyID",
                ParameterType = DbType.Int32,
                ParameterValue = companyID
            });
            queryData.DictionaryDatabaseParameter.Add("@auecIDs", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@auecIDs",
                ParameterType = DbType.String,
                ParameterValue = auecIds.ToString()
            });
            queryData.DictionaryDatabaseParameter.Add("@TypeID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@TypeID",
                ParameterType = DbType.Int32,
                ParameterValue = thirdPartyTypeID
            });
            queryData.DictionaryDatabaseParameter.Add("@dateType", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@dateType",
                ParameterType = DbType.Int32,
                ParameterValue = dateType
            });
            queryData.DictionaryDatabaseParameter.Add("@fileFormatID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@fileFormatID",
                ParameterType = DbType.Int32,
                ParameterValue = fileFormatID
            });

            try
            {
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
            return ds;
        }
        /// <summary>
        /// the method is used to fill an instance of 'ThirdPartyFlatFileDetail' with the saved details of the selected Third Party Accounts. 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static ThirdPartyFlatFileDetail FillTPAccountsSavedDetails(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            ThirdPartyFlatFileDetail thirdPartyFlatFileDetail = new ThirdPartyFlatFileDetail();
            try
            {
                if (row != null)
                {
                    int ThirdPartyFFRunID = offset + 0;
                    int FFUserID = offset + 1;
                    int CompanyID = offset + 2;
                    int CompanyThirdPartyID = offset + 3;
                    int DateTime = offset + 4;
                    int StatusID = offset + 5;
                    int FilePathName = offset + 6;
                    //int ThirdPartyName = offset + 7;
                    //int CompanyIdentifier = offset + 8;
                    int CompanyAccountID = offset + 7;
                    //int Prana2ClientXSLTPath = offset + 10;
                    //int Client2PranaXSLTPath = offset + 11;
                    int Status = offset + 8;
                    int Side = offset + 9;
                    int Symbol = offset + 10;
                    int ExecQty = offset + 11;
                    int OrderType = offset + 12;
                    int OrderTypeTag = offset + 13;
                    int AveragePrice = offset + 14;
                    int Asset = offset + 15;
                    int AssetID = offset + 16;
                    int UnderLyingID = offset + 17;
                    int UnderLying = offset + 18;
                    int CurrencyID = offset + 19;
                    int Exchange = offset + 20;
                    int ExchangeID = offset + 21;
                    int Currency = offset + 22;
                    int CommissionRate = offset + 23;
                    int SecFees = offset + 24;
                    int AUECID = offset + 25;
                    int NetAmt = offset + 26;
                    int GrossAmt = offset + 27;
                    int CommissionCharged = offset + 28;
                    int SecurityIDType = offset + 29;
                    int CommissionRateTypeID = offset + 30;
                    int CommissionRateType = offset + 31;
                    int CompanyCVID = offset + 32;
                    int CVName = offset + 33;
                    int CVIdentifier = offset + 34;
                    int Percentage = offset + 35;
                    int OrderQty = offset + 36;
                    int EntityID = offset + 37;
                    //int OrderTag = offset + 38;
                    int CounterPartyID = offset + 38;
                    int CounterParty = offset + 39;
                    //int TradAccntID = offset + 41;
                    int AlllocQty = offset + 40;
                    int TotalQty = offset + 41;
                    int AccountName = offset + 42;
                    int AccountMappedName = offset + 43;
                    int AccountNo = offset + 44;
                    int AccountTypeID = offset + 45;
                    int AccountType = offset + 46;
                    int OtherBrokerFee = offset + 47;
                    int PutOrCall = offset + 48; // added by sandeep as on 03-oct-2007
                    int CurrencySymbol = offset + 49; // added by sandeep as on 03-oct-2007
                    int StrikePrice = offset + 50; // added by sandeep as on 04-oct-2007
                    int ExpirationDate = offset + 51; // added by sandeep as on 04-oct-2007
                    int SettlementDate = offset + 52; // added by sandeep as on 04-oct-2007
                    int CompanyIdentifier = offset + 53; // added by sandeep as on 20-Nov-2007
                    int ThirdPartyTypeID = offset + 54; // added by sandeep as on 20-Nov-2007
                    int ThirdPartyType = offset + 55; // added by sandeep as on 20-Nov-2007
                    int VenueID = offset + 56; // added by sandeep as on 20-Nov-2007
                    int VenueName = offset + 57; // added by sandeep as on 20-Nov-2007
                    int ThirdPartyName = offset + 58; // added by sandeep as on 20-Nov-2007
                    int CUSIP = offset + 59; // added by sandeep as on 09-04-2008
                    int ISIN = offset + 60; // added by sandeep as on 09-04-2008
                    int SEDOL = offset + 61; // added by sandeep as on 09-04-2008
                    int RIC = offset + 62; // added by sandeep as on 09-04-2008
                    int BBCode = offset + 63; // added by sandeep as on 09-04-2008
                    int FullSecurityName = offset + 64; // added by sandeep as on 27-05-2008
                    int PBUniqueID = offset + 65; // added by sandeep as on 05-07-2008
                    int AllocationSeqNo = offset + 66; // added by sandeep as on 05-07-2008
                    int TaxLotStateID = offset + 67;
                    int ClearingBrokerFee = offset + 68;
                    int SoftCommissionRate = offset + 69;
                    int SoftCommissionCharged = offset + 70;

                    thirdPartyFlatFileDetail.ThirdPartyFFRunID = Convert.ToInt32(row[ThirdPartyFFRunID]);
                    thirdPartyFlatFileDetail.FFUserID = Convert.ToInt32(row[FFUserID]);
                    thirdPartyFlatFileDetail.CompanyID = Convert.ToInt32(row[CompanyID]);
                    thirdPartyFlatFileDetail.ThirdPartyID = Convert.ToInt32(row[CompanyThirdPartyID]);
                    thirdPartyFlatFileDetail.ThirdParty = row[ThirdPartyName].ToString();
                    thirdPartyFlatFileDetail.TradeDate = Convert.ToDateTime(row[DateTime]).ToShortDateString();
                    thirdPartyFlatFileDetail.StatusID = Convert.ToInt32(row[StatusID]);
                    thirdPartyFlatFileDetail.Status = Convert.ToString(row[Status]);
                    thirdPartyFlatFileDetail.FilePathName = Convert.ToString(row[FilePathName]);
                    thirdPartyFlatFileDetail.CompanyAccountID = Convert.ToInt32(row[CompanyAccountID]);
                    thirdPartyFlatFileDetail.Status = Convert.ToString(row[Status]);
                    thirdPartyFlatFileDetail.StatusID = Convert.ToInt32(row[StatusID]);
                    thirdPartyFlatFileDetail.Side = Convert.ToString(row[Side]);
                    thirdPartyFlatFileDetail.Symbol = Convert.ToString(row[Symbol]);
                    thirdPartyFlatFileDetail.ExecutedQty = Convert.ToDouble(row[ExecQty]);
                    thirdPartyFlatFileDetail.OrderType = Convert.ToString(row[OrderType]);
                    thirdPartyFlatFileDetail.OrderTypeTag = Convert.ToInt32(row[OrderTypeTag]);
                    thirdPartyFlatFileDetail.AveragePrice = Convert.ToDouble(row[AveragePrice]);
                    thirdPartyFlatFileDetail.Asset = Convert.ToString(row[Asset]);
                    thirdPartyFlatFileDetail.AssetID = Convert.ToInt32(row[AssetID]);
                    thirdPartyFlatFileDetail.UnderLyingID = Convert.ToInt32(row[UnderLyingID]);
                    thirdPartyFlatFileDetail.UnderLying = Convert.ToString(row[UnderLying]);
                    thirdPartyFlatFileDetail.CurrencyID = Convert.ToInt32(row[CurrencyID]);
                    thirdPartyFlatFileDetail.Exchange = Convert.ToString(row[Exchange]);
                    thirdPartyFlatFileDetail.ExchangeID = Convert.ToInt32(row[ExchangeID]);
                    thirdPartyFlatFileDetail.CurrencyName = Convert.ToString(row[Currency]);
                    thirdPartyFlatFileDetail.Commission = Convert.ToDouble(row[CommissionRate]);
                    thirdPartyFlatFileDetail.SoftCommission = Convert.ToDouble(row[SoftCommissionRate]);
                    thirdPartyFlatFileDetail.SecFees = Convert.ToDouble(row[SecFees]);
                    thirdPartyFlatFileDetail.AUECID = Convert.ToInt32(row[AUECID]);
                    thirdPartyFlatFileDetail.NetAmount = Convert.ToDouble(row[NetAmt]);
                    thirdPartyFlatFileDetail.GrossAmount = Convert.ToDouble(row[GrossAmt]);
                    thirdPartyFlatFileDetail.CommissionCharged = Convert.ToDouble(row[CommissionCharged]);
                    thirdPartyFlatFileDetail.SoftCommissionCharged = Convert.ToDouble(row[SoftCommissionCharged]);
                    thirdPartyFlatFileDetail.SecurityIDType = Convert.ToString(row[SecurityIDType]);
                    thirdPartyFlatFileDetail.CommissionRateTypeID = Convert.ToInt32(row[CommissionRateTypeID]);
                    thirdPartyFlatFileDetail.CommissionRateType = Convert.ToString(row[CommissionRateType]);
                    thirdPartyFlatFileDetail.CompanyCVID = Convert.ToInt32(row[CompanyCVID]);
                    thirdPartyFlatFileDetail.CVName = Convert.ToString(row[CVName]);
                    thirdPartyFlatFileDetail.CVIdentifier = Convert.ToString(row[CVIdentifier]);
                    thirdPartyFlatFileDetail.Percentage = Convert.ToDouble(row[Percentage]);
                    thirdPartyFlatFileDetail.OrderQty = Convert.ToDouble(row[OrderQty]);
                    thirdPartyFlatFileDetail.EntityID = Convert.ToString(row[EntityID]);
                    if (thirdPartyFlatFileDetail.EntityID != "NUL")
                    {
                        thirdPartyFlatFileDetail.TradeRefID = thirdPartyFlatFileDetail.EntityID;
                    }
                    //thirdPartyFlatFileDetail.OrderID = Convert.ToInt64(row[OrderTag]);
                    thirdPartyFlatFileDetail.CounterPartyID = Convert.ToInt32(row[CounterPartyID]);
                    thirdPartyFlatFileDetail.CounterParty = Convert.ToString(row[CounterParty]);
                    //thirdPartyFlatFileDetail.TradAccntID = Convert.ToInt32(row[TradAccntID]);
                    thirdPartyFlatFileDetail.AllocatedQty = Convert.ToDouble(row[AlllocQty]);
                    thirdPartyFlatFileDetail.TotalQty = Convert.ToDouble(row[TotalQty]);
                    thirdPartyFlatFileDetail.AccountName = Convert.ToString(row[AccountName]);
                    thirdPartyFlatFileDetail.AccountMappedName = Convert.ToString(row[AccountMappedName]);
                    thirdPartyFlatFileDetail.AccountNo = Convert.ToString(row[AccountNo]);
                    thirdPartyFlatFileDetail.CompanyAccountTypeID = Convert.ToInt32(row[AccountTypeID]);
                    thirdPartyFlatFileDetail.CompanyAccountType = Convert.ToString(row[AccountType]);
                    thirdPartyFlatFileDetail.OtherBrokerFee = Convert.ToDouble(row[OtherBrokerFee]);
                    thirdPartyFlatFileDetail.ClearingBrokerFee = Convert.ToDouble(row[ClearingBrokerFee]);
                    thirdPartyFlatFileDetail.PutOrCall = Convert.ToString(row[PutOrCall]);// added by sandeep as on 03-oct-2007
                    thirdPartyFlatFileDetail.CurrencySymbol = Convert.ToString(row[CurrencySymbol]);// added by sandeep as on 03-oct-2007
                    if (row[StrikePrice] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.StrikePrice = Convert.ToDouble(row[StrikePrice]);// added by sandeep as on 04-oct-2007
                    }
                    if (row[ExpirationDate] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.ExpirationDate = Convert.ToString(row[ExpirationDate]);// added by sandeep as on 04-oct-2007
                    }
                    if (row[SettlementDate] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.SettlementDate = Convert.ToString(row[SettlementDate]);// added by sandeep as on 04-oct-2007
                    }
                    if (row[CompanyIdentifier] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CompanyIdentifier = row[CompanyIdentifier].ToString();// added by sandeep as on 20-Nov-2007
                    }
                    if (row[ThirdPartyTypeID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.ThirdPartyTypeID = Convert.ToInt32(row[ThirdPartyTypeID]);// added by sandeep as on 20-Nov-2007
                    }
                    if (row[ThirdPartyType] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.ThirdPartyType = row[ThirdPartyType].ToString();// added by sandeep as on 20-Nov-2007
                    }
                    if (row[VenueID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.VenueID = Convert.ToInt32(row[VenueID].ToString());// added by sandeep as on 20-Nov-2007
                    }
                    if (row[VenueName] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.VenueName = row[VenueName].ToString();// added by sandeep as on 20-Nov-2007
                    }
                    if (row[CUSIP] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CUSIP = row[CUSIP].ToString();// added by sandeep as on 09-04-2008
                    }
                    if (row[ISIN] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.ISIN = row[ISIN].ToString();// added by sandeep as on 09-04-2008
                    }
                    if (row[SEDOL] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.SEDOL = row[SEDOL].ToString();// added by sandeep as on 09-04-2008
                    }
                    if (row[RIC] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.RIC = row[RIC].ToString();// added by sandeep as on 09-04-2008
                    }
                    if (row[BBCode] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.BBCode = row[BBCode].ToString();// added by sandeep as on 09-04-2008
                    }
                    if (row[FullSecurityName] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.FullSecurityName = row[FullSecurityName].ToString();// added by sandeep as on 09-04-2008
                    }
                    if (row[PBUniqueID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.PBUniqueID = Convert.ToInt64(row[PBUniqueID].ToString());// added by sandeep as on 05-07-2008
                    }
                    if (row[AllocationSeqNo] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.AllocationSeqNo = Convert.ToInt32(row[AllocationSeqNo].ToString());// added by sandeep as on 05-07-2008
                    }
                    if (row[TaxLotStateID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.TaxLotStateID = Convert.ToInt32(row[TaxLotStateID].ToString());
                        thirdPartyFlatFileDetail.TaxLotState = (PranaTaxLotState)Convert.ToInt32(row[TaxLotStateID].ToString());
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

            return thirdPartyFlatFileDetail;
        }

        /// <summary>
        /// The method is used to get the status of the selected ThirdPartyAccount, whether its details have been saved, FF generated or not.
        /// </summary>
        /// <param name="thirdPartyID"></param>
        /// <param name="companyAccountID"></param>
        /// <param name="inputDate"></param>
        /// <returns></returns>
        public static string GetThirdPartyFFAccountSavedStatus(int thirdPartyID, int companyAccountID, DateTime inputDate)
        {
            string status = string.Empty;

            Object[] parameter = new object[3];
            parameter[0] = thirdPartyID;
            parameter[1] = companyAccountID;
            parameter[2] = inputDate;
            try
            {
                status = Convert.ToString(DatabaseManager.DatabaseManager.ExecuteScalar("P_FFGetThirdPartyFundSavedStatus", parameter));

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion Catch
            return status;

        }

        /// <summary>
        /// The method is used to check the status of the selected Third Party Account.
        /// </summary>
        /// <param name="thirdPartyID"></param>
        /// <param name="companyAccountID"></param>
        /// <param name="inputDate"></param>
        /// <returns></returns>
        public static ThirdPartyFlatFileDetail GetThirdPartyFFAccountSaveStatus(int thirdPartyID, int companyAccountID, DateTime inputDate, int formatId)
        {
            ThirdPartyFlatFileDetail tPFFDetail = new ThirdPartyFlatFileDetail();

            Object[] parameter = new object[4];
            parameter[0] = thirdPartyID;
            parameter[1] = companyAccountID;
            DateTime tradeDate = Convert.ToDateTime(inputDate);
            parameter[2] = tradeDate;
            parameter[3] = formatId;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_FFGetThirdPartyFundStatus", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        tPFFDetail = FillTPAccountStatus(row, 0);
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

            }
            #endregion Catch
            return tPFFDetail;

        }

        /// <summary>
        /// The method is used to fill the instance of 'ThirdPartyFlatFileDetail' with the status details of the selected Third party Account.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static ThirdPartyFlatFileDetail FillTPAccountStatus(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            ThirdPartyFlatFileDetail thirdPartyFlatFileDetail = new ThirdPartyFlatFileDetail();
            try
            {
                if (row != null)
                {

                    int THIRDPARTYFFRUNID = offset + 0;
                    int COMPANYTHIRDPARTYID = offset + 1;
                    int DATETIME = offset + 2;
                    int STATUSID = offset + 3;
                    int STATUS = offset + 4;
                    int COMPANYFUNDID = offset + 5;


                    thirdPartyFlatFileDetail.ThirdPartyFFRunID = Convert.ToInt32(row[THIRDPARTYFFRUNID]);
                    thirdPartyFlatFileDetail.ThirdPartyID = Convert.ToInt32(row[COMPANYTHIRDPARTYID]);
                    thirdPartyFlatFileDetail.TradeDate = Convert.ToString(row[DATETIME]);
                    thirdPartyFlatFileDetail.StatusID = Convert.ToInt32(row[STATUSID]);
                    thirdPartyFlatFileDetail.Status = Convert.ToString(row[STATUS]);
                    thirdPartyFlatFileDetail.CompanyAccountID = Convert.ToInt32(row[COMPANYFUNDID]);

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
            return thirdPartyFlatFileDetail;
        }

        /// <summary>
        /// The method is used to fetch the saved data for the selected Third Party Accounts.
        /// </summary>
        /// <param name="thirdPartyID"></param>
        /// <param name="thirdPartyAccountID"></param>
        /// <param name="inputDate"></param>
        /// <returns></returns>
        public static ThirdPartyFlatFileDetailCollection GetFFThirdPartyAccountSavedDetails(int thirdPartyID, int thirdPartyAccountID, DateTime inputDate, int formatId, int companyID, StringBuilder auecIds)
        {
            ThirdPartyFlatFileDetailCollection thirdPartyFlatFileDetailCollection = new ThirdPartyFlatFileDetailCollection();

            Object[] parameter = new object[6];
            parameter[0] = thirdPartyID;
            parameter[1] = thirdPartyAccountID;
            parameter[2] = inputDate.ToString("yyyy/MM/dd");
            parameter[3] = formatId;
            parameter[4] = companyID;
            parameter[5] = auecIds.ToString();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_FFGetThirdPartyFundSavedStatus", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        thirdPartyFlatFileDetailCollection.AddItem(FillTPAccountsSavedDetails(row, 0));
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion Catch
            return thirdPartyFlatFileDetailCollection;
        }

        /// <summary>
        /// The method is used to fill an instance of 'ThirdPartyFlatFileDetail' with the fresh data of the selected account.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static ThirdPartyFlatFileDetail FillThirdPartyFlatFileDetail(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            ThirdPartyFlatFileDetail thirdPartyFlatFileDetail = new ThirdPartyFlatFileDetail();

            if (row != null)
            {
                int ENTITYID = offset + 0;
                int COMPANYFUNDID = offset + 1;
                int ORDERTYPEID = offset + 2;
                int ORDERTYPE = offset + 3;
                int SIDEID = offset + 4;
                int SIDE = offset + 5;
                int SYMBOL = offset + 6;
                int COUNTERPARTYID = offset + 7;
                int VENUEID = offset + 8;
                int ORDERQTY = offset + 9;
                int AVERAGEPRICE = offset + 10;
                int EXECUTEDQTY = offset + 11;
                int TOTALQTY = offset + 12;
                int AUECID = offset + 13;
                int ASSETID = offset + 14;
                int UNDERLYINGID = offset + 15;
                int EXCHANGEID = offset + 16;
                int CURRENCYID = offset + 17;
                int CURRENCYNAME = offset + 18;
                int CURRENCYSYMBOL = offset + 19;
                int FUNDMAPPEDNAME = offset + 20;
                int FUNDACCOUNT = offset + 21;
                int FUNDTYPEID = offset + 22;
                int FUNDTYPE = offset + 23;
                int TRADEREFID = offset + 24;
                int PERCENTAGE = offset + 25;
                int ALLOCATEDQTY = offset + 26;
                int LISTID = offset + 27;// IsBasketGroup                                        
                int PUTORCALL = offset + 28;
                int STRIKEPRICE = offset + 29;
                int EXPIRATIONDATE = offset + 30;
                int SETTLEMENTDATE = offset + 31;
                int COMMISSION = offset + 32;
                int OTHERBROKERFEES = offset + 33;
                int COMPANYTHIRDPARTYTYPEID = offset + 34;
                int COMPANYTHIRDPARTYTYPE = offset + 35;
                int COMPIDENTIFIER = offset + 36;
                int SecFees = offset + 37;
                int CVName = offset + 38;
                int CVIdentifier = offset + 39;
                int CompanyCounterPartyCVID = offset + 40;
                int UniqueIDForPM = offset + 41;
                int TaxLotState = offset + 42;
                int StampDuty = offset + 43;
                int TransactionLevy = offset + 44;
                int ClearingFees = offset + 45;
                int TaxOnCommissions = offset + 46;
                int MiscFees = offset + 47;
                int TradeDate = offset + 48;
                int AssetMultiplier = offset + 49;
                int StrategyID = offset + 50;
                int ISIN = offset + 51;
                int CUSIP = offset + 52;
                int SEDOL = offset + 53;
                int RIC = offset + 54;
                int BBCode = offset + 55;
                int FullSecurityName = offset + 56;
                int UnderlyingSymbol = offset + 57;
                int LeadCurrencyId = offset + 58;
                int LeadCurrencyName = offset + 59;
                int VsCurrencyId = offset + 60;
                int VsCurrencyName = offset + 61;
                int OSISymbol = offset + 62;
                int IDCOSymbol = offset + 63;
                int OpraSymbol = offset + 64;
                int ForexRate_Trade = offset + 65;
                int FXConversionMethodOperator_Trade = offset + 66;
                int FromDeleted = offset + 67;

                int ProcessDate = offset + 68;
                int OriginalPurchaseDate = offset + 69;
                int AccruedInterest = offset + 70;
                // thses 2 fields Comment1 and Comment2 are reserved for Future, if sometimes we need to fetch data from DBthen we can use them
                int Comment1 = offset + 71;
                int Comment2 = offset + 72;
                //[RG : 07252012] Third Party FixedIncome and Swap Handling.
                int Coupon = offset + 73;
                int IssueDate = offset + 74;
                int FirstCouponDate = offset + 75;
                int CouponFrequencyID = offset + 76;
                int AccrualBasisID = offset + 77;
                //int BondTypeID = offset + 80;
                int BenchMarkRate = offset + 79;
                int Differential = offset + 80;
                int SwapDescription = offset + 81;
                int DayCount = offset + 82;
                int FirstResetDate = offset + 83;
                int IsSwapped = offset + 84;
                int Country = offset + 85;
                int RerateDateBusDayAdjusted1 = offset + 86;
                int RerateDateBusDayAdjusted2 = offset + 87;
                int FXRate_Taxlot = offset + 88;
                int FXConversionMethodOperator_Taxlot = offset + 89;

                int LotID = offset + 90;
                int ExternalTransID = offset + 91;
                int TradeAttribute1 = offset + 92;
                int TradeAttribute2 = offset + 93;
                int TradeAttribute3 = offset + 94;
                int TradeAttribute4 = offset + 95;
                int TradeAttribute5 = offset + 96;
                int TradeAttribute6 = offset + 97;
                //UDA
                int UDAAssetName = offset + 98;
                int UDASecurityTypeName = offset + 99;
                int UDASectorName = offset + 100;
                int UDASubSectorName = offset + 101;
                int UDACountryName = offset + 102;

                int Description = offset + 103;
                int DeliveryDate = offset + 104;
                int SecFee = offset + 105;
                int OccFee = offset + 106;
                int OrfFee = offset + 107;
                int CLEARINGBROKERFEE = offset + 108;
                int SOFTCOMMISSION = offset + 109;

                int TransactionType = offset + 110;
                int SettlCurrency = offset + 111;
                int ChangeType = offset + 112;
                int Analyst = offset + 113;
                int CountryOfRisk = offset + 114;
                int CustomUDA1 = offset + 115;
                int CustomUDA2 = offset + 116;
                int CustomUDA3 = offset + 117;
                int CustomUDA4 = offset + 118;
                int CustomUDA5 = offset + 119;
                int CustomUDA6 = offset + 120;
                int CustomUDA7 = offset + 121;
                int Issuer = offset + 122;
                int LiquidTag = offset + 123;
                int MarketCap = offset + 124;
                int Region = offset + 125;
                int RiskCurrency = offset + 126;
                int UCITSEligibleTag = offset + 127;
                int VWAP = offset + 128;

                try
                {
                    #region FillData
                    if (row[ENTITYID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.EntityID = Convert.ToString(row[ENTITYID]);
                    }
                    if (row[COMPANYFUNDID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CompanyAccountID = Convert.ToInt32(row[COMPANYFUNDID]);
                    }
                    if (row[ORDERTYPEID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.OrderTypeTag = Convert.ToInt32(row[ORDERTYPEID]);
                    }
                    if (row[ORDERTYPE] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.OrderType = Convert.ToString(row[ORDERTYPE]);
                    }
                    if (row[SIDEID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.SideTag = Convert.ToString(row[SIDEID]);
                    }
                    if (row[SIDE] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.Side = Convert.ToString(row[SIDE]);
                    }
                    if (row[SYMBOL] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.Symbol = Convert.ToString(row[SYMBOL]);
                    }
                    if (row[COUNTERPARTYID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CounterPartyID = Convert.ToInt32(row[COUNTERPARTYID]);
                    }
                    if (row[VENUEID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.VenueID = Convert.ToInt32(row[VENUEID]);
                    }
                    if (row[ORDERQTY] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.OrderQty = Convert.ToDouble(row[ORDERQTY]);
                    }
                    if (row[AVERAGEPRICE] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.AveragePrice = Convert.ToDouble(row[AVERAGEPRICE]);
                    }
                    if (row[EXECUTEDQTY] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.ExecutedQty = Convert.ToDouble(row[EXECUTEDQTY]);
                    }
                    if (row[TOTALQTY] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.TotalQty = Convert.ToDouble(row[TOTALQTY]);
                    }
                    if (row[AUECID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.AUECID = Convert.ToInt32(row[AUECID]);
                    }
                    if (row[ASSETID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.AssetID = Convert.ToInt32(row[ASSETID]);
                        thirdPartyFlatFileDetail.Asset = Enum.Parse(typeof(AssetCategory), thirdPartyFlatFileDetail.AssetID.ToString(), true).ToString();
                    }
                    if (row[UNDERLYINGID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.UnderLyingID = Convert.ToInt32(row[UNDERLYINGID]);
                    }
                    if (row[EXCHANGEID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.ExchangeID = Convert.ToInt32(row[EXCHANGEID]);
                    }
                    if (row[CURRENCYID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CurrencyID = Convert.ToInt32(row[CURRENCYID]);
                    }
                    if (row[CURRENCYNAME] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CurrencyName = Convert.ToString(row[CURRENCYNAME]);
                    }
                    if (row[CURRENCYSYMBOL] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CurrencySymbol = Convert.ToString(row[CURRENCYSYMBOL]);// added by sandeep as on 03-oct-2007
                    }
                    if (row[FUNDMAPPEDNAME] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.AccountMappedName = Convert.ToString(row[FUNDMAPPEDNAME]);
                    }
                    if (row[FUNDACCOUNT] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.AccountNo = Convert.ToString(row[FUNDACCOUNT]);
                    }
                    if (row[FUNDTYPEID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CompanyAccountTypeID = Convert.ToInt32(row[FUNDTYPEID]);
                    }
                    if (row[FUNDTYPE] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CompanyAccountType = Convert.ToString(row[FUNDTYPE]);
                    }
                    if (row[TRADEREFID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.TradeRefID = row[TRADEREFID].ToString();
                    }
                    if (row[PERCENTAGE] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.Percentage = Convert.ToDouble(row[PERCENTAGE]);
                    }
                    if (row[ALLOCATEDQTY] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.AllocatedQty = Convert.ToDouble(row[ALLOCATEDQTY]);
                    }
                    if (row[LISTID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.ListID = Convert.ToString(row[LISTID]);
                    }
                    if (row[PUTORCALL] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.PutOrCall = Convert.ToString(row[PUTORCALL]);//by Sandeep on 28-sept-2007
                    }
                    if (row[STRIKEPRICE] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.StrikePrice = Convert.ToDouble(row[STRIKEPRICE]);//by sandeep on 04-10-2007
                    }
                    if (row[EXPIRATIONDATE] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.ExpirationDate = Convert.ToString(row[EXPIRATIONDATE]);//by sandeep on 04-10-2007
                    }
                    if (row[SETTLEMENTDATE] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.SettlementDate = Convert.ToString(row[SETTLEMENTDATE]);//by sandeep on 04-10-2007
                    }
                    if (row[COMMISSION] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CommissionCharged = Convert.ToDouble(row[COMMISSION]);//by sandeep on 30-10-2007
                    }
                    if (row[SOFTCOMMISSION] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.SoftCommissionCharged = Convert.ToDouble(row[SOFTCOMMISSION]);
                    }
                    if (row[OTHERBROKERFEES] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.OtherBrokerFee = Convert.ToDouble(row[OTHERBROKERFEES]);//by sandeep on 30-10-2007
                    }
                    if (row[CLEARINGBROKERFEE] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.ClearingBrokerFee = Convert.ToDouble(row[CLEARINGBROKERFEE]);
                    }
                    if (row[COMPANYTHIRDPARTYTYPEID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.ThirdPartyTypeID = Convert.ToInt32(row[COMPANYTHIRDPARTYTYPEID]);//by sandeep on 06-Nov-2007
                    }
                    if (row[COMPANYTHIRDPARTYTYPE] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.ThirdPartyType = Convert.ToString(row[COMPANYTHIRDPARTYTYPE]);//by sandeep on 06-Nov-2007
                    }
                    if (row[COMPIDENTIFIER] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CompanyIdentifier = Convert.ToString(row[COMPIDENTIFIER]);//by sandeep on 06-Nov-2007
                    }
                    if (row[SecFees] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.SecFees = Convert.ToDouble(row[SecFees]);//by sandeep on 19-Nov-2007
                    }
                    if (row[CVName] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CVName = Convert.ToString(row[CVName]);//by sandeep on 19-Nov-2007
                    }
                    if (row[CVIdentifier] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CVIdentifier = Convert.ToString(row[CVIdentifier]);//by sandeep on 19-Nov-2007
                    }
                    if (row[CompanyCounterPartyCVID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CompanyCVID = Convert.ToInt32(row[CompanyCounterPartyCVID]);//by sandeep on 19-Nov-2007
                    }
                    if (row[UniqueIDForPM] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.PBUniqueID = Convert.ToInt64(row[UniqueIDForPM].ToString());//by sandeep on 04-July-2008                       
                    }
                    if (row[TaxLotState] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.TaxLotStateID = Convert.ToInt32(row[TaxLotState].ToString());//by sandeep on 08-Aug-2008  
                        thirdPartyFlatFileDetail.TaxLotState = (PranaTaxLotState)Convert.ToInt64(row[TaxLotState].ToString());//by sandeep on 04-July-2008                        
                    }
                    if (row[StampDuty] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.StampDuty = Convert.ToDouble(row[StampDuty]);
                    }
                    if (row[TransactionLevy] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.TransactionLevy = Convert.ToDouble(row[TransactionLevy]);
                    }
                    if (row[ClearingFees] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.ClearingFee = Convert.ToDouble(row[ClearingFees]);
                    }
                    if (row[TaxOnCommissions] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.TaxOnCommissions = Convert.ToDouble(row[TaxOnCommissions]);
                    }
                    if (row[MiscFees] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.MiscFees = Convert.ToDouble(row[MiscFees]);
                    }
                    if (row[SecFee] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.SecFee = Convert.ToDouble(row[SecFee]);
                    }
                    if (row[OccFee] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.OccFee = Convert.ToDouble(row[OccFee]);
                    }
                    if (row[OrfFee] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.OrfFee = Convert.ToDouble(row[OrfFee]);
                    }
                    if (row[TradeDate] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.TradeDate = row[TradeDate].ToString();
                        thirdPartyFlatFileDetail.TradeDateTime = row[TradeDate].ToString();
                    }
                    thirdPartyFlatFileDetail.AssetMultiplier = row[AssetMultiplier] != DBNull.Value ? Convert.ToDouble(row[AssetMultiplier]) : 1;

                    if (row[StrategyID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.StrategyID = Convert.ToInt32(row[StrategyID].ToString());
                    }
                    if (row[ISIN] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.ISIN = row[ISIN].ToString();
                    }
                    if (row[CUSIP] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CUSIP = row[CUSIP].ToString();
                    }
                    if (row[SEDOL] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.SEDOL = row[SEDOL].ToString();
                    }
                    if (row[RIC] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.RIC = row[RIC].ToString();
                    }
                    if (row[BBCode] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.BBCode = row[BBCode].ToString();
                    }
                    if (row[FullSecurityName] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.FullSecurityName = row[FullSecurityName].ToString();
                    }
                    if (row[UnderlyingSymbol] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.UnderlyingSymbol = row[UnderlyingSymbol].ToString();
                    }
                    if (row[LeadCurrencyId] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.LeadCurrencyID = Convert.ToInt32(row[LeadCurrencyId].ToString());
                    }
                    if (row[LeadCurrencyName] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.LeadCurrencyName = row[LeadCurrencyName].ToString();
                    }
                    if (row[VsCurrencyId] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.VsCurrencyID = Convert.ToInt32(row[VsCurrencyId].ToString());
                    }
                    if (row[VsCurrencyName] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.VsCurrencyName = row[VsCurrencyName].ToString();
                    }
                    if (row[OSISymbol] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.OSIOptionSymbol = row[OSISymbol].ToString();
                    }
                    if (row[IDCOSymbol] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.IDCOOptionSymbol = row[IDCOSymbol].ToString();
                    }
                    if (row[OpraSymbol] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.OpraOptionSymbol = row[OpraSymbol].ToString();
                    }
                    if (row[ForexRate_Trade] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.ForexRate_Trade = Convert.ToDouble(row[ForexRate_Trade].ToString());
                    }
                    if (row[FXConversionMethodOperator_Trade] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.FXConversionMethodOperator_Trade = row[FXConversionMethodOperator_Trade].ToString();
                    }
                    if (row[FromDeleted] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.FromDeleted = row[FromDeleted].ToString();
                    }

                    if (row[ProcessDate] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.ProcessDate = row[ProcessDate].ToString();
                    }
                    if (row[OriginalPurchaseDate] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.OriginalPurchaseDate = row[OriginalPurchaseDate].ToString();
                    }
                    if (row[AccruedInterest] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.AccruedInterest = Convert.ToDouble(row[AccruedInterest]);
                    }
                    if (row[Comment1] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.Comment1 = row[Comment1].ToString();
                    }
                    if (row[Comment2] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.Comment2 = row[Comment2].ToString();
                    }
                    #endregion

                    if (row[Coupon] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.Coupon = double.Parse(row[Coupon].ToString());
                    }
                    if (row[IssueDate] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.IssueDate = row[IssueDate].ToString();
                    }

                    if (row[FirstCouponDate] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.FirstCouponDate = row[FirstCouponDate].ToString();
                    }
                    if (row[CouponFrequencyID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CouponFrequencyID = int.Parse(row[CouponFrequencyID].ToString());
                        thirdPartyFlatFileDetail.Frequency = (CouponFrequency)thirdPartyFlatFileDetail.CouponFrequencyID;
                    }
                    if (row[AccrualBasisID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.AccrualBasisID = int.Parse(row[AccrualBasisID].ToString());
                        thirdPartyFlatFileDetail.AccrualBasis = (AccrualBasis)(row[AccrualBasisID]);
                    }
                    if (row[BenchMarkRate] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.BenchMarkRate = double.Parse(row[BenchMarkRate].ToString());

                    }
                    if (row[Differential] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.Differential = double.Parse(row[Differential].ToString());

                    }
                    if (row[SwapDescription] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.SwapDescription = row[SwapDescription].ToString();

                    }
                    if (row[DayCount] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.DayCount = int.Parse(row[DayCount].ToString());

                    }
                    if (row[FirstResetDate] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.FirstResetDate = row[FirstResetDate].ToString();
                    }
                    if (row[IsSwapped] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.IsSwapped = bool.Parse(row[IsSwapped].ToString());
                    }
                    if (row[Country] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CountryName = row[Country].ToString();
                    }

                    if (row[RerateDateBusDayAdjusted1] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.RerateDateBusDayAdjusted1 = row[RerateDateBusDayAdjusted1].ToString();
                    }

                    if (row[RerateDateBusDayAdjusted2] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.RerateDateBusDayAdjusted2 = row[RerateDateBusDayAdjusted2].ToString();
                    }

                    if (row[FXRate_Taxlot] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.FXRate_Taxlot = double.Parse(row[FXRate_Taxlot].ToString());
                    }

                    if (row[FXConversionMethodOperator_Taxlot] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.FXConversionMethodOperator_Taxlot = row[FXConversionMethodOperator_Taxlot].ToString();
                    }


                    if (row[LotID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.LotId = row[LotID].ToString();
                    }
                    if (row[ExternalTransID] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.ExternalTransId = row[ExternalTransID].ToString();
                    }
                    if (row[TradeAttribute1] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.TradeAttribute1 = row[TradeAttribute1].ToString();
                    }
                    if (row[TradeAttribute2] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.TradeAttribute2 = row[TradeAttribute2].ToString();
                    }
                    if (row[TradeAttribute3] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.TradeAttribute3 = row[TradeAttribute3].ToString();
                    }
                    if (row[TradeAttribute4] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.TradeAttribute4 = row[TradeAttribute4].ToString();
                    }
                    if (row[TradeAttribute5] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.TradeAttribute5 = row[TradeAttribute5].ToString();
                    }
                    if (row[TradeAttribute6] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.TradeAttribute6 = row[TradeAttribute6].ToString();
                    }

                    //UDA

                    if (row[UDAAssetName] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.UDAAssetName = row[UDAAssetName].ToString();
                    }
                    if (row[UDASecurityTypeName] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.UDASecurityTypeName = row[UDASecurityTypeName].ToString();
                    }
                    if (row[UDASectorName] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.UDASectorName = row[UDASectorName].ToString();
                    }
                    if (row[UDASubSectorName] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.UDASubSectorName = row[UDASubSectorName].ToString();
                    }
                    if (row[UDACountryName] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.UDACountryName = row[UDACountryName].ToString();
                    }

                    if (row[Description] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.Description = row[Description].ToString();
                    }

                    if (row[DeliveryDate] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.DeliveryDate = row[DeliveryDate].ToString();
                    }

                    if (row[TransactionType] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.TransactionType = row[TransactionType].ToString();
                    }

                    if (row[SettlCurrency] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.SettlCurrency = row[SettlCurrency].ToString();
                    }

                    if (row[ChangeType] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.ChangeType = (ChangeType)(row[ChangeType]);
                    }
                    if (row[Analyst] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.Analyst = row[Analyst].ToString();
                    }
                    if (row[CountryOfRisk] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CountryOfRisk = row[CountryOfRisk].ToString();
                    }
                    if (row[CustomUDA1] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CustomUDA1 = row[CustomUDA1].ToString();
                    }
                    if (row[CustomUDA2] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CustomUDA2 = row[CustomUDA2].ToString();
                    }
                    if (row[CustomUDA3] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CustomUDA3 = row[CustomUDA3].ToString();
                    }
                    if (row[CustomUDA4] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CustomUDA4 = row[CustomUDA4].ToString();
                    }
                    if (row[CustomUDA5] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CustomUDA5 = row[CustomUDA5].ToString();
                    }
                    if (row[CustomUDA6] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CustomUDA6 = row[CustomUDA6].ToString();
                    }
                    if (row[CustomUDA7] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.CustomUDA7 = row[CustomUDA7].ToString();
                    }
                    if (row[Issuer] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.Issuer = row[Issuer].ToString();
                    }
                    if (row[LiquidTag] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.LiquidTag = row[LiquidTag].ToString();
                    }
                    if (row[MarketCap] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.MarketCap = row[MarketCap].ToString();
                    }
                    if (row[Region] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.Region = row[Region].ToString();
                    }
                    if (row[RiskCurrency] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.RiskCurrency = row[RiskCurrency].ToString();
                    }
                    if (row[UCITSEligibleTag] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.UCITSEligibleTag = row[UCITSEligibleTag].ToString();
                    }
                    if (row[VWAP] != System.DBNull.Value)
                    {
                        thirdPartyFlatFileDetail.VWAP = row[VWAP].ToString();
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

            }
            return thirdPartyFlatFileDetail;

            #region backup
            //if (offset < 0)
            //{
            //    offset = 0;
            //}

            //ThirdPartyFlatFileDetail thirdPartyFlatFileDetail = new ThirdPartyFlatFileDetail();

            //if (row != null)
            //{
            //    int TRADEREFID = offset + 0;
            //    int COMPANYID = offset + 1;
            //    int THIRDPARTYID = offset + 2;
            //    int THIRDPARTY = offset + 3;
            //    int SIDEID = offset + 4;
            //    int SIDE = offset + 5;
            //    int SYMBOL = offset + 6;
            //    int EXECUTEDQTY = offset + 7;
            //    int AVERAGEPRICE = offset + 8;
            //    int COMPANYFUNDID = offset + 9;
            //    int FUNDNAME = offset + 10;
            //    int FUNDMAPPEDNAME = offset + 11;
            //    int FUNDACCOUNT = offset + 12;
            //    int ORDERTYPEID = offset + 13;
            //    int ORDERTYPE = offset + 14;
            //    int ASSETID = offset + 15;
            //    int ASSET = offset + 16;
            //    int UNDERLYINGID = offset + 17;
            //    int UNDERLYING = offset + 18;
            //    int EXCHANGEID = offset + 19;
            //    int EXCHANGE = offset + 20;
            //    int CURRENCYID = offset + 21;
            //    int CURRENCY = offset + 22;
            //    int AUECID = offset + 23;
            //    int SECFEES = offset + 24;
            //    int COMMISSION = offset + 25;
            //    int COMMISSIONCHARGED = offset + 26;
            //    int GROSSAMOUNT = offset + 27;
            //    int NETAMOUNT = offset + 28;
            //    int SECURITYIDTYPE = offset + 29;
            //    int COMMISSIONRATETYPEID = offset + 30;
            //    int COMMISSIONRATETYPE = offset + 31;
            //    int TRADEDATE = offset + 32;
            //    int SAVEPATH = offset + 33;
            //    int NAMINGCONVENTION = offset + 34;
            //    int VENUEID = offset + 35;
            //    int VENUENAME = offset + 36;
            //    int CVIDENTIFIER = offset + 37;
            //    int FUNDTYPEID = offset + 38;
            //    int FUNDTYPE = offset + 39;
            //    int THIRDPARTYTYPEID = offset + 40;
            //    int THIRDPARTYTYPE = offset + 41;
            //    int COMPANYIDENTIFIER = offset + 42;
            //    int PERCENTAGE = offset + 43;


            //    thirdPartyFlatFileDetail.TradeRefID = Convert.ToInt32(row[TRADEREFID]);
            //    thirdPartyFlatFileDetail.CompanyID = Convert.ToInt32(row[COMPANYID]);
            //    thirdPartyFlatFileDetail.ThirdPartyID = Convert.ToInt32(row[THIRDPARTYID]);
            //    thirdPartyFlatFileDetail.ThirdParty = Convert.ToString(row[THIRDPARTY]);
            //    thirdPartyFlatFileDetail.SideTag = Convert.ToString(row[SIDEID]);
            //    thirdPartyFlatFileDetail.Side = Convert.ToString(row[SIDE]);
            //    thirdPartyFlatFileDetail.Symbol = Convert.ToString(row[SYMBOL]);
            //    thirdPartyFlatFileDetail.ExecutedQty = Convert.ToInt32(row[EXECUTEDQTY]);
            //    thirdPartyFlatFileDetail.AveragePrice = Convert.ToDouble(row[AVERAGEPRICE]);
            //    thirdPartyFlatFileDetail.CompanyAccountID = Convert.ToInt32(row[COMPANYFUNDID]);
            //    thirdPartyFlatFileDetail.AccountName = Convert.ToString(row[FUNDNAME]);
            //    thirdPartyFlatFileDetail.AccountMappedName = Convert.ToString(row[FUNDMAPPEDNAME]);
            //    thirdPartyFlatFileDetail.AccountNo = Convert.ToString(row[FUNDACCOUNT]);
            //    thirdPartyFlatFileDetail.OrderTypeTag = Convert.ToString(row[ORDERTYPEID]);
            //    thirdPartyFlatFileDetail.OrderType = Convert.ToString(row[ORDERTYPE]);
            //    thirdPartyFlatFileDetail.AssetID = Convert.ToInt32(row[ASSETID]);
            //    thirdPartyFlatFileDetail.Asset = Convert.ToString(row[ASSET]);
            //    thirdPartyFlatFileDetail.UnderLyingID = Convert.ToInt32(row[UNDERLYINGID]);
            //    thirdPartyFlatFileDetail.UnderLying = Convert.ToString(row[UNDERLYING]);
            //    thirdPartyFlatFileDetail.CurrencyID = Convert.ToInt32(row[CURRENCYID]);
            //    thirdPartyFlatFileDetail.CurrencyName = Convert.ToString(row[CURRENCY]);
            //    thirdPartyFlatFileDetail.AUECID = Convert.ToInt32(row[AUECID]);
            //    thirdPartyFlatFileDetail.Commission = Convert.ToDouble(row[COMMISSION]);
            //    thirdPartyFlatFileDetail.CommissionCharged = Convert.ToDouble(row[COMMISSIONCHARGED]);
            //    thirdPartyFlatFileDetail.GrossAmount = Convert.ToDouble(row[GROSSAMOUNT]);
            //    thirdPartyFlatFileDetail.NetAmount = Convert.ToDouble(row[NETAMOUNT]);
            //    thirdPartyFlatFileDetail.SecurityIDType = Convert.ToString(row[SECURITYIDTYPE]);
            //    thirdPartyFlatFileDetail.CommissionRateTypeID = Convert.ToInt32(row[COMMISSIONRATETYPEID]);
            //    thirdPartyFlatFileDetail.CommissionRateType = Convert.ToString(row[COMMISSIONRATETYPE]);
            //    thirdPartyFlatFileDetail.TradeDate = Convert.ToDateTime(row[TRADEDATE]);
            //    thirdPartyFlatFileDetail.NamingConvention = Convert.ToString(row[NAMINGCONVENTION]);
            //    thirdPartyFlatFileDetail.SavePath = Convert.ToString(row[SAVEPATH]);
            //    thirdPartyFlatFileDetail.VenueID = Convert.ToInt32(row[VENUEID]);
            //    thirdPartyFlatFileDetail.VenueName = Convert.ToString(row[VENUENAME]);
            //    thirdPartyFlatFileDetail.CVIdentifier = Convert.ToString(row[CVIDENTIFIER]);
            //    thirdPartyFlatFileDetail.CompanyAccountTypeID = Convert.ToInt32(row[FUNDTYPEID]);
            //    thirdPartyFlatFileDetail.CompanyAccountType = Convert.ToString(row[FUNDTYPE]);
            //    thirdPartyFlatFileDetail.ThirdPartyTypeID = Convert.ToInt32(row[THIRDPARTYTYPEID]);
            //    thirdPartyFlatFileDetail.ThirdPartyType = Convert.ToString(row[THIRDPARTYTYPE]);
            //    thirdPartyFlatFileDetail.CompanyIdentifier = Convert.ToString(row[COMPANYIDENTIFIER]);
            //    thirdPartyFlatFileDetail.Percentage = Convert.ToDouble(row[PERCENTAGE]);
            //}
            //return thirdPartyFlatFileDetail;
            #endregion backup
        }

        /// <summary>
        /// To save the XML containing the FlatFile details of the selected accounts of a third party to the database.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_thirdPartyID"></param>
        /// <param name="selectedaccountIDlist"></param>
        /// <param name="tradeDate"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int SaveThirdPartyAccountDetailsXML(int _companyID, int _thirdPartyID, string selectedaccountIDlist, DateTime tradeDate, string xml, int statusID, int userID, string filePath, int FormatId)
        {
            int result = int.MinValue;
            try
            {
                object[] parameter = new object[9];

                parameter[0] = _companyID;
                parameter[1] = _thirdPartyID;
                parameter[2] = selectedaccountIDlist;
                parameter[3] = Convert.ToDateTime(tradeDate);
                parameter[4] = xml;
                parameter[5] = statusID;
                parameter[6] = userID;
                parameter[7] = filePath;
                parameter[8] = FormatId;


                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteNonQuery("P_FFSaveThirdPartyDetailXML", parameter).ToString());
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
            finally
            {
                if (result > 0)
                {
                    //IsSaved = true;
                }
            }
            return result;
        }

        public static int SaveThirdPartyDetails(int thirdPartyID, DateTime timeOfSave, string FileName, Int64 FileId, string taxLotIds)
        {
            int result = int.MinValue;
            try
            {
                object[] parameter = new object[5];

                parameter[0] = thirdPartyID;
                parameter[1] = timeOfSave;
                parameter[2] = FileName;
                parameter[3] = FileId;
                parameter[4] = taxLotIds;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveThirdPartyDetails", parameter).ToString());
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


        public static int UpdateTaxlotState(int thirdpartyID, int fileFormatID, string taxLotIDs, string deletedTaxLotIDs, bool GenerateCancelNewForAmend = false)
        {
            int result = int.MinValue;
            try
            {
                object[] parameter = new object[5];
                parameter[0] = thirdpartyID;
                parameter[1] = fileFormatID;
                parameter[2] = taxLotIDs;
                parameter[3] = deletedTaxLotIDs;
                parameter[4] = GenerateCancelNewForAmend;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteNonQuery("P_UpdateTaxlotState", parameter).ToString());
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

        public static int InsertPBWiseTaxlotState(int thirdpartyID, int fileFormatID, string taxLotIDs)
        {
            int result = int.MinValue;
            try
            {
                object[] parameter = new object[3];
                parameter[0] = thirdpartyID;
                parameter[1] = fileFormatID;
                parameter[2] = taxLotIDs;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SavePBWiseTaxlotState", parameter).ToString());
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

        public static int UpdateTaxlotsToIgnoreState(int thirdPartyID, string taxLotIDs)
        {
            int result = int.MinValue;
            try
            {
                object[] parameter = new object[2];
                parameter[0] = thirdPartyID;
                parameter[1] = taxLotIDs;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteNonQuery("P_UpdateTaxlotsToIgnoreState", parameter).ToString());
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

        /// <summary>
        /// The method is used to check whether there are any unallocated trades or not.
        /// </summary>
        /// <returns></returns>
        public static bool CheckForUnallocatedTrades()
        {
            bool IsUnallocatedTrades = false;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_FFGetUnallocatedTradesIfAny";

            try
            {
                int result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar(queryData).ToString());
                if (result > 0)
                {
                    IsUnallocatedTrades = true;
                }
                else
                {
                    IsUnallocatedTrades = false;
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
            return IsUnallocatedTrades;
        }

        // This method is used to check whether there are any unallocated trades on datewise and userwise

        public static bool CheckForUnallocatedTrades(DateTime tradedate)
        {
            bool isUnallocatedTaxlots = false;
            try
            {
                //string AllAUECDatesString = string.Empty;
                //AllAUECDatesString = TimeZoneHelper.GetSameDateForAllAUEC(tradedate);

                object[] param = new object[1];

                param[0] = tradedate;

                object result = DatabaseManager.DatabaseManager.ExecuteScalar("P_GetUnallocatedOrdersCount", param);

                if (result == null || (int)result == 0)// there is no unallocated trade
                {
                    isUnallocatedTaxlots = false;
                }
                else
                {
                    isUnallocatedTaxlots = true;
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
            return isUnallocatedTaxlots;
        }

    }
}
