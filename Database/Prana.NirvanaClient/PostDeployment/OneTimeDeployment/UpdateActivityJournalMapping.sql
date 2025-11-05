-- Insert Pnl and FXPnl entries if not exist
if not exists(select * from T_ActivityAmountType where defaultSubaccountAcronym='Pnl')
begin
	INSERT INTO T_ActivityAmountType( AmountType, defaultSubaccountAcronym)	VALUES('PnL',	'PnL');
end
if not exists(select * from T_ActivityAmountType where defaultSubaccountAcronym='FXPnL')
begin
	INSERT INTO T_ActivityAmountType(AmountType, defaultSubaccountAcronym)	VALUES('FXPnL',	'FXPnL');
end

-- Variables
declare @activityTypeId int
declare @creditAccount int
declare @debitAccount int
declare @dateTypeForTradeAndEx int

-- Entries For Due From Broker TO Due To Broker
declare @subCategoryIdForCash int
declare @TransactionTypeIDForCash int

select @dateTypeForTradeAndEx=ActivityDateTypeId from T_ActivityDateType where ActivityDateType='Trade Date/Ex Date'
select @subCategoryIdForCash=SubCategoryID from T_SubCategory where SubCategoryName='Cash'
select @TransactionTypeIDForCash=TransactionTypeID from T_TransactionType where TransactionType='Cash'
if not exists (select * from T_SubAccounts where Acronym='DFB')
begin
	insert into T_SubAccounts(SubAccountID,Name,Acronym,SubCategoryId,TransactionTypeID,IsFixedAccount) values((select max(SubAccountId) from T_SubAccounts)+1,'Due From Broker','DFB',@subCategoryIdForCash,@TransactionTypeIDForCash,0)
end
if not exists (select * from T_SubAccounts where Acronym='DTB')
begin
	insert into T_SubAccounts(SubAccountID,Name,Acronym,SubCategoryId,TransactionTypeID,IsFixedAccount) values((select max(SubAccountId) from T_SubAccounts)+1,'Due To Broker','DTB',@subCategoryIdForCash,@TransactionTypeIDForCash,0)
end
-- FXL Entry
select @activityTypeId=ActivityTypeId from T_ActivityType where acronym='FXL'
declare @AmountTypeIdForAmount int
select @AmountTypeIdForAmount=AmountTypeId from T_ActivityAmountType where AmountType='Amount'
update T_ActivityJournalMapping 
set DebitAccount=(select SubAccountId from T_SubAccounts where  Acronym='DFB'),
    CreditAccount=(select SubAccountId from T_SubAccounts where  Acronym='DTB')
	where ActivityTypeId_FK=@activityTypeId and AmountTypeId_FK=@AmountTypeIdForAmount

-- fxl_currencySettled
select @activityTypeId=ActivityTypeId from T_ActivityType where acronym='FXL_CurrencySettled'

declare @AmountTypeIdForClosedQty int
select @AmountTypeIdForClosedQty=AmountTypeId from T_ActivityAmountType where AmountType='ClosedQty'

update T_ActivityJournalMapping 
set DebitAccount=(select SubAccountId from T_SubAccounts where  Acronym='DTB'),
    CreditAccount=(select SubAccountId from T_SubAccounts where  Acronym='DFB')
	where ActivityTypeId_FK=@activityTypeId and AmountTypeId_FK=@AmountTypeIdForClosedQty
delete from T_ActivityJournalMapping where ActivityTypeId_FK=@activityTypeId and AmountTypeId_FK=@AmountTypeIdForAmount

-- fx_Settle
select @activityTypeId=ActivityTypeId from T_ActivityType where acronym='FX_Settled'
select @creditAccount=SubAccountId from T_SubAccounts where acronym='FX_PNL'
select @debitAccount=SubAccountId from T_SubAccounts where acronym='Cash'
declare @subAccountForCash int
select @subAccountForCash=SubAccountId from T_SubAccounts where acronym='Cash'
update T_ActivityJournalMapping set DebitAccount=@subAccountForCash,CreditAccount=@subAccountForCash where ActivityTypeId_FK=@activityTypeId and AmountTypeId_FK=@AmountTypeIdForAmount

declare @AmountTypeIdForPnl int
declare @AmountTypeIdForFXPnl int
select @AmountTypeIdForPnl=AmountTypeId from T_ActivityAmountType where AmountType='PnL'
select @AmountTypeIdForFXPnl=AmountTypeId from T_ActivityAmountType where AmountType='FXPnL'

if not exists(select * from T_ActivityJournalMapping where [ActivityTypeId_FK]=@activityTypeId and [AmountTypeId_FK]=@AmountTypeIdForPnl)
begin
	insert into T_ActivityJournalMapping([ActivityTypeId_FK], [AmountTypeId_FK], [DebitAccount], [CreditAccount], [CashValueType], [ActivityDateType]) values(@activityTypeId,@AmountTypeIdForPnl,@debitAccount,@creditAccount,0,@dateTypeForTradeAndEx)
end

-- fxForwardSettle
select @activityTypeId=ActivityTypeId from T_ActivityType where acronym='FxForward_Settled'
select @creditAccount=SubAccountId from T_SubAccounts where acronym='Cash'
update T_ActivityJournalMapping set DebitAccount=@debitAccount,CreditAccount=@creditAccount where ActivityTypeId_FK=@activityTypeId and AmountTypeId_FK=@AmountTypeIdForAmount
if not exists(select * from T_ActivityJournalMapping where [ActivityTypeId_FK]=@activityTypeId and [AmountTypeId_FK]=@AmountTypeIdForPnl)
begin
	select @creditAccount=SubAccountId from T_SubAccounts where acronym='FXForwardRealizedPNL'
	insert into T_ActivityJournalMapping([ActivityTypeId_FK], [AmountTypeId_FK], [DebitAccount], [CreditAccount], [CashValueType], [ActivityDateType]) values(@activityTypeId,@AmountTypeIdForPnl,@debitAccount,@creditAccount,0,@dateTypeForTradeAndEx)
end
if not exists(select * from T_ActivityJournalMapping where [ActivityTypeId_FK]=@activityTypeId and [AmountTypeId_FK]=@AmountTypeIdForFXPnl)
begin
	select @creditAccount=SubAccountId from T_SubAccounts where acronym='FX_PNL'
	insert into T_ActivityJournalMapping([ActivityTypeId_FK], [AmountTypeId_FK], [DebitAccount], [CreditAccount], [CashValueType], [ActivityDateType]) values(@activityTypeId,@AmountTypeIdForFXPnl,@debitAccount,@creditAccount,0,@dateTypeForTradeAndEx)
end	
						
-- fxLongUnrealizedPnl
select @creditAccount=SubAccountId from T_SubAccounts where acronym='FX_PNL'
select @debitAccount=SubAccountId from T_SubAccounts where acronym='FXLongRevaluation'
select @activityTypeId=ActivityTypeId from T_ActivityType where acronym='FXLongUnRealizedPNL'

update T_ActivityJournalMapping set DebitAccount=@debitAccount,CreditAccount=@creditAccount where ActivityTypeId_FK=@activityTypeId and AmountTypeId_FK=8
declare @subCategoryIdForProfitAndLoss int
declare @TransactionTypeIDForTradeTransaction int

select @subCategoryIdForProfitAndLoss=SubCategoryID from T_SubCategory where SubCategoryName='Profit And Loss'
select @TransactionTypeIDForTradeTransaction=TransactionTypeID from T_TransactionType where TransactionType='Trade Transaction'
if not exists (select * from T_SubAccounts where acronym='FXLongRevaluation')
begin
	insert into T_SubAccounts(SubAccountID,Name,Acronym,SubCategoryId,TransactionTypeID,IsFixedAccount) Values((select max(SubAccountId) from T_SubAccounts)+1,'Unrealized FX Gain/Loss','FXLongRevaluation',@subCategoryIdForProfitAndLoss,@TransactionTypeIDForTradeTransaction,1)
end
else
begin 
	update T_SubAccounts set name='Unrealized FX Gain/Loss' where acronym='FXLongRevaluation'
end

if not exists (select * from T_SubAccounts where acronym='FX_PNL')
begin
	insert into T_SubAccounts Values((select max(SubAccountId) from T_SubAccounts)+1,'FX Profit And Loss','FX_PNL', @subCategoryIdForProfitAndLoss, @TransactionTypeIDForTradeTransaction, 1, NULL)
end
else
begin 
	update T_SubAccounts set name='FX Profit And Loss' where acronym='FX_PNL'
end

-- CashUnRealizedPNL
select @creditAccount=SubAccountId from T_SubAccounts where acronym='FX_PNL'
select @debitAccount=SubAccountId from T_SubAccounts where acronym='Cash'
select @activityTypeId=ActivityTypeId from T_ActivityType where acronym='CashUnRealizedPNL'

update T_ActivityJournalMapping set DebitAccount=@debitAccount,CreditAccount=@creditAccount where ActivityTypeId_FK=@activityTypeId and AmountTypeId_FK=@AmountTypeIdForAmount

select @creditAccount=SubAccountId from T_SubAccounts where acronym='FX_PNL'
select @debitAccount=SubAccountId from T_SubAccounts where acronym='FXLongRevaluation'
select @activityTypeId=ActivityTypeId from T_ActivityType where acronym='FXShortUnRealizedPNL'
update T_ActivityJournalMapping set DebitAccount=@debitAccount,CreditAccount=@creditAccount where ActivityTypeId_FK=@activityTypeId and AmountTypeId_FK= @AmountTypeIdForAmount

-- FXForwardShortAndLongPNLActivityJournalMapping.sql --

DECLARE @Id INT
	DEClARE @AmountTypeId_FK INT
	DECLARE @CashValueType INT
	DECLARE @ActivityDateType INT
	DECLARE @ActivityTypeId_FK INT
	
declare @count int
select @count =count(*) from T_ActivityJournalMapping where ActivityTypeId_FK in(select ActivityTypeId from T_ActivityType where 
        Acronym in ('FxForwardLongLT_Settled','FxForwardLongST_Settled','FxForwardShortST_Settled'))


if(@count !=9)
Begin
IF EXISTS (
	 select ActivityTypeId from T_ActivityType where 
	Acronym in ('FxForwardLongLT_Settled','FxForwardLongST_Settled','FxForwardShortST_Settled') 
	)
	begin
		delete from T_ActivityJournalMapping where ActivityTypeId_FK in(select ActivityTypeId from T_ActivityType where 
        Acronym in ('FxForwardLongLT_Settled','FxForwardLongST_Settled','FxForwardShortST_Settled'))
		
CREATE TABLE #TempActivityJournalMapping(
	[ActivityTypeId_FK] [int] NOT NULL,
	[AmountTypeId_FK] [int] NOT NULL,
	[DebitAccount] [int] NULL,
	[CreditAccount] [int] NULL,
	[CashValueType] [tinyint] NOT NULL,
	[ActivityDateType] [int] NOT NULL
	)

-------FxForwardLongLT_Settled----------------------------------   
SET @CashValueType =0
SET @ActivityDateType =1
SET @ActivityTypeId_FK=(Select ActivityTypeId  from T_ActivityType where Acronym in ('FxForwardLongLT_Settled'))
SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym in ('Cash'))
SET @DebitAccount=(Select SubAccountID from T_SubAccounts where Acronym in ('Cash'))
SET @AmountTypeId_FK =(select AmountTypeId from T_ActivityAmountType where AmountType  in ('Amount'))
	
	
		INSERT INTO #TempActivityJournalMapping(
		ActivityTypeId_FK
		,AmountTypeId_FK
		,DebitAccount
		,CreditAccount
		,CashValueType
		,ActivityDateType
		)
	  VALUES ( 
		 @ActivityTypeId_FK
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym in ('FXForwardRealizedPNL(LT)'))
		 SET @AmountTypeId_FK =(select AmountTypeId from T_ActivityAmountType where AmountType  in ('PnL'))
		 INSERT INTO #TempActivityJournalMapping(
		ActivityTypeId_FK
		,AmountTypeId_FK
		,DebitAccount
		,CreditAccount
		,CashValueType
		,ActivityDateType
		)
	  VALUES ( 
		 @ActivityTypeId_FK
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
	 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym in ('FX_PNL'))
	 SET @AmountTypeId_FK =(select AmountTypeId from T_ActivityAmountType where AmountType  in ('FXPnL'))
		 INSERT INTO #TempActivityJournalMapping(
		ActivityTypeId_FK
		,AmountTypeId_FK
		,DebitAccount
		,CreditAccount
		,CashValueType
		,ActivityDateType
		)
	  VALUES ( 
	 	@ActivityTypeId_FK
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)

-------FxForwardLongST_Settled----------------------------------

		SET @CashValueType =0
SET @ActivityDateType =1
SET @ActivityTypeId_FK=(Select ActivityTypeId  from T_ActivityType where Acronym in ('FxForwardLongST_Settled'))
SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym in ('Cash'))
SET @DebitAccount=(Select SubAccountID from T_SubAccounts where Acronym in ('Cash'))
SET @AmountTypeId_FK =(select AmountTypeId from T_ActivityAmountType where AmountType  in ('Amount'))
		

		
		INSERT INTO #TempActivityJournalMapping(
		ActivityTypeId_FK
		,AmountTypeId_FK
		,DebitAccount
		,CreditAccount
		,CashValueType
		,ActivityDateType
		)
	  VALUES ( 
		 @ActivityTypeId_FK
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym in ('FXForwardRealizedPNL(ST)'))
		 SET @AmountTypeId_FK =(select AmountTypeId from T_ActivityAmountType where AmountType  in ('PnL'))
		 INSERT INTO #TempActivityJournalMapping(
		ActivityTypeId_FK
		,AmountTypeId_FK
		,DebitAccount
		,CreditAccount
		,CashValueType
		,ActivityDateType
		)
	  VALUES ( 
		 @ActivityTypeId_FK
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
	 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym in ('FX_PNL'))
	 SET @AmountTypeId_FK =(select AmountTypeId from T_ActivityAmountType where AmountType  in ('FXPnL'))
		 INSERT INTO #TempActivityJournalMapping(
		ActivityTypeId_FK
		,AmountTypeId_FK
		,DebitAccount
		,CreditAccount
		,CashValueType
		,ActivityDateType
		)
	  VALUES ( 
	 	@ActivityTypeId_FK
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)




-------FxForwardShortST_Settled----------------------------------

		SET @CashValueType =0
SET @ActivityDateType =1
SET @ActivityTypeId_FK=(Select ActivityTypeId  from T_ActivityType where Acronym in ('FxForwardShortST_Settled'))
SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym in ('Cash'))
SET @DebitAccount=(Select SubAccountID from T_SubAccounts where Acronym in ('Cash'))
SET @AmountTypeId_FK =(select AmountTypeId from T_ActivityAmountType where AmountType  in ('Amount'))
		

		
		INSERT INTO #TempActivityJournalMapping(
		ActivityTypeId_FK
		,AmountTypeId_FK
		,DebitAccount
		,CreditAccount
		,CashValueType
		,ActivityDateType
		)
	  VALUES ( 
		 @ActivityTypeId_FK
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym in ('FXForwardShortRealizedPNL'))
		 SET @AmountTypeId_FK =(select AmountTypeId from T_ActivityAmountType where AmountType  in ('PnL'))
		 INSERT INTO #TempActivityJournalMapping(
		ActivityTypeId_FK
		,AmountTypeId_FK
		,DebitAccount
		,CreditAccount
		,CashValueType
		,ActivityDateType
		)
	  VALUES ( 
		 @ActivityTypeId_FK
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
	 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym in ('FX_PNL'))
	 SET @AmountTypeId_FK =(select AmountTypeId from T_ActivityAmountType where AmountType  in ('FXPnL'))
		 INSERT INTO #TempActivityJournalMapping(
		ActivityTypeId_FK
		,AmountTypeId_FK
		,DebitAccount
		,CreditAccount
		,CashValueType
		,ActivityDateType
		)
	  VALUES ( 
	 	@ActivityTypeId_FK
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)

		
		 INSERT INTO T_ActivityJournalMapping(
	    	ActivityTypeId_FK
		,AmountTypeId_FK
		,DebitAccount
		,CreditAccount
		,CashValueType
		,ActivityDateType
		)
	 select ActivityTypeId_FK
		,AmountTypeId_FK
		,DebitAccount
		,CreditAccount
		,CashValueType
		,ActivityDateType
		From #TempActivityJournalMapping

       drop table #TempActivityJournalMapping
		END
end