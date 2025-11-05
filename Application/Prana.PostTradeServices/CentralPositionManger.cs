using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Prana.PostTradeServices
{
    class CentralPositionManger
    {
        static Dictionary<string, List<PranaPosition>> _dictSymbolWisePositions = new Dictionary<string, List<PranaPosition>>();// Dictionary<Symbol, Position>
        static List<string> _listGrouping = new List<string>();
        static readonly object locker = new object();

        #region Public Functions
        public static void Refresh()
        {
            lock (locker)
            {
                PositionDataManager.Refresh();
                CreateSymbolWiseDictionary();
            }
        }
        public static List<PranaPosition> GetGroupedPositionAsRiskPref(List<string> listGrouping)
        {
            _listGrouping = listGrouping;
            lock (locker)
            {
                List<PranaPosition> coll = new List<PranaPosition>();

                try
                {
                    Dictionary<string, PranaPosition> dictFinalGroupPos = new Dictionary<string, PranaPosition>();// Dictionary<CompareKey, Position>

                    foreach (KeyValuePair<string, List<PranaPosition>> symbolListPos in _dictSymbolWisePositions)
                    {
                        List<PranaPosition> listPos = symbolListPos.Value;

                        if (listPos.Count == 1)
                        {   // Grouping effort is not needed
                            coll.Add((PranaPosition)listPos[0].Clone());
                            continue;
                        }

                        foreach (PranaPosition pos in listPos)
                        {
                            string compareKey = GetCompareKey(pos);

                            if (!dictFinalGroupPos.ContainsKey(compareKey))
                            {
                                dictFinalGroupPos.Add(compareKey, (PranaPosition)pos.Clone());
                            }
                            else
                            {
                                PranaPosition targetPos = dictFinalGroupPos[compareKey];
                                GroupPositionAsRiskPref(targetPos, pos);
                            }
                        }
                    }

                    // for which grouping is tried
                    foreach (KeyValuePair<string, PranaPosition> groupPos in dictFinalGroupPos)
                    {
                        coll.Add(groupPos.Value);
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
                #endregion
                return coll;
            }
        }
        #endregion

        #region Private & Help Functions
        private static void CreateSymbolWiseDictionary()
        {
            try
            {
                _dictSymbolWisePositions.Clear();
                List<PranaPosition> positionList = GetOpenPositions();
                foreach (PranaPosition pos in positionList)
                {
                    if (!_dictSymbolWisePositions.ContainsKey(pos.Symbol))
                    {
                        List<PranaPosition> listPos = new List<PranaPosition>();
                        listPos.Add(pos);
                        _dictSymbolWisePositions.Add(pos.Symbol, listPos);
                    }
                    else
                    {
                        _dictSymbolWisePositions[pos.Symbol].Add(pos);
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
        }
        private static List<PranaPosition> GetOpenPositions()
        {
            DataSet ds = PositionDataManager.GetOpenPositions();
            List<PranaPosition> positionList = new List<PranaPosition>();
            try
            {
                if (ds != null)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        PranaPosition pranaPos = new PranaPosition();
                        FillPositionDetails(row, pranaPos);
                        positionList.Add(pranaPos);
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
            #endregion
            return positionList;
        }
        private static void FillPositionDetails(DataRow row, PranaPosition pranaPos)
        {
            try
            {
                FillPositionBasicDetails(row, pranaPos);
                pranaPos.Strategy = row["Level2ID"] != DBNull.Value ? Prana.CommonDataCache.CachedDataManager.GetInstance.GetStrategyText(int.Parse(row["Level2ID"].ToString())) : "Strategy Unallocated";
                pranaPos.Account = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccountText(int.Parse(row["FundID"].ToString()));

                #region Merged UDA data from data row - omshiv, Nov 2013, UDA Merged to Sec Master

                if (row.Table.Columns.Contains("AssetName") && row["AssetName"] != DBNull.Value)
                    //  pranaPos.UDAAsset = row["AssetName"].ToString();

                    if (row.Table.Columns.Contains("SecurityTypeName") && row["SecurityTypeName"] != DBNull.Value)
                        pranaPos.SecurityTypeName = row["SecurityTypeName"].ToString();

                if (row.Table.Columns.Contains("SectorName") && row["SectorName"] != DBNull.Value)
                    pranaPos.SectorName = row["SectorName"].ToString();

                if (row.Table.Columns.Contains("SubSectorName") && row["SubSectorName"] != DBNull.Value)
                    pranaPos.SubSectorName = row["SubSectorName"].ToString();

                if (row.Table.Columns.Contains("CountryName") && row["CountryName"] != DBNull.Value)
                    pranaPos.CountryName = row["CountryName"].ToString();

                #endregion
                //Commmented by om shiv, Nov, 2013, UDA getting directly from DB with secMaster Data
                //Prana.BusinessObjects.SecurityMasterBusinessObjects.UDAData udaData = CachedDataManager.GetInstance.GetUDAData(pranaPos.Symbol);
                //if (udaData != null)
                //{
                //    pranaPos.SecurityTypeName = udaData.UDASecurityType;
                //    pranaPos.SectorName = udaData.UDASector;
                //    pranaPos.SubSectorName = udaData.UDASubSector;
                //    pranaPos.CountryName = udaData.UDACountry;
                //}
                //else
                //{
                //    pranaPos.SecurityTypeName = "Undefined";
                //    pranaPos.CountryName = "Undefined";
                //    pranaPos.SectorName = "Undefined";
                //    pranaPos.SubSectorName = "Undefined";
                //}


                pranaPos.SecurityName = row["CompanyName"].ToString();
                if (row["Multiplier"] != System.DBNull.Value)
                {
                    pranaPos.Multiplier = double.Parse(row["Multiplier"].ToString());
                }
                if (row["Delta"] != System.DBNull.Value)
                {
                    pranaPos.Delta = float.Parse(row["Delta"].ToString());
                }
                pranaPos.PositionType = (pranaPos.Quantity >= 0) ? "Long" : "Short";
                pranaPos.SideMultiplier = (pranaPos.Quantity >= 0) ? 1 : -1;
                pranaPos.StrikePrice = double.Parse(row["StrikePrice"].ToString());
                pranaPos.PutOrCall = (row["PutOrCall"].ToString() != String.Empty) ? row["PutOrCall"].ToString().ToCharArray()[0] : ' ';

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }
        private static void FillPositionBasicDetails(DataRow row, PranaBasicMessage pranaPos)
        {
            try
            {
                pranaPos.Symbol = row["Symbol"].ToString();
                pranaPos.OrderSideTagValue = row["SideID"].ToString().Trim();
                pranaPos.Quantity = double.Parse(row["OpenQuantity"].ToString());
                if (!Prana.CommonDataCache.NameValueFiller.IsLongSide(pranaPos.OrderSideTagValue))
                {
                    pranaPos.Quantity = -pranaPos.Quantity;
                }



                pranaPos.AssetID = int.Parse(row["AssetID"].ToString());
                pranaPos.AssetName = CommonDataCache.CachedDataManager.GetInstance.GetAssetText(pranaPos.AssetID);
                pranaPos.AvgPrice = float.Parse(row["AveragePrice"].ToString());


                if (pranaPos.AssetID == (int)AssetCategory.Equity)
                {
                    pranaPos.UnderlyingSymbol = pranaPos.Symbol;
                }
                else
                {
                    pranaPos.UnderlyingSymbol = row["UnderLyingSymbol"].ToString();
                }
                pranaPos.ExpirationDate = Convert.ToDateTime(row["ExpirationDate"].ToString());
                pranaPos.CurrencyID = int.Parse(row["CurrencyID"].ToString());
                pranaPos.ExchangeName = CachedDataManager.GetInstance.GetExchangeText(int.Parse(row["ExchangeID"].ToString()));
                pranaPos.UnderlyingName = CachedDataManager.GetInstance.GetUnderLyingText(int.Parse(row["UnderlyingID"].ToString()));
                pranaPos.CurrencyName = CachedDataManager.GetInstance.GetCurrencyText(pranaPos.CurrencyID);
                pranaPos.CompanyUserName = CachedDataManager.GetInstance.GetUserText(int.Parse(row["UserID"].ToString()));
                pranaPos.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(int.Parse(row["CounterPartyID"].ToString()));
                //pranaPos.StrikePrice = double.Parse(row["StrikePrice"].ToString());
                //pranaPos.PutOrCall = (row["PutOrCall"].ToString() != String.Empty) ? row["PutOrCall"].ToString().ToCharArray()[0] : ' ';
                pranaPos.AUECLocalDate = DateTime.Parse(row["AUECLocalDate"].ToString());
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        private static void GroupPositionAsRiskPref(PranaPosition targetPos, PranaPosition pos)
        {
            //Avg Price
            if (targetPos.Quantity + pos.Quantity != 0)
            {
                targetPos.AvgPrice = (targetPos.Quantity * targetPos.AvgPrice + pos.Quantity * pos.AvgPrice) / (targetPos.Quantity + pos.Quantity);
            }

            // Quantity
            targetPos.Quantity += pos.Quantity;

            // Position Side
            if (targetPos.Quantity >= 0)
            {
                targetPos.PositionType = "Long";
                targetPos.SideMultiplier = 1;
            }
            else
            {
                targetPos.PositionType = "Short";
                targetPos.SideMultiplier = -1;
            }

            // Account Name
            if (targetPos.Account != pos.Account)
            {
                targetPos.Account = "Multiple";
            }

            // Strategy
            if (targetPos.Strategy != pos.Strategy)
            {
                targetPos.Strategy = "Multiple";
            }

            // AUELocalDate
            if (targetPos.AUECLocalDate != pos.AUECLocalDate)
            {
                targetPos.AUECLocalDate = DateTime.Parse("1/1/1800");
            }

            // CompanyUserName
            if (targetPos.CompanyUserName != pos.CompanyUserName)
            {
                targetPos.CompanyUserName = "Multiple";
            }

            // CounterPartyName
            if (targetPos.CounterPartyName != pos.CounterPartyName)
            {
                targetPos.CounterPartyName = "Multiple";
            }

            // ExpirationDate
            if (targetPos.ExpirationDate != pos.ExpirationDate)
            {
                targetPos.ExpirationDate = DateTime.Parse("1/1/1800");
            }
        }
        private static string GetCompareKey(PranaPosition pos)
        {

            StringBuilder compareKey = new StringBuilder();
            compareKey.Append(pos.Symbol);
            compareKey.Append(pos.PositionType);

            if (_listGrouping.Contains("Account"))
            {
                compareKey.Append(pos.Account);
            }
            if (_listGrouping.Contains("Strategy"))
            {
                compareKey.Append(pos.Strategy);
            }
            return compareKey.ToString();
        }
        #endregion

        private static ISecMasterServices _secMasterServices;
        public static ISecMasterServices SecMasterServices
        {
            set { _secMasterServices = value; }

        }
    }
}
