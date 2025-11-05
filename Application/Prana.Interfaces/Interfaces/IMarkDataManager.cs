using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.Interfaces
{
    public interface IMarkDataManager
    {
        double GetMarkPriceForSymbolAndDate(string symbol, DateTime date, int accountId);
        DataTable GetMarkPricesForSymbolsAndDates(DataTable dtMarkPrices);
        DataSet GetMarkPricesForSymbolsAndExactDates(DataTable dtMarkPrices);
        MonthMarkPriceList GetAllStoredMonthMarkPricesForCurrentMonth();
        Dictionary<string, double> GetSplitFactors(string AUECString);
        DataTable GetMarkPricesForGivenDate(DateTime date, int dateMethodology, bool isFxFXForwardData, bool getClosedData);
        DataTable GetMarkPricesForGivenDateRange(string xmlAccounts, DateTime startDate, DateTime endDate, int dateMethodology, bool isFxFXForwardData, int filter);
        List<PricingRule> GetPricingRules();
        DataTable GetBusinessAdjustedDayMarkPriceForGivenDate(DateTime date);
        DataTable GetBusinessAdjustedDayMarkPriceForGivenDateCH(DateTime date);
        DataTable FetchMarkPricesAccountWiseForLastBusinessDay();
        DataTable GetBetaValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology);
        DataTable GetTradingVolDateWise(DateTime fromDate, DateTime toDate, int dateMethodology);
        DataTable GetDeltaValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology);
        DataTable GetOutSatndingDateWise(DateTime fromDate, DateTime toDate, int dateMethodology);
        DataTable GetVolatilityValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology);
        DataTable GetVWAPValueDateWise(DateTime fromDate, int dateMethodology, bool getSameDayClosedDataOnDV);
        DataTable GetDividendYieldValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology);
        int SaveMarkPrices(DataTable dtMarkPrices, bool isAutoApproved);
        int SaveBeta(DataTable dtBeta);
        int SaveTradingVolume(DataTable dtTradingVol);
        int SaveDelta(DataTable dtDelta);
        int SaveOutStandings(DataTable dtOutStanding);
        int SaveNAVValues(DataTable dtNAVValue);
        int SaveVolatility(DataTable dtVolatility);
        int SaveVWAP(DataTable dtVWAP);
        int SaveDividendYield(DataTable dtDividendYield);
        DataTable GetNAVValueDateWise(DateTime fromDate, DateTime toDate, int dateMethodology);
        DataTable GetPerformanceNumberValueDateWise(DateTime fromDate);
        DataTable GetStartOfMonthCapitalAccountValuesDateWise(DateTime fromDate, int dateMethodology);
        int SaveStartOfMonthCapitalAccountValues(DataTable dtStartOfMonthCapitalAccountValue);
        int DeleteStartOfMonthCapitalAccountValues(DateTime deletionDate);
        DataTable GetUserDefinedMTDPnLValuesDateWise(DateTime fromDate, int dateMethodology);
        int SaveUserDefinedMTDPnLValues(DataTable dtUserDefinedMTDPnLValue);
        int DeleteUserDefinedMTDPnLValues(DateTime deletionDate);
        Prana.BusinessObjects.CurrencyCollection GetCurrenciesWithSymbol();
        DataTable GetCollateralInterest(DateTime fromDate, int dateMethodology);
        DataTable GetDailyCash(DateTime fromDate, int dateMethodology);
        int SaveDailyCashValues(DataTable dtDailyCashValues);
        int SavePerformanceNumberValues(DataTable dtPerformanceNumberValuesTemp);
        int DeleteDailyCashValue(int accountID, int localCurrencyID, int baseCurrencyID, DateTime date);
        DataTable GetConversionRateDateWise(DateTime fromDate, int dateMethodology);
        DataTable GetAccountWiseConversionRate(string xmlAccount, DateTime fromDate, DateTime toDate, int dateMethodology, int filter);
        DataSet GetSAPIRequestFieldData(string requestField);
        void SaveSAPIRequestFieldData(DataSet saveDataSetTemp, string requestField);
        int SaveForexRate(DataTable dtForexRate);
        int SaveStandardCurrencyPair(DataTable dtForexRate);
        int SaveForexRateWithAccount(DataTable dtForexRate);
        DataTable GetUnapprovedMarkPricesFromDB(DateTime startDate, DateTime endDate);
        DataSet ApproveMarkPricesinDB(String xmlMarkPrice);
        int RescindMarkPricesinDB(string xmlMarkPrice);
        List<PricePolicyDetails> GetPriceRuleDetailFromDB();
        List<PricingPolicyDetailsFromSP> GetPricePolicyDetailSPFromDB(int accountID, DateTime dateTime, string spName);
        DataSet GetPricePolicyDetailSPFromDB(string spName, int accountID, string filePath, string folderPath, DateTime startDate, DateTime endDate);
        DataTable GetCollateralPriceDateWise(DateTime fromDate, int dateMethodology, bool getSameDayClosedDataOnDV, bool isOnlyFixedIncomeSymbols);
        int SaveCollateralValues(DataTable dtCollateralPriceValue);
    }
}
