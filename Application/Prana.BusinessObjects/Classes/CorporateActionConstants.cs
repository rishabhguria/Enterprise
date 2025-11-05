namespace Prana.BusinessObjects
{
    public class CorporateActionConstants
    {
        public const string CONST_tbApplied = "tbApplied";
        public const string CONST_tbUnApplied = "tbUnApplied";

        public const string CONST_OrigSymbolTag = "OrigSymbol";
        public const string CONST_OrigSecQtyRatio = "OrigSecQtyRatio";
        public const string CONST_NewSecQtyRatio = "NewSecQtyRatio";
        public const string CONST_Factor = "Factor";
        public const string CONST_IsApplied = "IsApplied";
        public const string CONST_NewSymbolTag = "NewSymbol";
        public const string CONST_EffectiveDate = "EffectiveDate";
        public const string CONST_NewAsset = "NewAsset";
        public const string CONST_NewUnderlying = "NewUnderlying";
        public const string CONST_NewExchange = "NewExchange";
        public const string CONST_NewCurrency = "NewCurrency";
        public const string CONST_Comments = "Comments";
        public const string CONST_ApplyOrRollback = "ApplyOrRollback";
        public const string CONST_CorporateActionType = "CorporateActionType";
        public const string CONST_Account = "Account";
        public const string CONST_CorporateActionId = "CorpActionID";
        public const string CONST_TargetTag = "TargetTag";
        public const string CONST_CompanyName = "CompanyName";// Original Comapany Name
        public const string CONST_NewCompanyName = "NewCompanyName";
        public const string CONST_Symbology = "Symbology";
        public const string CONST_Description = "Description";

        //Used only for the purpose of the Excel Export
        public const string CONST_SymbologyName = "SymbologyName";
        //Used only for the purpose of the Excel Export
        public const string CONST_CorporateActionTypeName = "CorporateActionTypeName";

        public const string CONST_AppliedTabKey = "tbcApplied";
        public const string CONST_UnAppliedTabKey = "tbcUnApplied";

        public const string CONST_RollBackCaption = "RollBack";
        public const string CONST_ApplyCaption = "Apply";
        public const string CONST_TaxLotID = "TaxLotID";

        public const string CONST_SymbologyID = "SymbologyID";

        public const string CONST_Cashinlieu = "Cashinlieu";
        public const string CONST_ClosingPrice = "ClosingPrice";
        public const string CONST_SaveStockDividendAtZeroPrice = "SaveStockDividendAtZeroPrice";
        //public const string CONST_IsOnlyCompanyNameChange = "IsOnlyCompanyNameChange";

        /// <summary>
        /// Dividends fields
        /// </summary>
        public const string CONST_DivDeclarationDate = "DivDeclarationDate";
        public const string CONST_ExDivDate = "ExDivDate";
        public const string CONST_RecordDate = "RecordDate";
        public const string CONST_DivPayoutDate = "DivPayoutDate";
        public const string CONST_DivRate = "DivRate";

        /// <summary>
        /// Exchange Merger fields
        /// </summary>
        public const string CONST_ExchangeRatio = "ExchangeRatio";

        /// <summary>
        /// This is used to round off CA generated fractional shares
        /// </summary>
        public const string CONST_CAQuantityToRoundOff = "CAQuantityToRoundOff";


    }
}
