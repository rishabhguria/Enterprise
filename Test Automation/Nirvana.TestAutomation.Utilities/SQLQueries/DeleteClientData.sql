SELECT DB_NAME() AS [Current Database];
if exists (select * from dbo.sysobjects where name = 'T_ClientFills')
delete from T_ClientFills
GO
  print 'Done Data Deletion in :T_ClientFills'
GO
if exists (select * from dbo.sysobjects where name = 'T_ClientSub')
delete from T_ClientSub
GO
  print 'Done Data Deletion in :T_ClientSub'
GO
if exists (select * from dbo.sysobjects where name = 'T_ClientOrder')
delete from T_ClientOrder
GO
  print 'Done Data Deletion in :T_ClientOrder'
GO
if exists (select * from dbo.sysobjects where name = 'PM_PositionTaxlots')
delete from PM_PositionTaxlots
GO
  print 'Done Data Deletion in :PM_PositionTaxlots'
GO
if exists (select * from dbo.sysobjects where name = 'PM_NetPositions')
delete from PM_NetPositions
GO
  print 'Done Data Deletion in :PM_NetPositions'
GO
if exists (select * from dbo.sysobjects where name = 'PM_TaxlotClosing')
delete from PM_TaxlotClosing
GO
  print 'Done Data Deletion in :PM_TaxlotClosing'
GO
if exists (select * from dbo.sysobjects where name = 'T_OrderCommission')
delete from T_OrderCommission
GO
  print 'Done Data Deletion in :T_OrderCommission'
GO
if exists (select * from dbo.sysobjects where name = 'T_GroupCommission')
delete from T_GroupCommission
GO
  print 'Done Data Deletion in :T_GroupCommission'
GO
if exists (select * from dbo.sysobjects where name = 'T_FundAllocationCommission')
delete from T_FundAllocationCommission
GO
  print 'Done Data Deletion in :T_FundAllocationCommission'
GO
if exists (select * from dbo.sysobjects where name ='PM_TaxlotClosing')
delete from dbo.PM_TaxlotClosing
GO
  print 'Done Data Deletion in :dbo.PM_TaxlotClosing'
GO
if exists (select * from dbo.sysobjects where name = 'T_Level2Allocation')
delete from T_Level2Allocation
GO

  print 'Done Data Deletion in :T_Level2Allocation'
GO
if exists (select * from dbo.sysobjects where name = 'T_FundAllocation')
delete from T_FundAllocation
GO
  print 'Done Data Deletion in :T_FundAllocation'
GO
if exists (select * from dbo.sysobjects where name = 'T_BTFundAllocation')
delete from T_BTFundAllocation
GO
  print 'Done Data Deletion in :T_BTFundAllocation'
GO
if exists (select * from dbo.sysobjects where name = 'T_BTGroupOrders')
delete from T_BTGroupOrders
GO
  print 'Done Data Deletion in :T_BTGroupOrders'
GO
if exists (select * from dbo.sysobjects where name = 'T_GroupOrder')
delete from T_GroupOrder
GO
  print 'Done Data Deletion in :T_GroupOrder'
GO
if exists (select * from dbo.sysobjects where name = 'BT_GroupsBaskets')
delete from BT_GroupsBaskets
GO
  print 'Done Data Deletion in :BT_GroupsBaskets'
GO
if exists (select * from dbo.sysobjects where name = 'BT_BasketGroups')
delete from BT_BasketGroups
GO
  print 'Done Data Deletion in :BT_BasketGroups'
GO
if exists (select * from dbo.sysobjects where name = 'T_BTWaveOrders')
delete from T_BTWaveOrders
GO
  print 'Done Data Deletion in :T_BTWaveOrders'
GO
if exists (select * from dbo.sysobjects where name = 'T_BTWave')
delete from T_BTWave
GO
  print 'Done Data Deletion in :T_BTWave'
GO
if exists (select * from dbo.sysobjects where name = 'T_BTTradedBasket')
delete from T_BTTradedBasket
GO
  print 'Done Data Deletion in :T_BTTradedBasket'
GO
if exists (select * from dbo.sysobjects where name = 'T_BTUpLoadedBasketOrders')
delete from T_BTUpLoadedBasketOrders
GO
  print 'Done Data Deletion in :T_BTUpLoadedBasketOrders'
GO
if exists (select * from dbo.sysobjects where name = 'T_BTUploadedBaskets')
delete from T_BTUploadedBaskets
GO
  print 'Done Data Deletion in :T_BTUploadedBaskets'
GO
if exists (select * from dbo.sysobjects where name = 'T_Fills')
delete from T_Fills
GO
  print 'Done Data Deletion in :T_Fills'
GO
if exists (select * from dbo.sysobjects where name = 'T_Sub')
delete from T_Sub
GO
  print 'Done Data Deletion in :T_Sub'
GO
if exists (select * from dbo.sysobjects where name = 'T_Order')
delete from T_Order
GO
  print 'Done Data Deletion in :T_Order'
GO
if exists (select * from dbo.sysobjects where name = 'T_TradedOrders')
delete from T_TradedOrders
GO
  print 'Done Data Deletion in :T_TradedOrders'
GO
if exists (select * from dbo.sysobjects where name = 'PM_CompanyDailyEquityValue')
delete from PM_CompanyDailyEquityValue
GO
  print 'Done Data Deletion in :PM_CompanyDailyEquityValue'
GO
if exists (select * from dbo.sysobjects where name = 'PM_CompanyFundDailyPNL')
delete from PM_CompanyFundDailyPNL
GO
  print 'Done Data Deletion in :PM_CompanyFundDailyPNL'
GO
if exists (select * from dbo.sysobjects where name = 'T_GroupOrder')
delete from T_GroupOrder
GO
  print 'Done Data Deletion in :T_GroupOrder'
GO
if exists (select * from dbo.sysobjects where name = 'T_Level2Allocation')
delete from T_Level2Allocation
GO
  print 'Done Data Deletion in :T_Level2Allocation'
GO
if exists (select * from dbo.sysobjects where name = 'T_Group')
delete from T_Group
GO
  print 'Done Data Deletion in :T_Group'
GO
if exists (select * from dbo.sysobjects where name = 'T_RelationShip')
delete from T_RelationShip
GO
  print 'Done Data Deletion in :T_RelationShip'
GO
if exists (select * from dbo.sysobjects where name = 'PM_PositionMaster')
delete from PM_PositionMaster
GO
  print 'Done Data Deletion in :PM_PositionMaster'
GO
if exists (select * from dbo.sysobjects where name = 'T_Expire_Settlement')
delete from T_Expire_Settlement
GO
  print 'Done Data Deletion in :T_Expire_Settlement'
GO
if exists (select * from dbo.sysobjects where name = 'PM_Taxlots')
delete from PM_Taxlots
GO
  print 'Done Data Deletion in :PM_Taxlots'
GO
if exists (select * from dbo.sysobjects where name = 'PM_YTDPnL')
delete from PM_YTDPnL
GO
  print 'Done Data Deletion in :PM_YTDPnL'
GO
if exists (select * from dbo.sysobjects where name = 'T_Externalorder')
delete from T_Externalorder
GO
  print 'Done Data Deletion in :T_Externalorder'
GO
if exists (select * from dbo.sysobjects where name = 'PM_CorpActionTaxlots')
delete from PM_CorpActionTaxlots
GO
  print 'Done Data Deletion in :PM_CorpActionTaxlots'
GO
if exists (select * from dbo.sysobjects where name = 'PM_CompanyFundCashCurrencyValue')
delete from PM_CompanyFundCashCurrencyValue
GO
  print 'Done Data Deletion in :PM_CompanyFundCashCurrencyValue'
GO
if exists (select * from dbo.sysobjects where name = 'T_SubAccountCashValue')
delete from T_SubAccountCashValue
GO
  print 'Done Data Deletion in :T_SubAccountCashValue'
GO
if exists (select * from dbo.sysobjects where name = 'T_TaxlotCashDividends')
delete from T_TaxlotCashDividends
GO
  print 'Done Data Deletion in :T_TaxlotCashDividends'
GO
if exists (select * from dbo.sysobjects where name ='T_Journal')
delete from dbo.T_Journal
GO
  print 'Done Data Deletion in :dbo.T_Journal'
GO
if exists (select * from dbo.sysobjects where name ='T_Journal_Backup1')
delete from dbo.T_Journal_Backup1
GO
  print 'Done Data Deletion in :dbo.T_Journal_Backup1'
GO

if exists (select * from dbo.sysobjects where name ='T_PranaUserPrefs')
delete from dbo.T_PranaUserPrefs
GO
  print 'Done Data Deletion in :dbo.T_PranaUserPrefs'
GO
if exists (select * from dbo.sysobjects where name ='T_PBWiseTaxlotState')
delete from dbo.T_PBWiseTaxlotState
GO
  print 'Done Data Deletion in :dbo.T_PBWiseTaxlotState'
GO
if exists (select * from dbo.sysobjects where name ='T_PMDataDump')
delete from dbo.T_PMDataDump
GO
  print 'Done Data Deletion in :dbo.T_PMDataDump'
GO
if exists (select * from dbo.sysobjects where name ='T_UDASymbolData')
delete from dbo.T_UDASymbolData
GO
  print 'Done Data Deletion in :dbo.T_UDASymbolData'
GO

if exists (select * from dbo.sysobjects where name ='T_Group_deletedAudit')
delete from dbo.T_Group_deletedAudit
GO
  print 'Done Data Deletion in :dbo.T_Group_deletedAudit'
GO
if exists (select * from dbo.sysobjects where name ='PM_Taxlots_deletedAudit')
delete from dbo.PM_Taxlots_deletedAudit
GO
  print 'Done Data Deletion in :dbo.PM_Taxlots_deletedAudit'
GO

  print 'Done Data Deletion in :dbo.T_TradeAudit'
GO
if exists (select * from dbo.sysobjects where name ='Trace30')
delete from dbo.Trace30
GO
  print 'Done Data Deletion in :dbo.Trace30'
GO

if exists (select * from dbo.sysobjects where name ='T_Sub')
delete from dbo.T_Sub
GO
  print 'Done Data Deletion in :dbo.T_Sub'
GO
if exists (select * from dbo.sysobjects where name ='SchemaChangeLog')
delete from dbo.SchemaChangeLog
GO
  print 'Done Data Deletion in :dbo.SchemaChangeLog'
GO
if exists (select * from dbo.sysobjects where name ='T_Fills')
delete from dbo.T_Fills
GO
  print 'Done Data Deletion in :dbo.T_Fills'
GO
if exists (select * from dbo.sysobjects where name ='T_Group')
delete from dbo.T_Group
GO
  print 'Done Data Deletion in :dbo.T_Group'
GO
if exists (select * from dbo.sysobjects where name ='PM_DailyTradingVol')
delete from dbo.PM_DailyTradingVol
GO
  print 'Done Data Deletion in :dbo.PM_DailyTradingVol'
GO
if exists (select * from dbo.sysobjects where name ='T_TradedOrders')
delete from dbo.T_TradedOrders
GO
  print 'Done Data Deletion in :dbo.T_TradedOrders'
GO
if exists (select * from dbo.sysobjects where name ='T_BTUnBundledBasketStrategyOreders')
delete from dbo.T_BTUnBundledBasketStrategyOreders
GO
  print 'Done Data Deletion in :dbo.T_BTUnBundledBasketStrategyOreders'
GO
if exists (select * from dbo.sysobjects where name ='PM_DailyDelta')
delete from dbo.PM_DailyDelta
GO
  print 'Done Data Deletion in :dbo.PM_DailyDelta'
GO
if exists (select * from dbo.sysobjects where name ='T_BTUnBundledBasketFundOrders')
delete from dbo.T_BTUnBundledBasketFundOrders
GO
  print 'Done Data Deletion in :dbo.T_BTUnBundledBasketFundOrders'
GO
if exists (select * from dbo.sysobjects where name ='T_OrderCustomRequestDetails')
delete from dbo.T_OrderCustomRequestDetails
GO
  print 'Done Data Deletion in :dbo.T_OrderCustomRequestDetails'
GO
if exists (select * from dbo.sysobjects where name ='T_Order')
delete from dbo.T_Order
GO
  print 'Done Data Deletion in :dbo.T_Order'
GO

if exists (select * from dbo.sysobjects where name ='PM_Taxlots')
delete from dbo.PM_Taxlots
GO
  print 'Done Data Deletion in :dbo.PM_Taxlots'
GO
if exists (select * from dbo.sysobjects where name ='PM_CompanyFundCashCurrencyValue')
delete from dbo.PM_CompanyFundCashCurrencyValue
GO
  print 'Done Data Deletion in :dbo.PM_CompanyFundCashCurrencyValue'
GO
if exists (select * from dbo.sysobjects where name ='T_LastCalculatedBalanceDate')
delete from dbo.T_LastCalculatedBalanceDate
GO
  print 'Done Data Deletion in :dbo.T_LastCalculatedBalanceDate'
GO
if exists (select * from dbo.sysobjects where name ='BT_AllocatedBasket_OrderEntityRelation')
delete from dbo.BT_AllocatedBasket_OrderEntityRelation
GO
  print 'Done Data Deletion in :dbo.BT_AllocatedBasket_OrderEntityRelation'
GO
if exists (select * from dbo.sysobjects where name ='PM_DataSourceAssets')
delete from dbo.PM_DataSourceAssets
GO
  print 'Done Data Deletion in :dbo.PM_DataSourceAssets'
GO
if exists (select * from dbo.sysobjects where name ='T_deletedTaxLots')
delete from dbo.T_deletedTaxLots
GO
  print 'Done Data Deletion in :dbo.T_deletedTaxLots'
GO
if exists (select * from dbo.sysobjects where name ='T_UserOptionModelInput')
delete from dbo.T_UserOptionModelInput
GO
  print 'Done Data Deletion in :dbo.T_UserOptionModelInput'
GO
if exists (select * from dbo.sysobjects where name ='T_CA_AlertHistory')
delete from dbo.T_CA_AlertHistory
GO
  print 'Done Data Deletion in :dbo.T_CA_AlertHistory'
GO
if exists (select * from dbo.sysobjects where name ='T_CompanyThirdPartyCVIdentifier')
delete from dbo.T_CompanyThirdPartyCVIdentifier
GO
  print 'Done Data Deletion in :dbo.T_CompanyThirdPartyCVIdentifier'
GO
if exists (select * from dbo.sysobjects where name ='AlgoSyntheticReplaceOrders')
delete from dbo.AlgoSyntheticReplaceOrders
GO
  print 'Done Data Deletion in :dbo.AlgoSyntheticReplaceOrders'
GO

if exists (select * from dbo.sysobjects where name ='T_companyuserfunds_Bak_30122013')
delete from dbo.T_companyuserfunds_Bak_30122013
GO
  print 'Done Data Deletion in :dbo.T_companyuserfunds_Bak_30122013'
GO
if exists (select * from dbo.sysobjects where name ='T_FundAllocation')
delete from dbo.T_FundAllocation
GO
  print 'Done Data Deletion in :dbo.T_FundAllocation'
GO
if exists (select * from dbo.sysobjects where name ='T_Level2Allocation')
delete from dbo.T_Level2Allocation
GO
  print 'Done Data Deletion in :dbo.T_Level2Allocation'
GO
if exists (select * from dbo.sysobjects where name ='PM_TaxlotClosing')
delete from dbo.PM_TaxlotClosing
GO
  print 'Done Data Deletion in :dbo.PM_TaxlotClosing'
GO
if exists (select * from dbo.sysobjects where name ='T_BTSavedBasketOrders')
delete from dbo.T_BTSavedBasketOrders
GO
  print 'Done Data Deletion in :dbo.T_BTSavedBasketOrders'
GO
if exists (select * from dbo.sysobjects where name ='T_ResidualQtyFund')
delete from dbo.T_ResidualQtyFund
GO
  print 'Done Data Deletion in :dbo.T_ResidualQtyFund'
GO

if exists (select * from dbo.sysobjects where name ='T_Layout')
delete from dbo.T_Layout
GO
  print 'Done Data Deletion in :dbo.T_Layout'
GO
--if exists (select * from dbo.sysobjects where name ='T_W_ClientFundMapping')
--delete from dbo.T_W_ClientFundMapping
--GO
--  print 'Done Data Deletion in :dbo.T_W_ClientFundMapping'
--GO
--if exists (select * from dbo.sysobjects where name ='T_W_Clients')
--delete from dbo.T_W_Clients
--GO
--  print 'Done Data Deletion in :dbo.T_W_Clients'
--GO
if exists (select * from dbo.sysobjects where name ='PM_CorpActionTaxlots')
delete from dbo.PM_CorpActionTaxlots
GO
  print 'Done Data Deletion in :dbo.PM_CorpActionTaxlots'
GO
if exists (select * from dbo.sysobjects where name ='T_BTSavedBaskets')
delete from dbo.T_BTSavedBaskets
GO
  print 'Done Data Deletion in :dbo.T_BTSavedBaskets'
GO
if exists (select * from dbo.sysobjects where name ='T_TradingInstructions')
delete from dbo.T_TradingInstructions
GO
  print 'Done Data Deletion in :dbo.T_TradingInstructions'
GO
if exists (select * from dbo.sysobjects where name ='tempgroup')
delete from dbo.tempgroup
GO
  print 'Done Data Deletion in :dbo.tempgroup'
GO


--if exists (select * from dbo.sysobjects where name ='T_W_Funds')
--delete from dbo.T_W_Funds
--GO
--  print 'Done Data Deletion in :dbo.T_W_Funds'
--GO
if exists (select * from dbo.sysobjects where name ='PM_Reports')
delete from dbo.PM_Reports
GO
  print 'Done Data Deletion in :dbo.PM_Reports'
GO

if exists (select * from dbo.sysobjects where name ='T_SwapParameters')
delete from dbo.T_SwapParameters
GO
  print 'Done Data Deletion in :dbo.T_SwapParameters'
GO

if exists (select * from dbo.sysobjects where name ='T_TaxlotCashDividends')
delete from dbo.T_TaxlotCashDividends
GO
  print 'Done Data Deletion in :dbo.T_TaxlotCashDividends'
GO

if exists (select * from dbo.sysobjects where name ='T_CA_RuleUserPermissions')
UPDATE T_CA_RuleUserPermissions
SET RuleOverrideType =1;
GO
  print 'UPDATED DEFAULT USER TYPE PERMISSION TO SOFT'
GO



/*if exists (select * from dbo.sysobjects where name ='T_CA_RulesUserDefined')
delete from dbo.T_CA_RulesUserDefined
GO
  print 'Done Data Deletion in :dbo.T_CA_RulesUserDefined'
GO*/
/*if exists (select * from dbo.sysobjects where name ='T_CA_OtherCompliancePermission')
delete from dbo.T_CA_OtherCompliancePermission
GO
  print 'Done Data Deletion in :dbo.T_CA_OtherCompliancePermission'
GO*/
if exists (select * from dbo.sysobjects where name ='PM_CompanyBaseEquityValue')
delete from dbo.PM_CompanyBaseEquityValue
GO
  print 'Done Data Deletion in :dbo.PM_CompanyBaseEquityValue'
GO
if exists (select * from dbo.sysobjects where name ='PM_CompanyMonthlyAccruals')
delete from dbo.PM_CompanyMonthlyAccruals
GO
  print 'Done Data Deletion in :dbo.PM_CompanyMonthlyAccruals'
GO
if exists (select * from dbo.sysobjects where name ='T_subaccountbalances')
delete from dbo.T_subaccountbalances
GO
  print 'Done Data Deletion in :dbo.T_subaccountbalances'
GO
if exists (select * from dbo.sysobjects where name ='T_CashDivTransactions')
delete from dbo.T_CashDivTransactions
GO
  print 'Done Data Deletion in :dbo.T_CashDivTransactions'
GO
if exists (select * from dbo.sysobjects where name ='PM_DailyOutStandings')
delete from dbo.PM_DailyOutStandings
GO
  print 'Done Data Deletion in :dbo.PM_DailyOutStandings'
GO
if exists (select * from dbo.sysobjects where name ='T_CompanyLogo')
delete from dbo.T_CompanyLogo
GO
  print 'Done Data Deletion in :dbo.T_CompanyLogo'
GO
if exists (select * from dbo.sysobjects where name ='T_PranaLogo')
delete from dbo.T_PranaLogo
GO
  print 'Done Data Deletion in :dbo.T_PranaLogo'
GO

-- need to keep master funds so commenting deletion of master funds
--if exists (select * from dbo.sysobjects where name ='T_CompanyMasterFundSubAccountAssociation')
--delete from dbo.T_CompanyMasterFundSubAccountAssociation
--GO
--  print 'Done Data Deletion in :dbo.T_CompanyMasterFundSubAccountAssociation'
--GO
--if exists (select * from dbo.sysobjects where name ='T_CompanyMasterFunds')
--delete from dbo.T_CompanyMasterFunds
--GO
--  print 'Done Data Deletion in :dbo.T_CompanyMasterFunds'
--GO

/*if exists (select * from dbo.sysobjects where name ='T_CA_CompliancePreferences')
delete from dbo.T_CA_CompliancePreferences

GO
  print 'Done Data Deletion in :dbo.T_CA_CompliancePreferences'
GO*/

if exists (select * from dbo.sysobjects where name ='T_AllocationMasterfundRatio')
delete from dbo.T_AllocationMasterfundRatio

GO
  print 'Done Data Deletion in :dbo.T_AllocationMasterfundRatio'
GO

if exists (select * from dbo.sysobjects where name ='T_ImportFileLog')
delete from dbo.T_ImportFileLog
GO
  print 'Done Data Deletion in :dbo.T_ImportFileLog'
GO

if exists (select * from dbo.sysobjects where name ='T_Order')
delete from dbo.T_Order
GO
  print 'Done Data Deletion in :dbo.T_Order'
GO

if exists (select * from dbo.sysobjects where name ='T_PBWiseTaxlotState')
delete from dbo.T_PBWiseTaxlotState
GO
  print 'Done Data Deletion in :dbo.T_PBWiseTaxlotState'
GO

if exists (select * from dbo.sysobjects where name ='T_DeletedTaxlots')
delete from dbo.T_DeletedTaxlots
GO
  print 'Done Data Deletion in :dbo.T_DeletedTaxlots'
GO
/*
if exists (select * from dbo.sysobjects where name ='T_CA_RulesUserDefined')
delete from dbo.T_CA_RulesUserDefined
GO
  print 'Done Data Deletion in :dbo.T_CA_RulesUserDefined'
GO*/

if exists (select * from dbo.sysobjects where name ='T_CA_AlertHistory_Backup')
delete from dbo.T_CA_AlertHistory_Backup
GO
  print 'Done Data Deletion in :dbo.T_CA_AlertHistory_Backup'
GO

/*if exists (select * from dbo.sysobjects where name ='T_CA_UserReadWritePermission')
delete from dbo.T_CA_UserReadWritePermission
GO
  print 'Done Data Deletion in :dbo.T_CA_UserReadWritePermission'
GO*/

if exists (select * from dbo.sysobjects where name ='T_JournalManualTransaction')
delete from dbo.T_JournalManualTransaction
GO
  print 'Done Data Deletion in :dbo.T_JournalManualTransaction'
GO

if exists (select * from dbo.sysobjects where name ='T_AllActivity')
delete from dbo.T_AllActivity
GO
  print 'Done Data Deletion in :dbo.T_AllActivity'
GO

if exists (select * from dbo.sysobjects where name ='T_CashTransactions')
delete from dbo.T_CashTransactions
GO
  print 'Done Data Deletion in :dbo.T_CashTransactions'
GO

if exists (select * from dbo.sysobjects where name = 'PM_NAVValue')
delete from PM_NAVValue
GO
  print 'Done Data Deletion in : dbo.PM_NAVValue'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AL_StrategyValue')
DELETE FROM T_AL_StrategyValue 
WHERE	AllocationPrefDataId IN (SELECT A.Id FROM T_AL_AllocationPreferenceDef P 
											INNER JOIN T_AL_AllocationPreferenceData A ON A.PresetdefId = P.Id
											WHERE P.Name LIKE '%-%-%' OR P.Id IN (SELECT CalculatedPrefId FROM T_AL_MFWisePrefValues M JOIN T_AL_MFAllocationPreference P
															ON M.MFPreferenceId = P.MFPreferenceId AND P.MFPreferenceName Like '%-%-%')
						)
GO
  print 'Done Data Deletion in : dbo.T_AL_StrategyValue'
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AL_AllocationPreferenceData')
DELETE FROM T_AL_AllocationPreferenceData 
WHERE	PresetdefId IN (SELECT Id FROM T_AL_AllocationPreferenceDef 
						WHERE Name LIKE '%-%-%' OR Id IN (SELECT CalculatedPrefId FROM T_AL_MFWisePrefValues M JOIN T_AL_MFAllocationPreference P
															ON M.MFPreferenceId = P.MFPreferenceId AND P.MFPreferenceName Like '%-%-%')
						)
GO
  print 'Done Data Deletion in : dbo.T_AL_AllocationPreferenceData'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AL_Asset')
DELETE FROM T_AL_Asset 
WHERE	CheckListId IN (SELECT C.CheckListId FROM T_AL_AllocationPreferenceDef P 
						INNER JOIN T_AL_CheckList C ON C.PresetdefId = P.Id
						WHERE P.Name LIKE '%-%-%' OR P.Id IN (SELECT CalculatedPrefId FROM T_AL_MFWisePrefValues M JOIN T_AL_MFAllocationPreference P
															ON M.MFPreferenceId = P.MFPreferenceId AND P.MFPreferenceName Like '%-%-%')
						)
GO
  print 'Done Data Deletion in : dbo.T_AL_Asset'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AL_Exchange')
DELETE FROM T_AL_Exchange 
WHERE	CheckListId IN (SELECT C.CheckListId FROM T_AL_AllocationPreferenceDef P 
						INNER JOIN T_AL_CheckList C ON C.PresetdefId = P.Id
						WHERE P.Name LIKE '%-%-%' OR P.Id IN (SELECT CalculatedPrefId FROM T_AL_MFWisePrefValues M JOIN T_AL_MFAllocationPreference P
															ON M.MFPreferenceId = P.MFPreferenceId AND P.MFPreferenceName Like '%-%-%')
						)
GO
  print 'Done Data Deletion in : dbo.T_AL_Exchange'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AL_PR')
DELETE FROM T_AL_PR
WHERE	CheckListId IN (SELECT C.CheckListId FROM T_AL_AllocationPreferenceDef P 
						INNER JOIN T_AL_CheckList C ON C.PresetdefId = P.Id
						WHERE P.Name LIKE '%-%-%' OR P.Id IN (SELECT CalculatedPrefId FROM T_AL_MFWisePrefValues M JOIN T_AL_MFAllocationPreference P
															ON M.MFPreferenceId = P.MFPreferenceId AND P.MFPreferenceName Like '%-%-%')
						)
GO
  print 'Done Data Deletion in : dbo.T_AL_PR'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AL_OrderSide')
DELETE FROM T_AL_OrderSide
WHERE	CheckListId IN (SELECT C.CheckListId FROM T_AL_AllocationPreferenceDef P 
						INNER JOIN T_AL_CheckList C ON C.PresetdefId = P.Id
						WHERE P.Name LIKE '%-%-%' OR P.Id IN (SELECT CalculatedPrefId FROM T_AL_MFWisePrefValues M JOIN T_AL_MFAllocationPreference P
															ON M.MFPreferenceId = P.MFPreferenceId AND P.MFPreferenceName Like '%-%-%')
						)
GO
  print 'Done Data Deletion in : dbo.T_AL_OrderSide'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AL_StrategyChecklistValues')
DELETE FROM T_AL_StrategyChecklistValues
where AccountCheckListId IN
(
	SELECT Id 
	FROM T_AL_AccountCheckListValue
	where CheckListId IN (SELECT C.CheckListId FROM T_AL_AllocationPreferenceDef P 
							INNER JOIN T_AL_CheckList C ON C.PresetdefId = P.Id
							WHERE P.Name LIKE '%-%-%' OR P.Id IN (SELECT CalculatedPrefId FROM T_AL_MFWisePrefValues M JOIN T_AL_MFAllocationPreference P
															ON M.MFPreferenceId = P.MFPreferenceId AND P.MFPreferenceName Like '%-%-%')
							)
)
GO
  print 'Done Data Deletion in : dbo.T_AL_StrategyChecklistValues'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AL_AccountCheckListValue')
DELETE FROM T_AL_AccountCheckListValue 
where CheckListId IN (SELECT C.CheckListId FROM T_AL_AllocationPreferenceDef P 
						INNER JOIN T_AL_CheckList C ON C.PresetdefId = P.Id
						WHERE P.Name LIKE '%-%-%' OR P.Id IN (SELECT CalculatedPrefId FROM T_AL_MFWisePrefValues M JOIN T_AL_MFAllocationPreference P
															ON M.MFPreferenceId = P.MFPreferenceId AND P.MFPreferenceName Like '%-%-%')
						)
GO
  print 'Done Data Deletion in : dbo.T_AL_AccountCheckListValue'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AL_CheckList')
DELETE FROM T_AL_CheckList 
WHERE	PresetdefId IN (SELECT Id FROM T_AL_AllocationPreferenceDef 
						WHERE Name Like '%-%-%' OR Id IN (SELECT CalculatedPrefId FROM T_AL_MFWisePrefValues M JOIN T_AL_MFAllocationPreference P
															ON M.MFPreferenceId = P.MFPreferenceId AND P.MFPreferenceName Like '%-%-%')
						)
GO
  print 'Done Data Deletion in : dbo.T_AL_CheckList'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AL_AllocationPreferenceDef')
DELETE FROM T_AL_AllocationPreferenceDef 
WHERE	Name Like '%-%-%' OR Id IN (SELECT CalculatedPrefId FROM T_AL_MFWisePrefValues M JOIN T_AL_MFAllocationPreference P
									ON M.MFPreferenceId = P.MFPreferenceId AND P.MFPreferenceName Like '%-%-%')
GO
  print 'Done Data Deletion in : dbo.T_AL_AllocationPreferenceDef'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AL_MasterFundProrataList')
DELETE FROM T_AL_MasterFundProrataList 
WHERE MFPreferenceId IN ( SELECT MFPreferenceId
							FROM T_AL_MFAllocationPreference
							WHERE MFPreferenceName Like '%-%-%'
						)
GO
  print 'Done Data Deletion in : dbo.T_AL_MasterFundProrataList'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AL_MFWisePrefValues')
DELETE FROM T_AL_MFWisePrefValues
WHERE MFPreferenceId IN ( SELECT MFPreferenceId
							FROM T_AL_MFAllocationPreference
							WHERE MFPreferenceName Like '%-%-%'
						)
GO
  print 'Done Data Deletion in : dbo.T_AL_MFWisePrefValues'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AL_MFAllocationPreference')
DELETE FROM T_AL_MFAllocationPreference
WHERE MFPreferenceName Like '%-%-%'
GO
  print 'Done Data Deletion in : dbo.T_AL_MFAllocationPreference'
GO

GO
print 'Client Specific Data deleted'
 GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AL_AllocationDefaultRule')
UPDATE T_AL_AllocationDefaultRule
SET EnableMasterFundAllocation = 0
GO
	print 'Disabled the master fund allocation'
GO

if exists (select * from dbo.sysobjects where name ='PM_DayMarkPrice')
UPDATE PM_DayMarkPrice
SET FinalMarkPrice = 0
GO
 print 'Done Data update in :dbo.PM_DayMarkPrice'
GO


if exists (select * from dbo.sysobjects where name ='T_CurrencyConversionRate')
UPDATE T_CurrencyConversionRate
SET ConversionRate = 0
GO
 print 'Done Data Update in :dbo.T_CurrencyConversionRate'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AL_AllocationDefaultRule')
update T_AL_AllocationDefaultRule
set AllocationSchemeKey =0
GO
   print 'Set Allocation scheme Symbol wise.'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AL_AllocationDefaultRule')
update T_AL_AllocationDefaultRule
set SetSchemeFromUI =0
GO
   print 'Disable advanced prorata UI'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AL_AllocationDefaultRule')
update T_AL_AllocationDefaultRule
set ProrataSchemeName = 'Positions'
GO
   print 'Set Custom Pref name as Positions'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_ConfirmationPopUp')
update T_ConfirmationPopUp
set ISManualOrder = 'False', ISCXL ='False', ISCXLReplace = 'False', ISNewOrder='False'
GO
   print 'Disable TT Manual order Pop-up as well as confirmation pop ups'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AttributeNames')
UPDATE T_AttributeNames
SET KeepRecord = 1, DefaultValues = NULL
GO
   print 'Restoring Attribute Renaming colmumn default values.'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AutoGroupingPref')
UPDATE T_AutoGroupingPref
SET AutoGroup = 0, TradingAC = 1 , TradeDate = 1 where  CompanyID>0
GO
   print 'Restoring Autogrouping default values.'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_AutoGroupingFunds')
Update T_AutoGroupingFunds
SET AutoGroup = 0 where FundId >=0
GO
   print 'Restoring Autogrouping Account default values.'
GO

IF EXISTS  (SELECT * FROM dbo.sysobjects WHERE name = 'T_PranaKeyValuePreferences')
Update T_PranaKeyValuePreferences
SET PreferenceValue = -1 where PreferenceKey = 'AvgPriceRounding'
GO
   print 'Restoring Avg Price Rounding default values.'
GO

IF EXISTS  (SELECT * FROM dbo.sysobjects WHERE name = 'T_PranaKeyValuePreferences')
Update T_PranaKeyValuePreferences
SET PreferenceValue = 0 where PreferenceKey = 'IsShowMasterFundonTT' OR PreferenceKey = 'IsShowmasterFundAsClient'
GO
   print 'Restoring Show MasterFund on TT default values.'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name =  'T_TradingRulesPreferences')
Update  T_TradingRulesPreferences
SET IsOverSellTradingRule = 0, IsOverBuyTradingRule = 0,  IsUnallocatedTradeAlert = 0,  IsFatFingerTradingRule = 0,   IsDuplicateTradeAlert = 0,  IsPendingNewTradeAlert  = 0, DefineFatFingerValue = 0,  DuplicateTradeAlertTime = 0, PendingNewOrderAlertTime = 0, FatFingerAccountOrMasterFund = 0, IsAbsoluteAmountOrDefinePercent = 0, IsInMarketIncluded = 0, IsSharesOutstandingRule = 0, SharesOutstandingPercent = 0, SharesOutstandingAccountOrMF = 0 where CompanyID >0
GO
   print'Restoring Default Trading Compliance Rules.'
Go

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name =  'T_TransferTradeRules')
Update  T_TransferTradeRules
Set IsAllowRestrictedSecuritiesList = 0, IsAllowAllowedSecuritiesList = 0, IsAllowAllUserToCancelReplaceRemove = 0, IsDefaultOrderTypeLimitForMultiDay = 1 , IsAllowAllUserToTransferTrade = 0 , MasterUsersIDs = 17 where CompanyId>0
GO
   print'Restoring Default TT preferences in admin'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_RestrictedAllowedSecuritiesList')
Update T_RestrictedAllowedSecuritiesList 
Set Symbol = NULL , IsTickerSymbology = 1 where CompanyID>0;
GO
   print 'Removing restricted allowed list symbols'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_UserTTGeneralPreferences')
Update T_UserTTGeneralPreferences
Set QuantityType = 0 where UserID>0;
GO
   print 'Restoring TT UI Preferences Default Values'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_DollarAmountPermission')
Update T_DollarAmountPermission
Set TT=1,PTT=0;
GO
   print 'Restoring Dollar Amount Permission Default Values'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_UserTTGeneralPreferences')
Update T_UserTTGeneralPreferences
Set CounterPartyID=NULL where UserID=17
GO
	print 'Restoring the TT defualt Counter Party'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_CompanyTTGeneralPreferences')
Update T_CompanyTTGeneralPreferences
SET OrderTypeID = 1, CounterPartyID = 1, VenueID = 1, TimeInForceID = 1 , TradingAccountID = 11,IsShowTargetQTY = 1 where CompanyId > 0

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_PranaKeyValuePreferences')
update T_PranaKeyValuePreferences 
set PreferenceValue = 0 where PreferenceKey = 'IsEquityOptionManualValidation'

GO
   print 'Restoring Default TT UI Preferences from admin'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_SMSymbolLookUpTable')
Update T_SMSymbolLookUpTable
SET SharesOutstanding = 0 where SharesOutstanding > 0
GO
   print 'Restoring Default SharesOutstanding Value for all symbols' 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_CompanyUserModule')
delete  from T_CompanyUserModule where 
CompanyModuleID=(select CM.CompanyModuleID from T_Module M inner join T_CompanyModule CM on M.ModuleID = CM.ModuleID  
Where UPPER(ModuleName) = UPPER('Short Locate'))
GO
print 'Done Data Deletion in : dbo.T_CompanyUserModule'
GO

if exists (select * from dbo.sysobjects where name = 'T_ShortLocateDetails')
delete from T_ShortLocateDetails
GO
  print 'Done Data Deletion in :T_ShortLocateDetails'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_GlobalClosingPreferences')
Update T_GlobalClosingPreferences
SET OverrideGlobal = 0
GO
   print 'Restoring Override Global Algo Preference to false' 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_GlobalClosingPreferences')
Update T_GlobalClosingPreferences
SET SplitunderlyingBasedOnPosition = 0
GO
   print 'Restoring IsExerciseAssignCheckSideValidation Preference to false' 
GO

IF EXISTS (SELECT * FROM T_RebalPreferences WHERE preferencekey = 'OtherItemsImpactingNAV')
UPDATE T_RebalPreferences SET preferenceValue = '{"IsIncludeOtherAssetsMarketValue":false,"IsIncludeCash":true,"IsIncludeAccruals":true,"IsIncludeSwapNavAdjustment":false,"IsIncludeUnrealizedPnlOfSwaps":false}'
where preferencekey = 'OtherItemsImpactingNAV'
GO
   print 'Restoring OtherItemsImpactingNAV to Default' 
GO

IF EXISTS (SELECT * FROM T_RebalPreferences WHERE preferencekey = 'RebalTradingRulesPref')
UPDATE T_RebalPreferences SET preferenceValue = '{"IsReInvestCash":false,"IsSellToRaiseCash":false,"IsNoShorting":false,"IsNegativeCashAllowed":false}'
where preferencekey = 'RebalTradingRulesPref'
GO
   print 'Restoring RebalancePreference to false' 
GO

IF EXISTS (SELECT * FROM T_RebalPreferences WHERE preferencekey = 'RebalTradingRulesPref')
UPDATE T_RebalPreferences SET PreferenceValue = 'False'
WHERE preferencekey = 'RebalExpandAcrossSecurities'
GO
   print 'Restore Expanded Rebalance Across Securities to Default' 
GO

IF EXISTS (select * from dbo.sysobjects where name ='T_CA_CompliancePreferences')
UPDATE T_CA_CompliancePreferences 
SET ImportExportPath = 'E:\Compliance\Import-Export Path', PrePostCrossImport = 'true', InMarket = 'true', InStage = 'true', PostInMarket = 'false' , PostInStage = 'false', BlockTradeOnComplianceFaliure ='true', StageValueFromField = 'true' 
where CompanyID>0
GO
   print 'Restoring Compliance Alerts to default' 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_UserTTGeneralPreferences')
Update T_UserTTGeneralPreferences
SET Quantity = '1', IncrementOnQty = '1', IncrementOnStop = '0.25', IncrementOnLimit= '0.25', IsUseRoundLots = '0'
WHERE UserID=17
GO
	print 'Restoring the default TT UI preferenceValue'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_CounterParty')
Update T_CounterParty
set IsAlgoBroker = Null 
where ShortName = 'MS' or ShortName = 'CSFB';
GO
	print 'Restoring IsAlgoBroker in Broker Venue preferences'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_Samsara_OpenfinPageInfo')
DELETE FROM T_Samsara_OpenfinPageInfo
WHERE PageName <> 'PM';

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_RTPNL_UserWidgetConfigDetails')
DELETE FROM T_RTPNL_UserWidgetConfigDetails
WHERE ViewName NOT IN ('AccountLevelAggregation', 'FundLevelAggregation', 'RealTimeSymAccountMonitor','RealTimeSymbolFundMonitor', 'SummaryDashboard', 'SymbolLevelAggregation');

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_Samsara_CompanyUserLayouts')
DELETE FROM T_Samsara_CompanyUserLayouts
WHERE ViewName NOT IN ('SummaryDashboard', 'FundLevelAggregation', 'AccountLevelAggregation','SymbolLevelAggregation', 'RealTimeSymbolFundMonitor', 'RealTimeSymAccountMonitor');


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_Samsara_OpenfinWorkspaceInfo')
DELETE FROM T_Samsara_OpenfinWorkspaceInfo 

GO
print 'Deleleting Extra RTPNL Tables'
GO

IF EXISTS (SELECT * FROM T_CommissionCalculationTime)
Update T_CommissionCalculationTime
SET IsPostAllocatedCalculation = 1
GO
	print 'Restore Commission Rule to Post Allocation'
GO

IF EXISTS (SELECT * FROM T_TransferTradeRules)
Update T_TransferTradeRules
SET IsApplyLimitRulesForReplacingStagedOrders = 0, IsApplyLimitRulesForReplacingOtherOrders = 0, IsApplyLimitRulesForReplacingSubOrders = 0, IsAllowAllUserToChangeOrderType = 1
GO
	print 'Restore Rules for Replacing Trades from TT and Limit Price Rules'
GO

IF EXISTS (SELECT * FROM T_NavLock)
DELETE FROM T_NavLock
GO
    print 'Nav Lock Table Clear'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = 'T_UserTTGeneralPreferences')
Update T_UserTTGeneralPreferences
SET OrderTypeID = Null , TimeInForceID = Null , ExecutionInstructionID = Null, HandlingInstructionID= Null, 
TradingAccountID = Null, StrategyID= Null, AccountID = Null, IsSettlementCurrencyBase = '0', VenueID = Null
WHERE UserID=17
GO
	print 'Restoring the default TT UI preferenceValue'
GO

IF EXISTS (SELECT * FROM T_UserTTAssetPreferences)
Update T_UserTTAssetPreferences
SET SideID = Null
WHERE UserID=17
GO
	print 'Restoring the default TT UI AssetSpecificPreferences'
GO

IF EXISTS (SELECT * FROM T_PTTAccountPercentagePreference)
Update T_PTTAccountPercentagePreference
SET PercentInMasterFund = 0, AccountFactor = 1
GO
     print 'Reverting % Trading Tool UI Preference'
GO

IF EXISTS (SELECT * FROM T_CA_OtherCompliancePermission)
Update T_CA_OtherCompliancePermission
SET IsApplyToManual = 1, Trading = 1, Staging = 1
GO
     print 'Reverting PreTrade Compliance Permission queries'
GO

IF EXISTS (SELECT * FROM PM_Preferences)
UPDATE PM_Preferences 
SET XPercentofAvgVolume = 0.01, IsShowPMToolbar = 0
GO
     print 'Reverting PM Preferences'
GO

IF EXISTS (SELECT * FROM T_TTRiskNValidationSetting)
UPDATE T_TTRiskNValidationSetting
SET IsRiskChecked = 'False', IsValidateSymbolChecked = 'False', RiskValue = 1, LimitPriceChecked = 'False', SetExecutedQtytoZero = 'False' where CompanyUserID = 17
GO
     print 'Reverting TT Compliance Preference'
GO

IF EXISTS (SELECT * FROM T_TradeAudit)
delete from T_TradeAudit
GO
     print 'Done Data Deletion in :T_TradeAudit'
GO

IF EXISTS  (SELECT * FROM dbo.sysobjects WHERE name = 'T_PranaKeyValuePreferences')
Update T_PranaKeyValuePreferences
SET PreferenceValue = 0 where PreferenceKey = 'IsImportOverrideOnShortLocate'
GO
   print 'Restoring Import Override On ShortLocate default values.'
GO

IF EXISTS (SELECT * FROM T_GlobalClosingPreferences)
Update T_GlobalClosingPreferences
SET GlobalClosingAlgo = 2, OverrideGlobal = 0, IsShortWithBuyandBuyToClose = 0,
IsSellWithBuyToClose = 0, SplitunderlyingBasedOnPosition = 0, SecondarySort = 0,
GlobalClosingMethodology = -1, IsFetchDataAutomatically = 1, LongTermTaxRate = 1,
ShortTermTaxRate = 1, QtyRoundOffDigits = 8, PriceRoundOffDigits = 4, IsAutoCloseStrategy = 0,
GlobalClosingField = 0, AutoOptExerciseValue = 0.01
GO
   print 'Restoring Closing UI Preferences.'
GO
IF EXISTS (SELECT * FROM T_AL_AllocationPreferenceDef)
BEGIN
    DELETE FROM T_AL_AllocationPreferenceData 
    WHERE PresetdefId IN (
        SELECT id 
        FROM T_AL_AllocationPreferenceDef 
        WHERE IsPrefVisible = 1 
          AND DATEDIFF(DAY, UpdateDateTime, GETDATE()) BETWEEN 1 AND 2
        INTERSECT
        SELECT PresetdefId 
        FROM T_AL_AllocationPreferenceData
    )

    DELETE FROM T_AL_AllocationPreferenceDef 
    WHERE IsPrefVisible = 1 
      AND DATEDIFF(DAY, UpdateDateTime, GETDATE()) BETWEEN 1 AND 2

    PRINT 'Revert calculated preference'
END
    
IF EXISTS (SELECT * FROM T_AL_MFAllocationPreference)
DELETE FROM T_AL_MFWisePrefValues
DELETE FROM T_AL_MFAllocationPreference
GO
     print 'Delete Master Fund Preference'
GO

IF EXISTS (SELECT * FROM T_CompanyMarketDataProvider)
Update T_CompanyMarketDataProvider 
SET MarketDataProvider = 0;
GO
     print 'MarketDataProvider is set to default'
GO



IF EXISTS(SELECT * FROM T_CompanyUserModule)
delete from T_CompanyUserModule 
where CompanyModuleID = 1115;
GO
     print 'ShortLocate Permission is set to default'
GO

IF EXISTS(SELECT * FROM T_CA_RulesUserDefined)
WITH DuplicateCTE AS (
    SELECT ID,
        RuleName, 
        ROW_NUMBER() OVER (PARTITION BY RuleName ORDER BY (SELECT NULL)) AS row_num
    FROM T_CA_RulesUserDefined
)
delete T_CA_RulesUserDefined where ID IN(
SELECT ID FROM DuplicateCTE WHERE row_num > 1);


IF EXISTS(SELECT * FROM T_CompanyUserHotKeyPreferences)
update T_CompanyUserHotKeyPreferences set HotKeyPreferenceElements = 'Symbol^Quantity^Allocation^Broker^Venue^TIF^Expiry Date^Order Type^Order Side^Execution Type' , LastSavedTime = GETDATE() where CompanyUserID  = '35'
update T_CompanyUserHotKeyPreferences set HotKeyPreferenceElements = 'Symbol^Quantity^Allocation^Broker^Venue^TIF^Expiry Date^Order Type^Order Side^Execution Type' , LastSavedTime = GETDATE() where CompanyUserID  = '17'

IF EXISTS(SELECT * FROM T_PranaKeyValuePreferences)
update T_PranaKeyValuePreferences set PreferenceValue = 0 where PreferenceKey = 'IsShowmasterFundOnShortLocate'

IF EXISTS(SELECT * FROM T_CompanyUserModule)
DECLARE @User_ID int set @User_ID=(select UserID from T_CompanyUser where ShortName='jpearce')
delete  from T_CompanyUserModule where CompanyModuleID IN(select CM.CompanyModuleID from T_Module M inner join T_CompanyModule CM on M.ModuleID = CM.ModuleID  Where UPPER(ModuleName) = UPPER('Short Locate')) AND CompanyUserID=@User_ID


IF EXISTS(SELECT * FROM T_CompanyTTGeneralPreferences)
update T_CompanyTTGeneralPreferences set CounterPartyID = '1';
Update T_CompanyTTGeneralPreferences Set AccountID = '1186' where CompanyID>0
GO
     print 'Setting Default broker as "MS" and Default account as Allocation1'
GO


IF EXISTS(SELECT * FROM T_CompanyUserHotKeyPreferencesDetails)
TRUNCATE TABLE T_CompanyUserHotKeyPreferencesDetails 
GO
     print 'Deleting HotKeys'
GO