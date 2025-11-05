using Prana.Global;
using System.Windows.Forms;


namespace Prana.ReconciliationNew
{
    public class ReconConstants
    {
        public const string MismatchReason = "MismatchDetails";
        public const string CAPTION_MissingData = "Missing Data";

        public const string CAPTION_MismatchReason = "Mismatch Details";
        public const string MismatchType = "MismatchType";
        public const string CAPTION_MismatchType = "MismatchType";
        public const string Matched = "Matched";
        public const string PrimaryKey = "RowID";
        public static string ReconDataDirectoryPath = Application.StartupPath.ToString() + @"\" + ApplicationConstants.RECON_DATA_DIRECTORY.ToString();
        //


        public const string CustomColumnsTableName = "CustomColumns";
        public const string MatchingRulesRuleTableName = "Parameter";
        public const string MasterColumnsNirvanaTableName = "NirvanaGridMasterColumns";
        public const string MasterColumnsPBTableName = "PBGridMasterColumns";
        public const string MismatchType_ExactlyMatched = "Exactly Matched";
        public const string MismatchType_MismatchWithPB = "Mismatch with PB";
        public const string MismatchType_MultipleMismatches = "Multiple Mismatches";
        public const string MismatchType_AllocationMismatch = "Allocation Mismatch";
        public const string MismatchReason_DataMissingPB = "Data Missing in PB File";
        public const string MismatchReason_MatchingRuleError = "Matching Rule Error";
        public const string MismatchReason_DataMissingNirvana = "Data Missing in Nirvana";
        public const string CAPTION_MasterFund = "Account";
        public const string CAPTION_AccountName = "Account";
        public const string LockStatus_Locked = "Locked";
        public const string LockStatus_UnLocked = "UnLocked";
        public const string COLUMN_RowIndex = "RowIndex";
        public const string RECONLAYOUT = "Recon Layout";



        //public const string TABLENAME_NirvanaMasterColumns = "NirvanaMasterColumns";
        //public const string TABLENAME_PBMasterColumns = "PBMasterColumns";

        public const string TABLENAME_NirvanaGridColumns = "NirvanaGridMasterColumns";
        public const string TABLENAME_PBGridMasterColumns = "PBGridMasterColumns";
        public const string COLUMN_CurrencySymbol = "CurrencySymbol";
        public const string COLUMN_Name = "Name";
        public const string COLUMN_ISSelected = "IsSelected";
        public const string COLUMN_ISEditable = "IsEditable";
        public const string COLUMN_GroupType = "GroupType";
        public const string COLUMN_FormulaExpression = "FormulaExpression";
        public const string COLUMN_ReconType = "ReconType";
        public const string COLUMN_SP = "SP";
        public const string COLUMN_DataSourceType = "DataSourceType";
        public const string COLUMN_ReconId = "Recon_Id";
        public const string COLUMN_Summary = "Summary";
        public const string COLUMN_ISIncluded = "IsIncluded";
        public const string NAME_ExceptionReport = "NirvanaExceptionReport";
        public const string GRIDGroup_GeneralInformation = "GeneralInformation";
        public const string COLUMN_LockStatus = "LockStatus";
        public const string CAPTION_LockStatus = "Lock Status";

        public const string COLUMN_Checkbox = "checkBox";
        public const string COLUMN_Comments = "Comments";
        public const string CAPTION_Comments = "Comments";
        public const string CAPTION_FormulaExpression = "Formula Expression";
        public const string CONST_Common = "Common";
        public const string CONST_Nirvana = "Nirvana";
        public const string CONST_Broker = "Broker";
        public const string CONST_ReconStatus = "ReconStatus";
        public const string CONST_ToleranceType = "ToleranceType";
        public const string CONST_ToleranceValue = "ToleranceValue";
        public const string CONST_Diff = "Diff";
        public const string CONST_OriginalValue = "OriginalValue";

        public const string COLUMN_Symbol = "Symbol";
        public const string COLUMN_Side = "Side";

        public const string CONST_AvgPX = "AvgPX";
        public const string CONST_Multiplier = "Multiplier";
        public const string CONST_SideMultiplier = "SideMultiplier";
        public const string CONST_NirvanaTotalCommissionAndFees = "NirvanaTotalCommissionandFees";
        public const string CONST_GrossNotionalValue = "GrossNotionalValue";
        public const string CONST_MarketValue = "MarketValue";
        public const string COL_TOTALCOMMISSIONANDFEES = "TotalCommissionandFees";

        public const string COLUMN_MasterFund = "MasterFund";
        public const string COLUMN_AccountName = "AccountName";
        public const string CONST_MARKPRICE = "MarkPrice";


        /////// public const string COLUMN_Name = "Name";
        public const string COLUMN_TradeDate = "TradeDate";
        public const string COLUMN_TaxLotID = "TaxLotID";
        public const string COLUMN_Client = "Client";
        public const string COLUMN_Group = "Group";
        public const string COLUMN_Account = "AccountName";
        public const string COLUMN_ThirdParty = "ThirdParty";
        public const string COLUMN_BreakType = "BreakType";
        public const string COLUMN_ApplicationData = "ApplicationData";
        public const string COLUMN_ThirdPartyData = "ThirdPartyData";
        public const string COLUMN_AmendedData = "AmendedData";
        public const string COLUMN_ChangedBy = "ChangedBy";
        public const string COLUMN_ChangedColumns = "ChangedColumns";
        public const string COLUMN_TaxLotStatus = "TaxLotStatus";
        public const string COLUMN_ApproveChangesXmlRowIndex = "Index";
        public const string COLUMN_ApproveChangesXmlPath = "Path";
        public const string COLUMN_PrimeBroker = "PrimeBroker";
        public const string COLUMN_Accounts = "Accounts";
        public const string COLUMN_Selected = "Selected";
        public const string COLUMN_UserLoggedIN = "UserLoggedIN";
        public const string COLUMN_Quantity = "Quantity";
        public const string COLUMN_NAVLockStatus = "NAVLockStatus";
        public const string COLUMN_ClosingStatus = "ClosingStatus";

        public const string CAPTION_PrimeBroker = "Prime Broker";
        public const string CAPTION_TaxLotStatus = "Tax Lot Status";
        public const string CAPTION_TradeDate = "Trade Date";
        public const string CAPTION_TaxLotID = "Tax Lot ID";
        public const string CAPTION_Client = "Client";
        public const string CAPTION_Group = "Group";
        public const string CAPTION_Account = "Account";
        public const string CAPTION_ThirdParty = "Third Party";
        public const string CAPTION_Symbol = "Symbol";
        public const string CAPTION_ReconType = "Recon Type";
        public const string CAPTION_BreakType = "Break Type";
        public const string CAPTION_ApplicationData = "Application Data";
        public const string CAPTION_ThirdPartyData = "Third Party Data";
        public const string CAPTION_AmendedData = "Amended Data";
        public const string CAPTION_ChangedBy = "Changed By";
        public const string CAPTION_NAVLockStatus = "NAV Lock Status";
        public const string CAPTION_ClosingStatus = "Closing Status";

        public const string COLUMN_FieldName = "ColumnName";
        public const string COLUMN_OldValue = "OldValue";
        public const string COLUMN_NewValue = "NewValue";
        public const string SelectDefaultValue = "-Select-";

        public const string COLUMN_FromCurrencyID = "FromCurrencyID";
        public const string COLUMN_ToCurrencyID = "ToCurrencyID";
        public const string COLUMN_BloombergSymbol = "BloombergSymbol";
        public const string COLUMN_AccountID = "AccountID";

        public const string COLUMN_ReconTypeName = "ReconTypeName";
        public const string COLUMN_GroupingColumns = "GroupingColumns";

        public const string COLUMN_ClientID = "ClientID";
        public const string COLUMN_ReconTypeID = "ReconTypeID";
        public const string COLUMN_TemplateName = "TemplateName";
        public const string COLUMN_TemplateKey = "TemplateKey";
        public const string COLUMN_IsShowCAGeneratedTrades = "IsShowCAGeneratedTrades";

    }
}
