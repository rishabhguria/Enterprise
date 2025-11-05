using Castle.Windsor;
using Prana.BusinessObjects;
using Prana.CommonDatabaseAccess;
using Prana.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.CommonDataCache
{
    public static class WindsorContainerManager
    {
        private static IWindsorContainer _container;
        private static IClientsCommonDataManager _clientsCommonDataManager;
        private static IKeyValueDataManager _keyValueDataManager;
        private static IMarkDataManager _markDataManager;
        private static IRiskPreferenceDataManager _riskPrefDataManager;

        public static IWindsorContainer Container
        {
            get { return _container; }
            set
            {
                _container = value;
                _clientsCommonDataManager = _container.Resolve<IClientsCommonDataManager>();
                _keyValueDataManager = _container.Resolve<IKeyValueDataManager>();
                _markDataManager = _container.Resolve<IMarkDataManager>();
                _riskPrefDataManager = _container.Resolve<IRiskPreferenceDataManager>();
            }
        }

        public static Prana.BusinessObjects.TradingAccountCollection GetTradingAccounts(int userID)
        {
            return _clientsCommonDataManager.GetTradingAccounts(userID);
        }

        public static DataSet GetAllAccountTablesFromDB()
        {
            return _clientsCommonDataManager.GetAllAccountTablesFromDB();
        }

        public static DataSet GetAllAccountsWithRelation(DataSet _masterCategorySubCategory)
        {
            return _clientsCommonDataManager.GetAllAccountsWithRelation(_masterCategorySubCategory);
        }

        public static DataTable GetAllPermittedAccounts(int UserID)
        {
            return _clientsCommonDataManager.GetAllPermittedAccounts(UserID);
        }

        public static Prana.BusinessObjects.AccountCollection GetMasterFunds()
        {
            return _clientsCommonDataManager.GetMasterFunds();
        }

        public static Prana.BusinessObjects.AccountCollection GetAccounts()
        {
            return _clientsCommonDataManager.GetAccounts();
        }

        public static List<string> GetAccountsForTheUser(int userID)
        {
            return _clientsCommonDataManager.GetAccountsForTheUser(userID);
        }

        public static bool GetPMModulePermission(int companyID)
        {
            return _clientsCommonDataManager.GetPMModulePermission(companyID);
        }

        public static CounterPartyCollection GetCompanyUserCounterParties(int userID)
        {
            return _clientsCommonDataManager.GetCompanyUserCounterParties(userID);
        }

        public static Prana.BusinessObjects.AccountCollection GetAccounts(int userID)
        {
            return _clientsCommonDataManager.GetAccounts(userID);
        }

        public static ExecutionInstructions GetExecutionInstructionByAUCVID(int assetID, int underlyingID, int counterPartyID, int venueID)
        {
            return _clientsCommonDataManager.GetExecutionInstructionByAUCVID(assetID, underlyingID, counterPartyID, venueID);
        }

        public static Prana.BusinessObjects.StrategyCollection GetStrategies(int userID)
        {
            return _clientsCommonDataManager.GetStrategies(userID);
        }

        public static Sides GetOrderSidesByCVAUEC(int assetID, int UnderLyingID, int counterPartyID, int venueID)
        {
            return _clientsCommonDataManager.GetOrderSidesByCVAUEC(assetID, UnderLyingID, counterPartyID, venueID);
        }

        public static TimeInForces GetTIFByAUCVID(int assetID, int underlyingID, int counterPartyID, int venueID)
        {
            return _clientsCommonDataManager.GetTIFByAUCVID(assetID, underlyingID, counterPartyID, venueID);
        }

        public static HandlingInstructions GetHandlingInstructionByAUCVID(int assetID, int underlyingID, int counterPartyID, int venueID)
        {
            return _clientsCommonDataManager.GetHandlingInstructionByAUCVID(assetID, underlyingID, counterPartyID, venueID);
        }

        public static OrderTypes GetOrderTypesByAUCVID(int assetID, int underlyingID, int counterPartyID, int venueID)
        {
            return _clientsCommonDataManager.GetOrderTypesByAUCVID(assetID, underlyingID, counterPartyID, venueID);
        }

        public static Traders GetTraders(int companyClientID)
        {
            return _clientsCommonDataManager.GetTraders(companyClientID);
        }

        public static ClearingFirmsPrimeBrokers GetClearingFirmsPrimeBrokers()
        {
            return _clientsCommonDataManager.GetClearingFirmsPrimeBrokers();
        }

        public static ClientAccounts GetCompanyClientAccounts(int companyClientID)
        {
            return _clientsCommonDataManager.GetCompanyClientAccounts(companyClientID);
        }

        public static ClientCollection GetClients(int companyID)
        {
            return _clientsCommonDataManager.GetClients(companyID);
        }

        public static Prana.BusinessObjects.VenueCollection GetVenuesByAUIDCounterPartyAndUserID(int userID, int counterPartyID, int assetID, int underlyingID)
        {
            return _clientsCommonDataManager.GetVenuesByAUIDCounterPartyAndUserID(userID, counterPartyID, assetID, underlyingID);
        }

        public static CounterPartyCollection GetCounterPartiesByAUIDAndUserID(int userID, int AssetID, int UnderlyingID)
        {
            return _clientsCommonDataManager.GetCounterPartiesByAUIDAndUserID(userID, AssetID, UnderlyingID);
        }

        public static UnderLyings GetUnderLyingsByAssetAndUserID(int userID, int assetID)
        {
            return _clientsCommonDataManager.GetUnderLyingsByAssetAndUserID(userID, assetID);
        }

        public static Assets GetCompanyUserAssets(int userID)
        {
            return _clientsCommonDataManager.GetCompanyUserAssets(userID);
        }

        public static AUECs GetCompanyUserAUECDetails(int companyUserID)
        {
            return _clientsCommonDataManager.GetCompanyUserAUECDetails(companyUserID);
        }

        public static DataSet GetAllStandardCurrencyPairs()
        {
            return _clientsCommonDataManager.GetAllStandardCurrencyPairs();
        }

        public static void DeleteCustomViewPreference(int userID, string tabname)
        {
            _clientsCommonDataManager.DeleteCustomViewPreference(userID, tabname);
        }

        public static Sides GetSides()
        {
            return _clientsCommonDataManager.GetSides();
        }

        public static List<string> GetAllTradingAccounts()
        {
            return _clientsCommonDataManager.GetAllTradingAccounts();
        }

        public static Prana.BusinessObjects.StrategyCollection GetStrategies()
        {
            return _clientsCommonDataManager.GetStrategies();
        }

        public static Dictionary<string, Tuple<DateTime, int>> GetTaxlotsLatestCADates()
        {
            return _clientsCommonDataManager.GetTaxlotsLatestCADates();
        }

        public static ExecutionInstructions GetExecutionInstructions()
        {
            return _clientsCommonDataManager.GetExecutionInstructions();
        }

        public static HandlingInstructions GetHandlingInstructions()
        {
            return _clientsCommonDataManager.GetHandlingInstructions();
        }

        public static TradingAccountCollection GetAllUsersTradingAccounts()
        {
            return _clientsCommonDataManager.GetAllUsersTradingAccounts();
        }

        public static TimeInForces GetTimeInForces()
        {
            return _clientsCommonDataManager.GetTimeInForces();
        }

        public static OrderTypes GetOrderTypes()
        {
            return _clientsCommonDataManager.GetOrderTypes();
        }

        public static VenueCollection GetAllUsersVenues()
        {
            return _clientsCommonDataManager.GetAllUsersVenues();
        }

        public static UnderLyings GetUnderLyingsByUserID(int userID)
        {
            return _clientsCommonDataManager.GetUnderLyingsByUserID(userID);
        }

        public static TranferTradeRules GetTransferTradeRules(int CompanyId)
        {
            return _clientsCommonDataManager.GetTransferTradeRules(CompanyId);
        }

        public static List<PricingPolicyDetailsFromSP> GetPricePolicyDetailSPFromDB(int accountID, DateTime dateTime, string spName)
        {
            return _markDataManager.GetPricePolicyDetailSPFromDB(accountID, dateTime, spName);
        }

        public static DataTable FetchMarkPricesAccountWiseForLastBusinessDay()
        {
            return _markDataManager.FetchMarkPricesAccountWiseForLastBusinessDay();
        }

        public static DataSet GetPricePolicyDetailSPFromDB(string spName, int accountID, string filePath, string folderPath, DateTime startDate, DateTime endDate)
        {
            return _markDataManager.GetPricePolicyDetailSPFromDB(spName, accountID, filePath, folderPath, startDate, endDate);
        }

        public static List<PricePolicyDetails> GetPriceRuleDetailFromDB()
        {
            return _markDataManager.GetPriceRuleDetailFromDB();
        }

        public static List<PricingRule> GetPricingRules()
        {
            return _markDataManager.GetPricingRules();
        }

        public static DataTable GetAccountWiseConversionRate(string xmlAccount, DateTime fromDate, DateTime toDate, int dateMethodology, int filter)
        {
            return _markDataManager.GetAccountWiseConversionRate(xmlAccount, fromDate, toDate, dateMethodology, filter);
        }

        public static Prana.BusinessObjects.CurrencyCollection GetCurrenciesWithSymbol()
        {
            return _markDataManager.GetCurrenciesWithSymbol();
        }

        public static int DeleteDailyCashValue(int accountID, int localCurrencyID, int baseCurrencyID, DateTime date)
        {
            return _markDataManager.DeleteDailyCashValue(accountID, localCurrencyID, baseCurrencyID, date);
        }

        public static DataTable GetMarkPricesForSymbolsAndDates(DataTable dtMarkPrices)
        {
            return _markDataManager.GetMarkPricesForSymbolsAndDates(dtMarkPrices);
        }

        public static int DeleteUserDefinedMTDPnLValues(DateTime deletionDate)
        {
            return _markDataManager.DeleteUserDefinedMTDPnLValues(deletionDate);
        }

        public static int SaveUserDefinedMTDPnLValues(DataTable dtUserDefinedMTDPnLValue)
        {
            return _markDataManager.SaveUserDefinedMTDPnLValues(dtUserDefinedMTDPnLValue);
        }

        public static DataTable GetUserDefinedMTDPnLValuesDateWise(DateTime fromDate, int dateMethodology)
        {
            return _markDataManager.GetUserDefinedMTDPnLValuesDateWise(fromDate, dateMethodology);
        }

        public static int DeleteStartOfMonthCapitalAccountValues(DateTime deletionDate)
        {
            return _markDataManager.DeleteStartOfMonthCapitalAccountValues(deletionDate);
        }

        public static int SaveStartOfMonthCapitalAccountValues(DataTable dtStartOfMonthCapitalAccountValue)
        {
            return _markDataManager.SaveStartOfMonthCapitalAccountValues(dtStartOfMonthCapitalAccountValue);
        }

        public static DataTable GetStartOfMonthCapitalAccountValuesDateWise(DateTime fromDate, int dateMethodology)
        {
            return _markDataManager.GetStartOfMonthCapitalAccountValuesDateWise(fromDate, dateMethodology);
        }

        public static DataTable GetCollateralInterest(DateTime fromDate, int dateMethodology)
        {
            return _markDataManager.GetCollateralInterest(fromDate, dateMethodology);
        }

        public static DataTable GetDailyCash(DateTime fromDate, int dateMethodology)
        {
            return _markDataManager.GetDailyCash(fromDate, dateMethodology);
        }

        public static int SaveNAVValues(DataTable dtNAVValue)
        {
            return _markDataManager.SaveNAVValues(dtNAVValue);
        }

        public static DataTable GetNAVValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
        {
            return _markDataManager.GetNAVValueDateWise(fromDate, toDate, dateMethodology);
        }

        public static DataTable GetConversionRateDateWise(DateTime fromDate, int dateMethodology)
        {
            return _markDataManager.GetConversionRateDateWise(fromDate, dateMethodology);
        }

        public static DataTable GetVWAPValueDateWise(DateTime fromDate, int dateMethodology, bool getSameDayClosedDataOnDV)
        {
            return _markDataManager.GetVWAPValueDateWise(fromDate, dateMethodology, getSameDayClosedDataOnDV);
        }

        public static DataTable GetVolatilityValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
        {
            return _markDataManager.GetVolatilityValueDateWise(fromDate, toDate, dateMethodology);
        }

        public static DataTable GetDividendYieldValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
        {
            return _markDataManager.GetDividendYieldValueDateWise(fromDate, toDate, dateMethodology);
        }

        public static DataTable GetDeltaValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
        {
            return _markDataManager.GetDeltaValueDateWise(fromDate, toDate, dateMethodology);
        }

        public static DataTable GetTradingVolDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
        {
            return _markDataManager.GetTradingVolDateWise(fromDate, toDate, dateMethodology);
        }

        public static DataTable GetBetaValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
        {
            return _markDataManager.GetBetaValueDateWise(fromDate, toDate, dateMethodology);
        }

        public static int SaveTradingVolume(DataTable dtTradingVol)
        {
            return _markDataManager.SaveTradingVolume(dtTradingVol);
        }

        public static int SaveDelta(DataTable dtDelta)
        {
            return _markDataManager.SaveDelta(dtDelta);
        }

        public static DataTable GetPerformanceNumberValueDateWise(DateTime fromDate)
        {
            return _markDataManager.GetPerformanceNumberValueDateWise(fromDate);
        }

        public static DataTable GetOutSatndingDateWise(DateTime fromDate, DateTime toDate, int dateMethodology)
        {
            return _markDataManager.GetOutSatndingDateWise(fromDate, toDate, dateMethodology);
        }

        public static double GetMarkPriceForSymbolAndDate(string symbol, DateTime date, int accountId)
        {
            return _markDataManager.GetMarkPriceForSymbolAndDate(symbol, date, accountId);
        }

        public static string SavePSSymbolMappingToDB(DataTable dt)
        {
            return _riskPrefDataManager.SavePSSymbolMappingToDB(dt);
        }

        public static void DeleteInterestRateFromDB(int id)
        {
            _riskPrefDataManager.DeleteInterestRateFromDB(id);
        }

        public static List<int> GetInUseAUECIDs()
        {
            return _keyValueDataManager.GetInUseAUECIDs();
        }

        public static Dictionary<int, string> getSubAccountsWithMasterCategoryName(bool isStaleData = false)
        {
            return _keyValueDataManager.getSubAccountsWithMasterCategoryName(isStaleData);
        }

        public static DataSet getSubAccountsForExport()
        {
            return _keyValueDataManager.getSubAccountsForExport();
        }

        public static Dictionary<int, DateTime> FetchClearanceTime()
        {
            return _keyValueDataManager.FetchClearanceTime();
        }

        public static Dictionary<int, MarketTimes> GetMarketStartEndTime()
        {
            return _keyValueDataManager.GetMarketStartEndTime();
        }
    }
}