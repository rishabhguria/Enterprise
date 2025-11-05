using Prana.BusinessObjects;
using Prana.BusinessObjects.GreenFieldModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.Interfaces
{
    public interface IKeyValueDataManager
    {
        Dictionary<int, Prana.BusinessObjects.PositionManagement.ThirdPartyNameID> GetAllDataSources();
        DataSet GetKeyValuePairs();
        Dictionary<int, List<Account>> FillAccountsCompanyWise(DataTable keyValues);
        DataSet GetFrequentlyUsedData();
        Dictionary<int, string> FillKeyValuePairs(DataTable keyValues, int offset);
        Dictionary<string, string> FillImportTag(DataTable keyValues, int offset);
        Dictionary<int, List<int>> FillThirdPartyAccounts(DataTable keyValues, int offset);
        Dictionary<int, string> FillAUEC(DataTable DtAUECs);
        int GetLastPreferencedAccountID();
        System.Collections.Generic.Dictionary<int, double> GetAUECMultipliers();
        System.Collections.Generic.Dictionary<int, decimal> GetAUECRoundLots();
        System.Collections.Generic.Dictionary<string, int> GetExchangeIdentifiers();
        System.Collections.Generic.Dictionary<int, int> GetRoundOffRules();
        System.Collections.Generic.Dictionary<int, int> GetAUECIdToAssetMapping();
        Dictionary<int, List<int>> GetCompanyMasterFundSubAccountAssociation(int companyID);
        List<int> GetUserPermittedMasterFundBasedOnFunds(int companyID, int userID);
        Dictionary<int, List<int>> GetCompanyDataSourceSubAccountAssociation();
        void ResetAllAccountTable(DataSet dsCashAccountTables, DataSet dsCashAccountTablesWithRelation);
        void SetAllActivityTables(DataSet dsActivityTables);
        Dictionary<int, string> getSubAccounts();
        Dictionary<int, string> getSubAccountsWithMasterCategoryName(bool isStaleData = false);
        DataSet getSubAccountsForExport();
        Dictionary<string, int> getSubAccounts_Acronym();
        Dictionary<int, Dictionary<string, int>> getSubAccounts_Side_Multipier();
        Dictionary<int, string> getSubAccounts_AccountType();
        System.Collections.Generic.Dictionary<int, byte[]> GetFlagsbyAUECs();
        DataSet GetAUECandCVPermissionsForUser(int userID, int companyID);
        DataTable GetAllExecutionInstruction();
        Dictionary<string, string> GetAllSides();
        DataTable GetAllSidesWithID();
        DataTable GetAllBasicSides();
        DataTable GetAllOrderTypes();
        DataTable GetAllHandlingInstruction();
        DataTable GetAllTIFs();
        DataTable GetAllCMTA();
        DataTable GetAllGiveUp();
        DataTable GetAssetsExchangeIdentifiers();
        System.Collections.Generic.Dictionary<int, Prana.BusinessObjects.TimeZone> GetAUECIDTimeZones();
        System.Collections.Generic.Dictionary<int, StructSettlementPeriodSidewise> GetAUECIDSettlementPeriods();
        DataTable GetCompany();
        Dictionary<int, string> GetCompanies();
        Dictionary<int, string> GetAllMasterFunds();
        Dictionary<string, int> GetCountryWiseFactsetCodes();
        Dictionary<string, int> GetCountryWiseBloombergCodes();
        Dictionary<int, string> GetAllAccountGroups();
        Dictionary<int, string> GetAllCounterPartyVenues();
        Venue GetExAssignVenueID();
        Dictionary<int, string> GetAllMasterStrategy();
        Dictionary<int, string> GetClosingAssets();
        List<GenericNameID> GetAllPrimeBrokers();
        List<int> GetInUseAUECIDs();
        Dictionary<int, DateTime> FetchClearanceTime();
        Prana.BusinessObjects.TimeZone GetAUECTimeZone(int auecID);
        int GetAUECCount();
        Dictionary<int, MarketTimes> GetMarketStartEndTime();
        Dictionary<string, int> GetActivityType();
        Dictionary<string, string> GetActivityTypeWithAcronym();
        Dictionary<int, RevaluationUpdateDetail> GetLastRevaluationCalcDate();
        Dictionary<int, byte> GetActivityTypeActivitySource();
        Dictionary<int, string> GetActivityAmountType();
        Dictionary<string, int> GetActivityWithBalanceTypeID();
        DataSet GetAttributeNames();
        DataSet GetPranaPreference();
        ConcurrentDictionary<int, int> FillAccountWiseBaseCurrency(DataTable dtKeyValues, int offset);
        Dictionary<int, List<int>> FillReleaseWiseAccount(DataTable dataTable);
        Dictionary<int, string> GetCompanyModulesPermissioning(int companyID);
        Dictionary<int, string> GetAlgoBrokersWithFullName();
        Dictionary<int, string> GetAlgoBrokersWithShortName();
        bool GetSendAllocationsViaFix();
        DateTime? GetCurrentNavLockDate();

        IList<MasterFundAccountDetails> GetAllCustomGroups();
    }
}
