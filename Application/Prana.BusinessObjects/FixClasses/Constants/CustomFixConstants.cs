namespace Prana.BusinessObjects
{
    public class CustomFIXConstants
    {
        public const string CUST_TAG_TradingAccountID = "100001";
        public const string CUST_TAG_CounterPartyID = "100002";
        public const string CUST_TAG_VenueID = "100003";
        public const string CUST_TAG_AssetID = "100004";
        public const string CUST_TAG_UnderlyingID = "100005";
        public const string CUST_TAG_ExchangeID = "100006";
        public const string CUST_TAG_CurrencyID = "100007";
        public const string CUST_TAG_AUECID = "100008";
        public const string CUST_TAG_CompanyUserID = "100009";
        public const string CUST_TAG_ListID = "100010";
        public const string CUST_TAG_WaveID = "100011";
        public const string CUST_TAG_GroupID = "100012";
        public const string CUST_TAG_BasketSequenceNumber = "100013";
        public const string CUST_TAG_Level1ID = "100014";
        public const string CUST_TAG_Level2ID = "100015";
        public const string CUST_TAG_PranaMsgType = "100016";
        public const string CUST_TAG_SendQty = "100017";
        //        public const string CUST_TAG_Text = "100018";
        public const string CUST_TAG_StagedOrderID = "100019";
        public const string CUST_TAG_ParentClOrderID = "100020";
        public const string CUST_TAG_ClientOrderID = "100021";
        public const string CUST_TAG_ParentClientOrderID = "100022";
        public const string CUST_TAG_ClientTime = "100023";

        public const string CUST_Tag_GiveUpID = "100025";
        public const string CUST_TAG_CompanyUserName = "100026";
        public const string CUST_TAG_CounterPartyStatus = "100027";
        public const string CUST_TAG_OrderSeqNumber = "100028";
        public const string CUST_TAG_ShortRebate = "100029";
        public const string CUST_TAG_PNP = "100030";
        public const string CUST_TAG_AlgoProperties = "100031";

        public const string CUST_TAG_AlgoStrategyID = "100032";
        //added Manually
        public const string MSGListACK = "ACK";
        public const string CUST_TAG_OriginatorType = "100033";
        public const string CUST_TAG_DisplayQuantity = "100034";
        public const string CUST_TAG_RandomQuantity = "100035";
        public const string CUST_TAG_Broker = "100036";
        public const string CUST_TAG_SettlementDate = "100038";
        public const string CUST_TAG_AUECLocalDate = "100039";
        public const string CUST_TAG_Symbology = "100041";
        public const string CUST_TAG_TickerSymbol = "100042";
        public const string CUST_TAG_ReutersSymbol = "100043";
        public const string CUST_TAG_ISINSymbol = "100044";
        public const string CUST_TAG_SEDOLSymbol = "100045";
        public const string CUST_TAG_CusipSymbol = "100046";
        public const string CUST_TAG_BloombergSymbol = "100047";
        public const string CUST_TAG_BloombergSymbolExCode = "100310";
        // reserved for other symbology (Starting from 100109)

        public const string CUST_TAG_PositionTag = "100049";
        public const string CUST_TAG_AUECDate = "100050";
        public const string CUST_TAG_UTCDate = "100051";
        public const string CUST_TAG_TaxlotOpenQty = "100052";
        public const string CUST_TAG_L1TaxlotID = "100053";
        public const string CUST_TAG_L2TaxlotID = "100054";
        public const string CUST_TAG_PositionType = "100055";
        public const string CUST_TAG_GeneratedPositionType = "100056";
        public const string CUST_TAG_NewCompanyName = "100057";
        public const string CUST_TAG_CompanyName = "100058";
        public const string CUST_TAG_Sector = "100059";
        public const string CUST_TAG_ReferID = "110000";
        public const string CUST_TAG_OrigSecQtyRatio = "100061";
        public const string CUST_TAG_NewSecQtyRatio = "100062";
        public const string CUST_TAG_OrigSymbol = "100063";
        public const string CUST_TAG_NewSymbol = "100064";
        public const string CUST_TAG_CorporateActionType = "100065";
        public const string CUST_TAG_GroupCumQty = "100066";
        public const string CUST_TAG_GroupAllocatedQty = "100067";
        public const string CUST_TAG_L1AllocatedQty = "100068";
        public const string CUST_TAG_L2AllocatedQty = "100069";
        public const string CUST_TAG_ClosedQty = "100070";
        public const string CUST_TAG_EffectiveDate = "100071";
        public const string CUST_TAG_TargetTag = "100072";

        public const string CUST_TAG_CorpActionID = "100073";

        //SwapParameters
        public const string CUST_TAG_SwapParameters = "100074";
        public const string CUST_TAG_NotionalValue = "100075";
        public const string CUST_TAG_BenchMarkRate = "100076";
        public const string CUST_TAG_Differential = "100077";
        public const string CUST_TAG_ResetFrequency = "100078";
        public const string CUST_TAG_OrigTransDate = "100079";
        public const string CUST_TAG_OrigCostBasis = "100080";
        public const string CUST_TAG_SwapDescription = "100081";
        public const string CUST_TAG_DayCount = "100082";
        public const string CUST_TAG_FirstResetDate = "100083";
        public const string CUST_TAG_SwapClosingDate = "100084";
        public const string CUST_TAG_SwapClosingPrice = "100085";
        public const string CUST_TAG_SwapTransDate = "100085";





        //CONST_OrigSecQtyRatio
        public const string CUST_TAG_AvgFXRateForTrade = "100086";
        public const string CUST_TAG_ServiceEndPoint = "100087";

        //TODO Should be in custom message types class
        #region custom messaage types
        public const string MsgDropCopyReceived = "DCR";
        public const string MsgDropCopyAck = "DCA";
        public const string MsgDropCopyExecution = "DCE";
        public const string MsgDropCopyReject = "DCRj";
        public const string MSGAlgoSyntheticReplaceOrderNew = "AlgoRep";
        public const string MSGAlgoSyntheticReplaceOrderFIX = "AlgoRepFIX";
        public const string MSGAlgoSyntheticReplaceOrder = "AlgoSynRRPL";
        public const string MsgServerReject = "ServerRej";
        public const string ORDSTATUS_AlgoPreviousPendingReplace = "AlgoRpl";
        public const string ORDSTATUS_AlgoPreviousCancelRejected = "AlgoCXLRej";
        public const string ORDSTATUS_Aborted = "Abort";
        public const string MsgUserConnected = "UserConnected";

        public const string MSG_SECMASTER_REQ = "SMReq";
        public const string MSG_SECMASTER_RESPONSE = "SMRes";
        public const string MSG_SECMASTER_SaveREQ = "SMSaveReq";
        public const string MSG_SECMASTER_SYMBOLSEARCH_REQ = "SMSymbolSearchReq";
        public const string MSG_SECMASTER_SYMBOLSEARCH_RESPONSE = "SMSymbolSearchRes";
        public const string MSG_SECMASTER_UPDATE_SHAREOUTSTANDING = "SMUpdateShareOutstanding";

        public const string MSG_SECMASTER_SaveREQ_IMPORT = "SMSaveReq_Import";
        public const string MSG_SECMASTER_UpdateREQ_IMPORT = "SMUpdateReq_Import";
        public const string MSG_SECMASTER_UpdateSMFields_IMPORT = "UpdateSMFields_Import";

        public const string MSG_SECMASTER_SavePrefREQ = "SMSavePrefReq";
        public const string MSG_SECMASTER_GetPrefREQ = "SMGetPrefReq";
        public const string MSG_RISK_REQ = "RiskReq";
        public const string MSG_RISK_RES = "RiskRes";
        public const string MSG_RISKSIM_REQ = "RiskSimReq";
        public const string MSG_RISKSIM_RES = "RiskSimRes";
        public const string MSG_RESPONSE_COMPLETED = "ResComp";
        public const string MSG_SECMASTER_SymbolREQ = "SMSymbolReq";
        public const string MSG_SECMASTER_FutureMultiplierREQ = "SMFutureMulReq";
        public const string MSG_SECMASTER_FutureMultiplierSave = "SMFutureMulSave";
        public const string MSG_SECMASTER_SymbolRESPONSE = "SMSymbolRes";
        public const string MSG_SECMASTER_ListSymbolRESPONSE = "SMListSymbolRes";
        public const string MSG_SECMASTER_BulkSymbolRESPONSE = "SMBulkSymbolRes";
        public const string MSG_SET_PSSYMBOLS = "SetPSSymbols";
        public const string MSG_RISKBeta_REQ = "RiskBetaReq";
        public const string MSG_RISKBeta_RES = "RiskBetaRes";
        public const string MSG_Trade = "Trade";
        public const string MSG_Grp_Trade = "Grp_Trade";
        public const string MSG_Coprorate_Undo = "CorpUndo";
        public const string MSG_SECMASTER_FutureMultiplierSaveRESPONSE = "FutRootSaveRes";
        public const string MSG_SECMASTER_UpdateClientCache = "UpdateClientCache";

        public const string MSG_SECMASTER_UDA_DATA_Req = "UDAReq";
        public const string MSG_SECMASTER_UDA_DATA_Res = "UDARes";
        public const string MSG_SECMASTER_UDA_Save = "UDASave";

        public const string MSG_SECMASTER_AUEC_MAPPING_REQUEST = "GetAUECMappingRequest";
        public const string MSG_SECMASTER_AUEC_MAPPING_RESPONSE = "GetAUECMappingResponse";
        public const string MSG_SECMASTER_AUEC_MAPPING_SAVE = "SaveAUECMapping";

        public const string MSG_SECMASTER_FUND_SYMBOL_UDA_REQUEST = "GetAccountsUDARequest";
        public const string MSG_SECMASTER_FUND_SYMBOL_UDA_RESPONSE = "GetAccountsUDAResponse";
        public const string MSG_SECMASTER_FUND_SYMBOL_UDA_SAVE = "SaveAccountsUDA";

        //stuck trade on server 
        public const string MSG_GET_MISSING_TRADES = "MISSING_TRADES";

        public const string MSG_Undo_NameChange = "UndoNameChange";
        public const string MSG_Undo_Split = "UndoSplit";
        public const string MSG_Undo_CashDividend = "UndoCashDividend";
        public const string MSG_Coprorate_Undo_Response = "CorpUndoRes";

        public const string MSG_Coprorate_Delete = "CorpDel";
        public const string MSG_SaveCAForSymbolOnly = "SaveCorporateActionForSymbolOnly";
        //public const string MSG_SaveCAForSymbolOnlyRedo = "SaveCorporateActionForSymbolOnlyRedo";
        //public const string MSG_SaveCAWithCompanyNameChangeOnlyRedo = "SaveCorporateActionWithCompanyNameChangeOnlyRedo";
        public const string MSG_SaveCAWithSymbolAndCompanyNameChange = "SaveCorpActionWithSymbolAndCompanyNameChange";
        //public const string MSG_SaveCAWithSymbolAndCompanyNameChangeRedo = "SaveCorpActionWithSymbolAndCompanyNameChangeRedo";
        public const string MSG_SaveCAForSplits = "SaveCAForSplits";

        public const string MSG_GetOldCompanyNameForNameChange = "GetOldCompanyNameForNameChange";

        public const string MSG_GetCompanyDetailsFromSymbol = "GetCompanyDetailsFromSymbol";

        public const string MSG_SaveCorporateActionWithoutApplying = "SaveCorporateActionWithoutApplying";
        public const string MSG_UpdateCorporateActionWithoutApplying = "UpdateCorporateActionWithoutApplying";
        public const string MSG_SaveCAForCashDividend = "SaveCAForCashDividend";
        public const string MSG_GetAllCAs = "GetAllCorporateActions";
        public const string MSG_GetFullCAData = "GetFullCorporateActionData";
        public const string MSG_ConvertFixToXmlFormatInDb = "ConvertFixToXmlFormatInDb";

        public const string MSG_REFRESH_DEFAULT = "Refresh_AllocDefault";
        public const string MSG_COUNTERPARTY_CONNECTIONSTATUS_REPORT = "CP";
        public const string MSG_COUNTERPARTY_CONNECTIONSTATUS_REQUEST = "CP_Req";
        public const string MSG_CounterPartyDown = "CP_Down";
        public const string MSG_CounterPartySendingProblem = "CP_SendingProb";
        public const string MSG_CounterPartyUp = "CP_Up";
        public const string MSG_ExceptionRaised = "Exp";
        public const string MSG_PerformanceReport = "P_Report";
        public const string MSG_StatusMessge = "MsgStatus";
        public const string MSG_PREPARE_REPORTS = "Prep_R";
        public const string MSG_REPORTS_PREPARED = "Prep_F";
        public const string MSG_GET_POSITIONS = "Get_Pos";
        public const string MSG_REFRESH_POSITIONS = "Ref_Pos";
        public const string MSG_PSHistoricalVol_OMI = "PSHistoricalVolReq";
        public const string MSG_CASHACTIVITY_QUEUE = "CashActivity_QUEUE";
        public const string MSG_NAV_LOCK_DATE_UPDATE = "NAVLockDateUpdate";
        public const string MSG_MANUAL_ROLLOVER = "Manual_RollOver";
        #endregion


        public const string CUST_TAG_TaxLotPK = "100090";
        //public const string CUST_TAG_IsOnlyCompanyNameChange = "100091";

        public const string CUST_TAG_NewTaxlotOpenQty = "100092";
        public const string CUST_TAG_NewAvgPrice = "100093";
        public const string CUST_TAG_HandlerType = "100094";
        public const string CUST_TAG_Delta = "100097";
        //100098, is reserved for temp tag
        //100099 is for leadcurrencyID

        public const string CUST_TAG_CounterPartyName = "100106";
        public const string CUST_TAG_ServerReceivedTime = "100108";

        public const string CUST_TAG_OSIOptionSymbol = "100109";
        public const string CUST_TAG_IDCOOptionSymbol = "100110";
        public const string CUST_TAG_OPRAOptionSymbol = "100111";

        // Used for future process date.
        public const string CUST_TAG_IsCutOffTimeUsed = "100112";
        public const string CUST_TAG_CutOffTime = "100113";
        public const string CUST_TAG_ProcessDate = "100114";
        // Used in case of name change and transfers among accounts
        public const string CUST_TAG_OriginalPurchaseDate = "100115";

        public const string CUST_TAG_DaysToSettlementFixedIncome = "100116";

        public const string CUST_TAG_CommissionRate = "100117";
        public const string CUST_TAG_CalculationBasis = "100118";

        public const string CUST_TAG_CommissionAmt = "100119";


        public const string CUST_TAG_SoftCommissionRate = "100120";
        public const string CUST_TAG_SoftCommissionCalculationBasis = "100121";
        public const string CUST_TAG_SoftCommissionAmt = "100122";
        public const string CUST_TAG_ServerReceivedFullTime = "100123";
        public const string CUST_TAG_TradeAttribute1 = "100126";
        public const string CUST_TAG_TradeAttribute2 = "100127";
        public const string CUST_TAG_TradeAttribute3 = "100128";
        public const string CUST_TAG_TradeAttribute4 = "100129";
        public const string CUST_TAG_TradeAttribute5 = "100130";
        public const string CUST_TAG_TradeAttribute6 = "100131";
        public const string CUST_TAG_SettlementCurrencyName = "100133";
        public const string CUST_TAG_SettlementCurrencyID = "100134";
        public const string CUST_TAG_OrigCounterPartyID = "100135";
        public const string CUST_TAG_FXConversionMethodOperator = "100136";
        public const string CUST_TAG_MultiTradeMessage = "100137";
        public const string CUST_TAG_ChangeType = "100138";
        public const string CUST_TAG_InternalComments = "100140";
        public const string CUST_TAG_RoundLot = "100141";
        public const string CUST_TAG_OriginalLevel1ID = "100142";
        public const string CUST_TAG_IsProcessed = "100143";
        public const string CUST_TAG_CumQtyForSubOrder = "100144";
        public const string CUST_TAG_AlgoStrategyName = "100145";
        public const string CUST_TAG_TradeBlocked = "100146";
        public const string CUST_TAG_ACCRUAL_BASIS = "100147";
        public const string CUST_TAG_DATE_ISSUE = "100148";
        public const string CUST_TAG_FIRST_COUPON_DATE = "100149";
        public const string CUST_TAG_FREQUENCY = "100150";
        public const string CUST_TAG_IsStageRequired = "100151";
        public const string CUST_TAG_IsManualOrder = "100152";
        public const string CUST_TAG_TransactionSourceTag = "100153";

        //OTC Parameters
        public const string CUST_TAG_DaysToSettle = "100153";
        //public const string CUST_TAG_EffectiveDate = "100154";
        public const string CUST_TAG_ISDA_CounterParty = "100155";
        public const string CUST_TAG_EquityLeg_Frequency = "100156";
        public const string CUST_TAG_EquityLeg_BulletSwap = "100157";
        public const string CUST_TAG_EquityLeg_ImpliedCommission = "100158";
        public const string CUST_TAG_EquityLeg_ExcludeDividends = "100185";
        public const string CUST_TAG_EquityLeg_FirstPaymentDate = "100159";
        public const string CUST_TAG_EquityLeg_ExpirationDate = "100160";
        public const string CUST_TAG_Commission_Basis = "100161";
        public const string CUST_TAG_Commission_HardCommissionRate = "100162";
        public const string CUST_TAG_Commission_SoftCommissionRate = "100163";
        public const string CUST_TAG_FinanceLeg_InterestRate = "100164";
        public const string CUST_TAG_FinanceLeg_SpreadBasisPoint = "100165";
        public const string CUST_TAG_FinanceLeg_DayCount = "100166";
        public const string CUST_TAG_FinanceLeg_Frequency = "100167";
        public const string CUST_TAG_FinanceLeg_FixedRate = "100168";
        public const string CUST_TAG_FinanceLeg_FirstResetDate = "100169";
        public const string CUST_TAG_FinanceLeg_FirstPaymentDate = "100170";
        public const string CUST_TAG_CustomFields = "100171";
        public const string CUST_TAG_TradeDate = "100172";
        public const string CUST_TAG_InstrumentTypeID = "100173";

        public const string CUST_TAG_Collateral_DayCount = "100174";
        public const string CUST_TAG_Collateral_Margin = "100175";
        public const string CUST_TAG_Collateral_Rate = "100176";
        public const string CUST_TAG_Symbol = "100177";
        public const string CUST_TAG_UniqueIdentifier = "100178";
        public const string CUST_TAG_Description = "100179";
        public const string CUST_TAG_DCStageWorkFlow = "100156";

        //ShortLocateParameter
        public const string CUST_TAG_BorrowerID = "100024";
        public const string CUST_TAG_BorrowBroker = "100180";
        public const string CUST_TAG_BorrowQuantity = "100181";
        public const string CUST_TAG_BorrowSharesAvailable = "100182";
        public const string CUST_TAG_BorrowRate = "100184";
        public const string CUST_TAG_NirvanaLocateID = "100185";
        public const string CUST_TAG_ReplaceQuantity = "100205";
        public const string CUST_TAG_IsPricingAvailable = "100187";

        public const string CUST_TAG_ModifiedUserId = "100186";
        public const string CUST_TAG_IsAUECDateAppendRequired = "100188";


        public const string CUST_TAG_EquityLeg_ConversionPrice = "100189";
        public const string CUST_TAG_EquityLeg_ConversionRatio = "100190";
        public const string CUST_TAG_Currency = "100191";
        public const string CUST_TAG_EquityLeg_ConversionDate = "100192";
        public const string CUST_TAG_FinanceLeg_ZeroCoupon = "100193";
        public const string CUST_TAG_FinanceLeg_IRBenchMark = "100194";
        public const string CUST_TAG_FinanceLeg_FXRate = "100195";
        public const string CUST_TAG_FinanceLeg_SBPoint = "100196";
        public const string CUST_TAG_Isin = "100197";
        public const string CUST_TAG_Cusip = "100198";
        public const string CUST_TAG_FinanceLeg_CouponFreq = "100199";
        public const string CUST_TAG_FinanceLeg_ParValue = "100200";
        public const string CUST_TAG_Sedol = "100201";
        public const string CUST_TAG_AvgPriceForCompliance = "100202";
        public const string CUST_TAG_Activ_Symbol = "100203";
        public const string CUST_TAG_FactSet_Symbol = "100204";
        public const string CUST_TAG_IsSamsaraUser = "100206";
        public const string CUST_TAG_IsUseCustodianBroker = "100207";
        public const string CUST_TAG_AccountBrokerMapping = "100208";
        public const string CUST_TAG_BrokerConnectionType = "100209";
        public const string CUST_TAG_ConnectionID = "100210";
        public const string CUST_TAG_TransactionLevel = "100216";
        public const string CUST_TAG_ThirdPartyBatchId = "100217";
        public const string CUST_TAG_ThirdPartyRunDate = "100218";
        public const string CUST_TAG_ThirdPartyJobName = "100219";

        //EOD repeating group custom tags
        public const string CUST_TAG_PartyID1 = "100211";
        public const string CUST_TAG_PartyID2 = "100212";
        public const string CUST_TAG_PartyID3 = "100213";
        public const string CUST_TAG_PartyID4 = "100214";
        public const string CUST_TAG_PartyID5 = "100215";

        public const string CUST_TAG_PartyIDSource1 = "100221";
        public const string CUST_TAG_PartyIDSource2 = "100222";
        public const string CUST_TAG_PartyIDSource3 = "100223";
        public const string CUST_TAG_PartyIDSource4 = "100224";
        public const string CUST_TAG_PartyIDSource5 = "100225";

        public const string CUST_TAG_PartyRole1 = "100231";
        public const string CUST_TAG_PartyRole2 = "100232";
        public const string CUST_TAG_PartyRole3 = "100233";
        public const string CUST_TAG_PartyRole4 = "100234";
        public const string CUST_TAG_PartyRole5 = "100235";

        public const string CUST_TAG_NestedPartyID1 = "100241";
        public const string CUST_TAG_NestedPartyID2 = "100242";
        public const string CUST_TAG_NestedPartyID3 = "100243";
        public const string CUST_TAG_NestedPartyID4 = "100244";
        public const string CUST_TAG_NestedPartyID5 = "100245";

        public const string CUST_TAG_NestedPartyIDSource1 = "100251";
        public const string CUST_TAG_NestedPartyIDSource2 = "100252";
        public const string CUST_TAG_NestedPartyIDSource3 = "100253";
        public const string CUST_TAG_NestedPartyIDSource4 = "100254";
        public const string CUST_TAG_NestedPartyIDSource5 = "100255";

        public const string CUST_TAG_NestedPartyRole1 = "100261";
        public const string CUST_TAG_NestedPartyRole2 = "100262";
        public const string CUST_TAG_NestedPartyRole3 = "100263";
        public const string CUST_TAG_NestedPartyRole4 = "100264";
        public const string CUST_TAG_NestedPartyRole5 = "100265";

        public const string CUST_TAG_MiscFeeAmt1 = "100271";
        public const string CUST_TAG_MiscFeeAmt2 = "100272";
        public const string CUST_TAG_MiscFeeAmt3 = "100273";
        public const string CUST_TAG_MiscFeeAmt4 = "100274";
        public const string CUST_TAG_MiscFeeAmt5 = "100275";
        public const string CUST_TAG_MiscFeeAmt6 = "100276";
        public const string CUST_TAG_MiscFeeAmt7 = "100277";
        public const string CUST_TAG_MiscFeeAmt8 = "100278";
        public const string CUST_TAG_MiscFeeAmt9 = "100279";

        public const string CUST_TAG_MiscFeeCurr1 = "100281";
        public const string CUST_TAG_MiscFeeCurr2 = "100282";
        public const string CUST_TAG_MiscFeeCurr3 = "100283";
        public const string CUST_TAG_MiscFeeCurr4 = "100284";
        public const string CUST_TAG_MiscFeeCurr5 = "100285";
        public const string CUST_TAG_MiscFeeCurr6 = "100286";
        public const string CUST_TAG_MiscFeeCurr7 = "100287";
        public const string CUST_TAG_MiscFeeCurr8 = "100288";
        public const string CUST_TAG_MiscFeeCurr9 = "100289";

        public const string CUST_TAG_MiscFeeType1 = "100291";
        public const string CUST_TAG_MiscFeeType2 = "100292";
        public const string CUST_TAG_MiscFeeType3 = "100293";
        public const string CUST_TAG_MiscFeeType4 = "100294";
        public const string CUST_TAG_MiscFeeType5 = "100295";
        public const string CUST_TAG_MiscFeeType6 = "100296";
        public const string CUST_TAG_MiscFeeType7 = "100297";
        public const string CUST_TAG_MiscFeeType8 = "100298";
        public const string CUST_TAG_MiscFeeType9 = "100299";

        public const string CUST_TAG_MiscFeeBasis1 = "100301";
        public const string CUST_TAG_MiscFeeBasis2 = "100302";
        public const string CUST_TAG_MiscFeeBasis3 = "100303";
        public const string CUST_TAG_MiscFeeBasis4 = "100304";
        public const string CUST_TAG_MiscFeeBasis5 = "100305";
        public const string CUST_TAG_MiscFeeBasis6 = "100306";
        public const string CUST_TAG_MiscFeeBasis7 = "100307";
        public const string CUST_TAG_MiscFeeBasis8 = "100308";
        public const string CUST_TAG_MiscFeeBasis9 = "100309";

        public const string CUST_TAG_TradeAttribute7 = "100311";
        public const string CUST_TAG_TradeAttribute8 = "100312";
        public const string CUST_TAG_TradeAttribute9 = "100313";
        public const string CUST_TAG_TradeAttribute10 = "100314";
        public const string CUST_TAG_TradeAttribute11 = "100315";
        public const string CUST_TAG_TradeAttribute12 = "100316";
        public const string CUST_TAG_TradeAttribute13 = "100317";
        public const string CUST_TAG_TradeAttribute14 = "100318";
        public const string CUST_TAG_TradeAttribute15 = "100319";
        public const string CUST_TAG_TradeAttribute16 = "100320";
        public const string CUST_TAG_TradeAttribute17 = "100321";
        public const string CUST_TAG_TradeAttribute18 = "100322";
        public const string CUST_TAG_TradeAttribute19 = "100323";
        public const string CUST_TAG_TradeAttribute20 = "100324";
        public const string CUST_TAG_TradeAttribute21 = "100325";
        public const string CUST_TAG_TradeAttribute22 = "100326";
        public const string CUST_TAG_TradeAttribute23 = "100327";
        public const string CUST_TAG_TradeAttribute24 = "100328";
        public const string CUST_TAG_TradeAttribute25 = "100329";
        public const string CUST_TAG_TradeAttribute26 = "100330";
        public const string CUST_TAG_TradeAttribute27 = "100331";
        public const string CUST_TAG_TradeAttribute28 = "100332";
        public const string CUST_TAG_TradeAttribute29 = "100333";
        public const string CUST_TAG_TradeAttribute30 = "100334";
        public const string CUST_TAG_TradeAttribute31 = "100335";
        public const string CUST_TAG_TradeAttribute32 = "100336";
        public const string CUST_TAG_TradeAttribute33 = "100337";
        public const string CUST_TAG_TradeAttribute34 = "100338";
        public const string CUST_TAG_TradeAttribute35 = "100339";
        public const string CUST_TAG_TradeAttribute36 = "100340";
        public const string CUST_TAG_TradeAttribute37 = "100341";
        public const string CUST_TAG_TradeAttribute38 = "100342";
        public const string CUST_TAG_TradeAttribute39 = "100343";
        public const string CUST_TAG_TradeAttribute40 = "100344";
        public const string CUST_TAG_TradeAttribute41 = "100345";
        public const string CUST_TAG_TradeAttribute42 = "100346";
        public const string CUST_TAG_TradeAttribute43 = "100347";
        public const string CUST_TAG_TradeAttribute44 = "100348";
        public const string CUST_TAG_TradeAttribute45 = "100349";

        public const string CUST_TAG_TradeApplicationSource = "100350";
        public const string CUST_TAG_IsMultiBrokerTrade = "100351";
    }
}
