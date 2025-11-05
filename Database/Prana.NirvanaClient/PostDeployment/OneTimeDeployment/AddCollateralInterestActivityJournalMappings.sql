-------------------------------------------------AddCollateralInterestSubAccounts-------------------------------------
IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_SubAccounts'
			AND COLUMN_NAME = 'SubAccountID'
		)

BEGIN
DECLARE @SubAccountID INT
SET @SubAccountID =(SELECT max(SubAccountID) FROM T_SubAccounts)+1
INSERT INTO T_SubAccounts (
		SubAccountID
		, Name
		,Acronym
		,SubCategoryID
		,TransactionTypeID
		,IsFixedAccount
		,SubAccountTypeId
		)
	VALUES (   
	     @SubAccountID
		,'Collateral Interest Receivable'
		,'CollateralInterestReceivable'
		,2
		,1
		,0
		,NULL
		)
	SET @SubAccountID =@SubAccountID+1
INSERT INTO T_SubAccounts (
		SubAccountID
		, Name
		,Acronym
		,SubCategoryID
		,TransactionTypeID
		,IsFixedAccount
		,SubAccountTypeId
		)
	VALUES (   
	     @SubAccountID
		,'Collateral Interest Payable'
		,'CollateralInterestPayable'
		,5
		,1
		,0
		,NULL
		)
	SET @SubAccountID =@SubAccountID+1
INSERT INTO T_SubAccounts (
		SubAccountID
		, Name
		,Acronym
		,SubCategoryID
		,TransactionTypeID
		,IsFixedAccount
		,SubAccountTypeId
		)
	VALUES (   
	     @SubAccountID
		,'Collateral Interest Income'
		,'CollateralInterestIncome'
		,15
		,1
		,0
		,NULL
		)
	SET @SubAccountID =@SubAccountID+1
INSERT INTO T_SubAccounts (
		SubAccountID
		, Name
		,Acronym
		,SubCategoryID
		,TransactionTypeID
		,IsFixedAccount
		,SubAccountTypeId
		)
	VALUES (   
	     @SubAccountID
		,'Collateral Interest Expense'
		,'CollateralInterestExpense'
		,16
		,1
		,0
		,NULL
		)
END

--------------------------------------------------------------------------------------------------------------------------------

-------------------------------------------------AddCollateralInterestActivityType----------------------------------------------
IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_ActivityType'
			AND COLUMN_NAME = 'ActivityTypeId'
		)
BEGIN
 INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	  VALUES ( 
		'CollateralInterestReceivable'
		,'Collateral_Interest_Receivable'
		,NULL
		,2
		,0
		)
		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	  VALUES ( 
		'CollateralInterestPayable'
		,'Collateral_Interest_Payable'
		,NULL
		,2
		,0
		)
		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	  VALUES ( 
		'CollateralInterestReceived'
		,'Collateral_Interest_Received'
		,NULL
		,1
		,0
		)
		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	  VALUES ( 
		'CollateralInterestPaid'
		,'Collateral_Interest_Paid'
		,NULL
		,1
		,0
		)
END

--------------------------------------------------------------------------------------------------------------------------------

-------------------------------------------------AddCollateralInterestActivityJournalMapping-------------------------------------
IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_ActivityJournalMapping'
			AND COLUMN_NAME = 'Id'
		)

BEGIN
DECLARE @ActivityTypeId_FK INT
DECLARE @CreditAccount INT
DECLARE @DebitAccount INT

SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='CollateralInterestReceivable')
SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='CollateralInterestIncome')
SET @DebitAccount=(SELECT SubAccountID from  T_SubAccounts where Acronym='CollateralInterestReceivable')
INSERT INTO T_ActivityJournalMapping(
		ActivityTypeId_FK
		,AmountTypeId_FK
		,DebitAccount
		,CreditAccount
		,CashValueType
		,ActivityDateType
		)
	  VALUES ( 
		@ActivityTypeId_FK
		,8
		,@DebitAccount
		,@CreditAccount
		,0
		,1
		)
SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='CollateralInterestReceived')
SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='CollateralInterestReceivable')
SET @DebitAccount=(SELECT SubAccountID from  T_SubAccounts where Acronym='Cash')
INSERT INTO T_ActivityJournalMapping(
		ActivityTypeId_FK
		,AmountTypeId_FK
		,DebitAccount
		,CreditAccount
		,CashValueType
		,ActivityDateType
		)
	  VALUES ( 
		@ActivityTypeId_FK
		,8
		,@DebitAccount
		,@CreditAccount
		,0
		,1
		)
SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='CollateralInterestPayable')
SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='CollateralInterestPayable')
SET @DebitAccount=(SELECT SubAccountID from  T_SubAccounts where Acronym='CollateralInterestExpense')
INSERT INTO T_ActivityJournalMapping(
		ActivityTypeId_FK
		,AmountTypeId_FK
		,DebitAccount
		,CreditAccount
		,CashValueType
		,ActivityDateType
		)
	  VALUES ( 
		@ActivityTypeId_FK
		,8
		,@DebitAccount
		,@CreditAccount
		,1
		,1
		)
SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='CollateralInterestPaid')
SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='Cash')
SET @DebitAccount=(SELECT SubAccountID from  T_SubAccounts where Acronym='CollateralInterestPayable')
INSERT INTO T_ActivityJournalMapping(
		ActivityTypeId_FK
		,AmountTypeId_FK
		,DebitAccount
		,CreditAccount
		,CashValueType
		,ActivityDateType
		)
	  VALUES ( 
		@ActivityTypeId_FK
		,8
		,@DebitAccount
		,@CreditAccount
		,1
		,1
		)
END
