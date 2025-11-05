using Act40OrderGeneratorTool.Classes;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using Prana.ServiceConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Act40OrderGeneratorTool.Cache
{
    class PositionCache
    {
        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static PositionCache _singiltonObject = null;

        /// <summary>
        /// private cunstructor
        /// </summary>
        private PositionCache()
        {

        }

        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        internal static PositionCache GetInstance()
        {
            lock (_lock)
            {
                if (_singiltonObject == null)
                    _singiltonObject = new PositionCache();
                return _singiltonObject;
            }
        }
        #endregion

        private DataTable _symbolPositionCache = null;

        private DataTable _underlyingPositionCache = null;

        private DataTable _symbolData = null;

        private DataTable _securityData = null;

        private String FUND_NAME_STRING;

        private String CALC_PREF_STRING;

        /// <summary>
        /// Loads the data from esper and applies types to the columns
        /// </summary>
        /// <returns></returns>
        internal Boolean Initialize(ModelPrefrence prefrence, CalculationPreference calcPref)
        {
            try
            {
                // 1st let us get the symbol cache
                // We just need the following from the symbol cache : Account, Symbol, Underlying Symbol, Sector, selectedFeedPrice
                // We need the following at underling level : Account, UnderlyingSymbol, ExposureSide, All fields in FilterFields and ReplacementGroups

                if (calcPref == CalculationPreference.Exposure)
                    CALC_PREF_STRING = "netExposureBase";
                else
                    CALC_PREF_STRING = "betaAdjustedExposureBase";


                List<String> underlyingSymbolCols = new List<String>() { "underlyingExposurePositionSide", "underlyingMarketCapitalization",
                                                                            "deltaAdjPosition", CALC_PREF_STRING ,
                                                                            "customUDA1","customUDA2","customUDA3","customUDA4","customUDA5",
                                                                            "customUDA6","customUDA7"};

                List<String> symbolCols = new List<String>() { "underlyingSymbol" };

                List<String> symbolDataCols = new List<String>() { "selectedFeedPrice", "avgVolume20Days" };

                List<String> securityDataCols = new List<String>() { "udaSector" };

                if (prefrence == ModelPrefrence.Account)
                {
                    _underlyingPositionCache = ComplianceServiceConnector.GetInstance().GetCalculations(Compression.AccountUnderlyingSymbol, underlyingSymbolCols);
                    _symbolPositionCache = ComplianceServiceConnector.GetInstance().GetCalculations(Compression.AccountSymbol, symbolCols);
                    FUND_NAME_STRING = "accountShortName";
                }
                else if (prefrence == ModelPrefrence.MasterFund)
                {
                    _underlyingPositionCache = ComplianceServiceConnector.GetInstance().GetCalculations(Compression.MasterFundUnderlyingSymbol, underlyingSymbolCols);
                    _symbolPositionCache = ComplianceServiceConnector.GetInstance().GetCalculations(Compression.MasterFundSymbol, symbolCols);
                    FUND_NAME_STRING = "masterFundName";
                }

                _symbolData = ComplianceServiceConnector.GetInstance().GetCalculations(Compression.SymbolData, symbolDataCols);
                _securityData = ComplianceServiceConnector.GetInstance().GetCalculations(Compression.SecurityData, securityDataCols);

                if (_underlyingPositionCache == null || _symbolPositionCache == null || _symbolPositionCache == null || _securityData == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns ALL the positions in a account  grouped by underlying. Includes Naked Securities
        /// </summary>
        /// <param name="accountShortName"></param>
        /// <returns></returns>
        internal List<Position> GetUnderlyingPositionsForAccount(String accountShortName)
        {
            try
            {
                if (_symbolPositionCache != null)
                {
                    var filterededRows = _underlyingPositionCache.Select(String.Format("{0} = '{1}' ", FUND_NAME_STRING, accountShortName));
                    if (filterededRows.Count() > 0)
                    {
                        var filterededData = filterededRows.CopyToDataTable();
                        // add symbol level columns to underlying table ie : SelectedFeedPrice, sector, avgVolume20Days
                        filterededData.Columns.Add("selectedFeedPrice");
                        filterededData.Columns.Add("sector");

                        foreach (DataRow dr in filterededData.Rows)
                        {
                            try
                            { dr["sector"] = _securityData.Select(String.Format("tickerSymbol = '{0}'", dr["underlyingSymbol"].ToString()))[0]["udaSector"].ToString(); }
                            catch { }

                            try
                            { dr["selectedFeedPrice"] = _symbolData.Select(String.Format("symbol = '{0}'", dr["underlyingSymbol"].ToString()))[0]["selectedFeedPrice"].ToString(); }
                            catch { }
                        }
                        return RowsToList(filterededData.Select());
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
            return new List<Position>();
        }

        /// <summary>
        /// Includes securities as per the defined rules
        /// </summary>
        /// <param name="accountShortName"></param>
        /// <param name="rule"></param>
        /// <param name="side"></param>
        /// <param name="replacementMatrix"></param>
        /// <param name="includeNakedSecurities"></param>
        /// <returns></returns>
        internal List<Position> GetUnderlyingPositionsFilteredByRule(String accountShortName, Rule rule, Side side, DataTable replacementMatrix, Boolean includeNakedSecurities)
        {
            String filterquery = String.Format("{0} = '{1}' AND underlyingExposurePositionSide = '{2}' ", FUND_NAME_STRING, accountShortName, side.ToString().ToLower());
            var rows = _underlyingPositionCache.Select(filterquery);

            if (rows.Count() > 0)
            {
                DataTable filterededData = rows.CopyToDataTable();
                if (!includeNakedSecurities)
                {
                    // filter out the naked securities
                    foreach (DataRow dr in filterededData.Rows)
                    {
                        if (IsNakedSecurity(dr))
                            dr.Delete();
                    }
                    filterededData.AcceptChanges();
                }

                if (Preference.GetInstance().ExcludedSymbols.Count > 0)
                {
                    // filter out the excluded securities
                    foreach (DataRow dr in filterededData.Rows)
                    {
                        String symbol = dr["underlyingSymbol"].ToString();
                        if (Preference.GetInstance().ExcludedSymbols.Contains(symbol))
                            dr.Delete();
                    }
                    filterededData.AcceptChanges();
                }



                if (filterededData.Rows.Count > 0)
                {
                    // add symbol level columns to underlying table ie : SelectedFeedPrice, sector, avgVolume20Days
                    filterededData.Columns.Add("selectedFeedPrice");
                    filterededData.Columns.Add("sector");
                    filterededData.Columns.Add("avgVolume20Days");

                    // the value for these columns will be picked up in the followin order
                    // 1. From compliance (incase the user has over ridden it)
                    // 2. From pricing serviice and security service
                    // Note : Incase there are no naked securities, all the required info will be available in compliance. Sans the replacement matrix symbol

                    foreach (DataRow dr in filterededData.Rows)
                    {
                        try
                        { dr["sector"] = _securityData.Select(String.Format("tickerSymbol = '{0}'", dr["underlyingSymbol"].ToString()))[0]["udaSector"].ToString(); }
                        catch { }

                        try
                        { dr["selectedFeedPrice"] = _symbolData.Select(String.Format("symbol = '{0}'", dr["underlyingSymbol"].ToString()))[0]["selectedFeedPrice"].ToString(); }
                        catch { }

                        try
                        { dr["avgVolume20Days"] = _symbolData.Select(String.Format("symbol = '{0}'", dr["underlyingSymbol"].ToString()))[0]["avgVolume20Days"].ToString(); }
                        catch { }
                    }

                    filterededData = ApplyDataTypesToColumns(filterededData);

                    // apply filter and sort
                    rows = filterededData.Select(Misc.DataTabeleAsFilterQuery(rule.FilterConditions), Misc.GetEnumDescription(rule.LimitingField) + " DESC");

                    if (rows.Length > 0)
                    {
                        // Apply the replacement Matrix
                        filterededData = rows.CopyToDataTable();
                        foreach (DataRow dr in replacementMatrix.Select(String.Format("Side = '{0}'", side)))
                        {
                            // select where group = '<filterGroup>'
                            foreach (DataRow row in filterededData.Select(String.Format("{0} = '{1}'", dr["Group"], dr["Target"])))
                            {
                                row["underlyingSymbol"] = dr["Replacement Symbol"];

                                if (String.IsNullOrWhiteSpace(dr["Price"].ToString()) || Convert.ToDouble(dr["Price"].ToString()) == 0)
                                    row["selectedFeedPrice"] = PricingServiceConnector.GetInstance().GetPrice(dr["Replacement Symbol"].ToString());
                                else
                                    row["selectedFeedPrice"] = dr["Price"];

                                row["sector"] = SecurityServiceConnector.GetInstance().GetSector(dr["Replacement Symbol"].ToString());
                            }
                        }

                        DataRow[] finalRows = new DataRow[Math.Min(rule.PortfolioLimit, rows.Length)];

                        for (int i = 0; i < finalRows.Length; i++)
                            finalRows[i] = filterededData.Rows[i];

                        return RowsToList(finalRows);
                    }
                }
            }

            return new List<Position>();
        }

        /// <summary>
        /// Returns true if the underlying security is present in the portfolio
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private Boolean IsNakedSecurity(DataRow dr)
        {
            String accountShortName = dr[FUND_NAME_STRING].ToString();
            String symbol = dr["underlyingSymbol"].ToString();
            String filterquery = String.Format("{0} = '{1}' AND symbol = '{2}' ", FUND_NAME_STRING, accountShortName, symbol);
            if (_symbolPositionCache.Select(filterquery).Count() > 0)
                return false;
            return true;
        }

        // This currently assumes that there are only type double FilterFields, If some other field is added it must be taken into account.
        private DataTable ApplyDataTypesToColumns(DataTable table)
        {
            try
            {
                DataTable dt = table.Clone();
                foreach (FilterFields field in Enum.GetValues(typeof(FilterFields)))
                {
                    if (dt.Columns[Misc.GetEnumDescription(field)] != null)
                        dt.Columns[Misc.GetEnumDescription(field)].DataType = typeof(double);
                }

                foreach (DataRow dr in table.Rows)
                {
                    dt.ImportRow(dr);
                }
                dt.AcceptChanges();
                return dt;
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
            return null;
        }

        /// <summary>
        /// Converts the data rows to a list of Positions
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private List<Position> RowsToList(DataRow[] rows)
        {
            try
            {
                List<Position> positions = new List<Position>();
                foreach (DataRow row in rows)
                {
                    String account = row[FUND_NAME_STRING].ToString();
                    String symbol = row["underlyingSymbol"].ToString();
                    Side side = (Side)Enum.Parse(typeof(Side), row["underlyingExposurePositionSide"].ToString(), true);
                    Double quantity = Convert.ToDouble(row["deltaAdjPosition"].ToString());
                    Double markPrice = Convert.ToDouble(row["selectedFeedPrice"].ToString());
                    String sector = row["sector"].ToString();
                    Double dollarDelta = Convert.ToDouble(row[CALC_PREF_STRING].ToString());
                    Position pos = new Position(account, symbol, side, quantity, markPrice, sector, dollarDelta);

                    Position existingPos = positions.FirstOrDefault(x => x.Symbol == symbol);
                    if (existingPos == null)
                        positions.Add(pos);
                    else
                    {
                        existingPos.Quantity += quantity;
                        existingPos.Price = markPrice;
                        existingPos.DollarDelta += dollarDelta;
                    }
                }
                return positions.Where(x => x.Quantity != 0 && !Double.IsInfinity(x.Quantity) && !Double.IsNaN(x.Quantity)).ToList();
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
            return null;
        }
    }
}
