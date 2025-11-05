Declare @errormsg varchar(max)
Declare @FromDate datetime
Declare @ToDate datetime

set @errormsg=''
set @FromDate=''
set @ToDate=''

Select CompanyFundID into #tempFundID from T_Companyfunds


BEGIN

	Select GroupID 
	Into #T_TempGroupIDTable
	From V_Taxlots
	Where FundID  NOT IN (SELECT * FROM #tempFundID)
	
	Select ParentCLOrderId 
	Into #TempParentCLOrderId
	From T_TradedOrders

	Where GroupID NOT IN 
	(
	Select GroupID From #T_TempGroupIDTable
	)
	Select * into #T_Fills from T_Fills 
	Where CLOrderID NOT In
	(
		Select CLOrderID from T_Sub
			Where ParentCLOrderId NOT In
			(
				Select ParentCLOrderId from T_Order
				Where ParentCLOrderId NOT In
				(
					Select ParentCLOrderId From #TempParentCLOrderId
				)
			)
	)
	
	
if  exists( select * from #T_Fills)
begin
set @errormsg=@errormsg+'Obsolete Data in T_Fills.|'
select * from #T_Fills
END
	
	
	Select *  into #T_Sub from T_Sub
	Where ParentCLOrderId NOT In
	(
		Select ParentCLOrderId from T_Order
		Where ParentCLOrderId NOT In
		(
			Select ParentCLOrderId From #TempParentCLOrderId
		)
	)
	
if  exists( select * from #T_Sub)
begin
set @errormsg=@errormsg+'Obsolete Data in T_Sub.|'
select * from #T_Sub
END

	Select *  into #T_Order from T_Order
	Where ParentCLOrderId NOT In
	(
		Select ParentCLOrderId From #TempParentCLOrderId
	)
	
if  exists( select * from #T_Order)
begin
set @errormsg=@errormsg+'Obsolete Data in T_Order.|'
select * from #T_Order
END

	Select * into #T_TradedOrders from T_TradedOrders 
	Where GroupID NOT IN 
	(
		Select GroupID From #T_TempGroupIDTable
	)

if  exists( select * from #T_TradedOrders)
begin
set @errormsg=@errormsg+'Obsolete Data in T_TradedOrders.|'
select * from #T_TradedOrders
END
	
	Select *  into #PM_TaxlotClosing from PM_TaxlotClosing
	Where TaxlotClosingID NOT IN
	(
		Select TaxlotClosingID_FK from PM_Taxlots
		Where GroupID NOT IN 
		(
			Select GroupID From #T_TempGroupIDTable
		)
	)

if  exists( select * from #PM_TaxlotClosing)
begin
set @errormsg=@errormsg+'Obsolete Data in PM_TaxlotClosing.|'
select * from #PM_TaxlotClosing
END

	Select * into #T_PBWiseTaxlotState from T_PBWiseTaxlotState 
	Where TaxlotID NOT In
	(
		Select TaxlotID from T_Level2Allocation
		Where GroupID NOT IN 
		(
			Select GroupID From #T_TempGroupIDTable
		)
	)
	
if  exists( select * from #T_PBWiseTaxlotState)
begin
set @errormsg=@errormsg+'Obsolete Data in T_PBWiseTaxlotState.|'
select * from #T_PBWiseTaxlotState
END

	Select * into #T_CompanyThirdPartyMappingDetails from T_CompanyThirdPartyMappingDetails
	Where InternalFundNameID_FK NOT In (Select * From  #TempFundID)

if  exists( select * from #T_CompanyThirdPartyMappingDetails)
begin
set @errormsg=@errormsg+'Obsolete Data in T_CompanyThirdPartyMappingDetails.|'
select * from #T_CompanyThirdPartyMappingDetails
END

	Select * into #T_ThirdPartyPermittedFunds from T_ThirdPartyPermittedFunds
	Where CopanyFundID NOT In (Select * From  #TempFundID)	

if  exists( select * from #T_ThirdPartyPermittedFunds)
begin
set @errormsg=@errormsg+'Obsolete Data in T_ThirdPartyPermittedFunds.|'
select * from #T_ThirdPartyPermittedFunds
END


	Select * into #T_AllActivity from T_AllActivity
	Where FundID NOT In (Select * From  #TempFundID)

if  exists( select * from #T_AllActivity)
begin
set @errormsg=@errormsg+'Obsolete Data in T_AllActivity.|'
select * from #T_AllActivity
END

		
	Select * into #T_Journal from T_Journal
	Where FundID NOT In (Select * From  #TempFundID)

if  exists( select * from #T_Journal)
begin
set @errormsg=@errormsg+'Obsolete Data in T_Journal.|'
select * from #T_Journal
END


	
	Select * into #PM_NAVValue From PM_NAVValue
	Where FundID NOT In (Select * From  #TempFundID)

	if  exists( select * from #PM_NAVValue)
begin
set @errormsg=@errormsg+'Obsolete Data in PM_NAVValue.|'
select * from #PM_NAVValue
END
	Select * into #PM_CompanyFundCashCurrencyValue From PM_CompanyFundCashCurrencyValue
	Where FundID NOT In (Select * From  #TempFundID)

	if  exists( select * from #PM_CompanyFundCashCurrencyValue)
begin
set @errormsg=@errormsg+'Obsolete Data in PM_CompanyFundCashCurrencyValue.|'
select * from #PM_CompanyFundCashCurrencyValue
END

	Select * into #T_CashTransactions From T_CashTransactions
	Where FundID NOT In (Select * From  #TempFundID)
if  exists( select * from #T_CashTransactions)
begin
set @errormsg=@errormsg+'Obsolete Data in T_CashTransactions.|'
select * from #T_CashTransactions
END

	Select * into #PM_DayMarkPrice From PM_DayMarkPrice
	Where FundID NOT In (Select * From  #TempFundID)
if  exists( select * from #PM_DayMarkPrice)
begin
set @errormsg=@errormsg+'Obsolete Data in PM_DayMarkPrice.|'
select * from #PM_DayMarkPrice
END

	Select * into #T_CurrencyConversionRate From T_CurrencyConversionRate
	Where FundID NOT In (Select * From  #TempFundID)
	
if  exists( select * from #T_CurrencyConversionRate)
begin
set @errormsg=@errormsg+'Obsolete Data in T_CurrencyConversionRate.|'
select * from #T_CurrencyConversionRate
END


	IF OBJECT_ID (N'PM_FundWiseDayMarkPrice', N'U') IS NOT NULL 
		Begin
			Select * into #PM_FundWiseDayMarkPrice From PM_FundWiseDayMarkPrice
			Where FundID NOT In (Select * From  #TempFundID)
				if  exists( select * from #PM_FundWiseDayMarkPrice)
				begin
				set @errormsg=@errormsg+'Obsolete Data in PM_FundWiseDayMarkPrice.|'
				select * from #PM_FundWiseDayMarkPrice
				END

			End

	IF OBJECT_ID (N'T_FundWiseCurrencyConversionRate', N'U') IS NOT NULL 
		Begin
			Select * into #T_FundWiseCurrencyConversionRate From T_FundWiseCurrencyConversionRate
			Where FundID NOT In (Select * From  #TempFundID)
		
				if  exists( select * from #T_FundWiseCurrencyConversionRate)
				begin
				set @errormsg=@errormsg+'Obsolete Data in T_FundWiseCurrencyConversionRate.|'
				select * from #T_FundWiseCurrencyConversionRate
				END

		End

		
		
	IF OBJECT_ID (N'T_SubAccountCashValue', N'U') IS NOT NULL 
	Begin
		Select * into #T_SubAccountCashValue From T_SubAccountCashValue
		where FundID NOT In (Select * From  #TempFundID)
				if  exists( select * from #T_SubAccountCashValue)
				begin
				set @errormsg=@errormsg+'Obsolete Data in T_SubAccountCashValue.|'
				select * from #T_SubAccountCashValue
				END
	End

	IF OBJECT_ID (N'T_W_ClientFundMapping', N'U') IS NOT NULL 
		Begin
			Select * into #T_W_ClientFundMapping From T_W_ClientFundMapping
			Where TouchFundID NOT In 
			(
				Select TouchFundID from T_W_Funds
				Where PranafundID NOT In 
					(
						Select * From  #TempFundID
					)
			)	
				if  exists( select * from #T_W_ClientFundMapping)
				begin
				set @errormsg=@errormsg+'Obsolete Data in T_W_ClientFundMapping.|'
				select * from #T_W_ClientFundMapping
				END
		End

	IF OBJECT_ID (N'T_W_Funds', N'U') IS NOT NULL 
		Begin
			Select * into #T_W_Funds From T_W_Funds
			Where PranafundID NOT In (Select * From  #TempFundID)	
				if  exists( select * from #T_W_Funds)
				begin
				set @errormsg=@errormsg+'Obsolete Data in T_W_Funds.|'
				select * from #T_W_Funds
				END
		End

	Select * into #T_MW_Transactions from T_MW_Transactions
	Where Fund In
	(
		Select FundName From T_CompanyFunds
		Where CompanyFundID NOT In (Select * From  #TempFundID)
	)
				if  exists( select * from #T_MW_Transactions)
				begin
				set @errormsg=@errormsg+'Obsolete Data in T_MW_Transactions.|'
				select * from #T_MW_Transactions
				END
	Select fund,symbol,rundate,tradedate,pnl.masterfund,asset,pnl.Open_CloseTag  into #T_MW_GenericPNL from T_MW_GenericPNL pnl
	inner join T_companyFunds TCF on pnl.Fund=FundName
	Where TCF.CompanyFundID NOT In
	(
	Select * From  #TempFundID
	)
	
					if  exists( select * from #T_MW_GenericPNL)
				begin
				set @errormsg=@errormsg+'Obsolete Data in T_MW_GenericPNL.|'
				select * from #T_MW_GenericPNL
				END
				

	Select * into #PM_Taxlots from PM_Taxlots
	Where GroupID NOT IN 
	(
		Select GroupID From #T_TempGroupIDTable
	)
				if  exists( select * from #PM_Taxlots)
				begin
				set @errormsg=@errormsg+'Obsolete Data in PM_Taxlots.|'
				select * from #PM_Taxlots
				END
	Select * into #T_Level2Allocation from T_Level2Allocation
	Where GroupID NOT IN 
	(
		Select GroupID From #T_TempGroupIDTable
	)
	
					if  exists( select * from #T_Level2Allocation)
				begin
				set @errormsg=@errormsg+'Obsolete Data in T_Level2Allocation.|'
				select * from #T_Level2Allocation
				END
				
	Select * into #T_FundAllocation from T_FundAllocation
	Where GroupID NOT IN 
	(
		Select GroupID From #T_TempGroupIDTable
	)
					if  exists( select * from #T_FundAllocation)
				begin
				set @errormsg=@errormsg+'Obsolete Data in T_FundAllocation.|'
				select * from #T_FundAllocation
				END
	Select * into #T_Group from T_Group
	Where GroupID NOT IN 
	(
	Select GroupID From #T_TempGroupIDTable
	)
					if  exists( select * from #T_Group)
				begin
				set @errormsg=@errormsg+'Obsolete Data in T_Group.|'
				select * from #T_Group
				END

	Select * into #T_CompanyMasterFundSubAccountAssociation From T_CompanyMasterFundSubAccountAssociation
	Where CompanyFundID NOT In (Select * From  #TempFundID)

					if  exists( select * from #T_CompanyMasterFundSubAccountAssociation)
				begin
				set @errormsg=@errormsg+'Obsolete Data in T_CompanyMasterFundSubAccountAssociation.|'
				select * from #T_CompanyMasterFundSubAccountAssociation
				END
	Select * into #PM_DataSourceCompanyFund From PM_DataSourceCompanyFund
	Where CompanyFundID NOT In (Select * From  #TempFundID)

					if  exists( select * from #PM_DataSourceCompanyFund)
				begin
				set @errormsg=@errormsg+'Obsolete Data in PM_DataSourceCompanyFund.|'
				select * from #PM_DataSourceCompanyFund
				END
	Select * into #T_CompanyUserFunds From T_CompanyUserFunds 
	Where CompanyFundID  NOT In (Select * From  #TempFundID)
				
				if  exists( select * from #T_CompanyUserFunds)
				begin
				set @errormsg=@errormsg+'Obsolete Data in T_CompanyUserFunds.|!'
				select * from #T_CompanyUserFunds
				END

END


Select @errormsg as Errormsg

Drop Table #T_TempGroupIDTable, #TempFundID,#TempParentCLOrderId
