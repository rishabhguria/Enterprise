-------------------------------------------------AddRealizedPNLLongTermShortTermSubAccounts-------------------------------------
IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_SubAccounts'
			AND COLUMN_NAME = 'SubAccountID'
		)

BEGIN
DECLARE @SubAccountID INT
DEClARE @SubCategoryID INT
DECLARE @TransactionTypeID INT
DECLARE @IsFixedAccount INT
SET @SubAccountID =(SELECT max(SubAccountID) FROM T_SubAccounts)+1

	IF EXISTS (
		SELECT * from  T_SubAccounts where Acronym='EquityRealizedPNL'
		)
	 BEGIN
	 SET @SubCategoryID =(Select SubCategoryID From T_SubAccounts where Acronym ='EquityRealizedPNL')
	 SET @TransactionTypeID =(Select TransactionTypeID From T_SubAccounts where Acronym ='EquityRealizedPNL')
	 SET @IsFixedAccount =(Select IsFixedAccount From T_SubAccounts where Acronym ='EquityRealizedPNL')
	 
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
		,'Equity Long Realized Profit And Loss (LT)'
		,'EquityRealizedPNL(LT)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'Equity Long Realized Profit And Loss (ST)'
		,'EquityRealizedPNL(ST)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'Equity Short Realized Profit And Loss (ST)'
		,'EquityShortRealizedPNL'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
		,NULL
		)
		END

		IF EXISTS (
		SELECT * from  T_SubAccounts where Acronym='EquityOptionRealizedPNL'
		)
		BEGIN
		SET @SubCategoryID =(Select SubCategoryID From T_SubAccounts where Acronym ='EquityOptionRealizedPNL')
		SET @TransactionTypeID =(Select TransactionTypeID From T_SubAccounts where Acronym ='EquityOptionRealizedPNL')
		SET @IsFixedAccount =(Select IsFixedAccount From T_SubAccounts where Acronym ='EquityOptionRealizedPNL')
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
		,'Equity Option Long Realized Profit And Loss (LT)'
		,'EquityOptionRealizedPNL(LT)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'Equity Option Long Realized Profit And Loss (ST)'
		,'EquityOptionRealizedPNL(ST)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'Equity Option Short Realized Profit And Loss (ST)'
		,'EquityOptionShortRealizedPNL'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
		,NULL
		)
		END

		IF EXISTS (
		SELECT * from  T_SubAccounts where Acronym='EquitySwapRealizedPNL'
		)
		BEGIN
		SET @SubCategoryID =(Select SubCategoryID From T_SubAccounts where Acronym ='EquitySwapRealizedPNL')
		SET @TransactionTypeID =(Select TransactionTypeID From T_SubAccounts where Acronym ='EquitySwapRealizedPNL')
		SET @IsFixedAccount =(Select IsFixedAccount From T_SubAccounts where Acronym ='EquitySwapRealizedPNL')
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
		,'Equity Swap Long Realized Profit And Loss (LT)'
		,'EquitySwapRealizedPNL(LT)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'Equity Swap Long Realized Profit And Loss (ST)'
		,'EquitySwapRealizedPNL(ST)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'Equity Swap Short Realized Profit And Loss (ST)'
		,'EquitySwapShortRealizedPNL'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
		,NULL
		)
		END

		IF EXISTS (
		SELECT * from  T_SubAccounts where Acronym='BondRealizedPNL'
		)
		BEGIN
		SET @SubCategoryID =(Select SubCategoryID From T_SubAccounts where Acronym ='BondRealizedPNL')
		SET @TransactionTypeID =(Select TransactionTypeID From T_SubAccounts where Acronym ='BondRealizedPNL')
		SET @IsFixedAccount =(Select IsFixedAccount From T_SubAccounts where Acronym ='BondRealizedPNL')
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
		,'Bond Long Realized Profit And Loss (LT)'
		,'BondRealizedPNL(LT)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'Bond Long Realized Profit And Loss (ST)'
		,'BondRealizedPNL(ST)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'Bond Short Realized Profit And Loss (ST)'
		,'BondShortRealizedPNL'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
		,NULL
		)
		END

		IF EXISTS (
		SELECT * from  T_SubAccounts where Acronym='FutureOptionRealizedPNL'
		)
		BEGIN
		SET @SubCategoryID =(Select SubCategoryID From T_SubAccounts where Acronym ='FutureOptionRealizedPNL')
		SET @TransactionTypeID =(Select TransactionTypeID From T_SubAccounts where Acronym ='FutureOptionRealizedPNL')
		SET @IsFixedAccount =(Select IsFixedAccount From T_SubAccounts where Acronym ='FutureOptionRealizedPNL')
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
		,'Future Option Long Realized Profit And Loss (LT)'
		,'FutureOptionRealizedPNL(LT)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'Future Option Long Realized Profit And Loss (ST)'
		,'FutureOptionRealizedPNL(ST)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'Future Option Short Realized Profit And Loss (ST)'
		,'FutureOptionShortRealizedPNL'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
		,NULL
		)
		END

		IF EXISTS (
		SELECT * from  T_SubAccounts where Acronym='FutureRealizedPNL'
		)
		BEGIN
		SET @SubCategoryID =(Select SubCategoryID From T_SubAccounts where Acronym ='FutureRealizedPNL')
		SET @TransactionTypeID =(Select TransactionTypeID From T_SubAccounts where Acronym ='FutureRealizedPNL')
		SET @IsFixedAccount =(Select IsFixedAccount From T_SubAccounts where Acronym ='FutureRealizedPNL')
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
		,'Future Long Realized Profit And Loss (ST)'
		,'FutureLongRealizedPNL(ST)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'Future Long Realized Profit And Loss (LT)'
		,'FutureLongRealizedPNL(LT)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'Future Short Realized Profit And Loss (ST)'
		,'FutureShortRealizedPNL(ST)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'Future Short Realized Profit And Loss (LT)'
		,'FutureShortRealizedPNL(LT)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
		,NULL
		)
		END

		IF EXISTS (
		SELECT * from  T_SubAccounts where Acronym='FXForwardRealizedPNL'
		)
		BEGIN
		SET @SubCategoryID =(Select SubCategoryID From T_SubAccounts where Acronym ='FXForwardRealizedPNL')
		SET @TransactionTypeID =(Select TransactionTypeID From T_SubAccounts where Acronym ='FXForwardRealizedPNL')
		SET @IsFixedAccount =(Select IsFixedAccount From T_SubAccounts where Acronym ='FXForwardRealizedPNL')
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
		,'FX Forward Long Realized Profit And Loss (LT)'
		,'FXForwardRealizedPNL(LT)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'FX Forward Long Realized Profit And Loss (ST)'
		,'FXForwardRealizedPNL(ST)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'FX Forward Short Realized Profit And Loss (ST)'
		,'FXForwardShortRealizedPNL'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
		,NULL
		)
		END

		IF EXISTS (
		SELECT * from  T_SubAccounts where Acronym='FXRealizedPNL'
		)
		BEGIN
		SET @SubCategoryID =(Select SubCategoryID From T_SubAccounts where Acronym ='FXRealizedPNL')
		SET @TransactionTypeID =(Select TransactionTypeID From T_SubAccounts where Acronym ='FXRealizedPNL')
		SET @IsFixedAccount =(Select IsFixedAccount From T_SubAccounts where Acronym ='FXRealizedPNL')
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
		,'FX Long Realized Profit And Loss (LT)'
		,'FXRealizedPNL(LT)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'FX Long Realized Profit And Loss (ST)'
		,'FXRealizedPNL(ST)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'FX Short Realized Profit And Loss (ST)'
		,'FXShortRealizedPNL'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
		,NULL
		)
		END

		IF EXISTS (
		SELECT * from  T_SubAccounts where Acronym='PrivateEquityRealizedPNL'
		)
		BEGIN
		SET @SubCategoryID =(Select SubCategoryID From T_SubAccounts where Acronym ='PrivateEquityRealizedPNL')
		SET @TransactionTypeID =(Select TransactionTypeID From T_SubAccounts where Acronym ='PrivateEquityRealizedPNL')
		SET @IsFixedAccount =(Select IsFixedAccount From T_SubAccounts where Acronym ='PrivateEquityRealizedPNL')
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
		,'Private Equity Long Realized Profit And Loss (LT)'
		,'PrivateEquityRealizedPNL(LT)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'Private Equity Long Realized Profit And Loss (ST)'
		,'PrivateEquityRealizedPNL(ST)'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
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
		,'Private Equity Short Realized Profit And Loss (ST)'
		,'PrivateEquityShortRealizedPNL'
		,@SubCategoryID
		,@TransactionTypeID
		,@IsFixedAccount
		,NULL
		)
		END
END
--------------------------------------------------------------------------------------------------------------------------------

-------------------------------------------------AddRealizedPNLLongTermShortTermActivityType-------------------------------------
IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_ActivityType'
			AND COLUMN_NAME = 'ActivityTypeId'
		)

BEGIN
	DEClARE @ActvitySource INT
	DECLARE @BalanceType INT


	 IF EXISTS (
		SELECT * from  T_ActivityType where Acronym='EquityLongRealizedPNL'
		)
	 BEGIN
	 SET @ActvitySource =(Select ActivitySource From T_ActivityType where Acronym ='EquityLongRealizedPNL')
	 SET @BalanceType =(Select BalanceType From T_ActivityType where Acronym ='EquityLongRealizedPNL')
	 
	 INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	  VALUES ( 
		'EquityLongRealizedPNL (LT)'
		,'EquityLongRealizedPNL(LT)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)

		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	    VALUES (  
		'EquityLongRealizedPNL (ST)'
		,'EquityLongRealizedPNL(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)

		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     
		'EquityShortRealizedPNL (ST)'
		,'EquityShortRealizedPNL(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)
		END

		IF EXISTS (
		SELECT * from  T_ActivityType where Acronym='EquityOptionLongRealizedPNL'
		)
	 BEGIN
	 SET @ActvitySource =(Select ActivitySource From T_ActivityType where Acronym ='EquityOptionLongRealizedPNL')
	 SET @BalanceType =(Select BalanceType From T_ActivityType where Acronym ='EquityOptionLongRealizedPNL')
	 
	 INSERT INTO T_ActivityType (
		
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
		'EquityOptionLongRealizedPNL (LT)'
		,'EquityOptionLongRealizedPNL(LT)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)

		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'EquityOptionLongRealizedPNL (ST)'
		,'EquityOptionLongRealizedPNL(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)
		
		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'EquityOptionShortRealizedPNL (ST)'
		,'EquityOptionShortRealizedPNL(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)
		END

		IF EXISTS (
		SELECT * from  T_ActivityType where Acronym='EquitySwapLongRealizedPNL'
		)
	 BEGIN
	 SET @ActvitySource =(Select ActivitySource From T_ActivityType where Acronym ='EquitySwapLongRealizedPNL')
	 SET @BalanceType =(Select BalanceType From T_ActivityType where Acronym ='EquitySwapLongRealizedPNL')
	 
	 INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'EquitySwapLongRealizedPNL (LT)'
		,'EquitySwapLongRealizedPNL(LT)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)

		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'EquitySwapLongRealizedPNL (ST)'
		,'EquitySwapLongRealizedPNL(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)

		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
		'EquitySwapShortRealizedPNL (ST)'
		,'EquitySwapShortRealizedPNL(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)
		END

		IF EXISTS (
		SELECT * from  T_ActivityType where Acronym='BondLongRealizedPNL'
		)
	 BEGIN
	 SET @ActvitySource =(Select ActivitySource From T_ActivityType where Acronym ='BondLongRealizedPNL')
	 SET @BalanceType =(Select BalanceType From T_ActivityType where Acronym ='BondLongRealizedPNL')
	 
	 INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'BondLongRealizedPNL (LT)'
		,'BondLongRealizedPNL(LT)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)

		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'BondLongRealizedPNL (ST)'
		,'BondLongRealizedPNL(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)

		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'BondShortRealizedPNL (ST)'
		,'BondShortRealizedPNL(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)
		END

		IF EXISTS (
		SELECT * from  T_ActivityType where Acronym='FutureRealizedPNL'
		)
			 BEGIN
	 SET @ActvitySource =(Select ActivitySource From T_ActivityType where Acronym ='FutureRealizedPNL')
	 SET @BalanceType =(Select BalanceType From T_ActivityType where Acronym ='FutureRealizedPNL')
	 
	 INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'FutureLongRealizedPNL (LT)'
		,'FutureLongRealizedPNL(LT)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)

 	INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'FutureLongRealizedPNL (ST)'
		,'FutureLongRealizedPNL(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)

		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'FutureShortRealizedPNL (ST)'
		,'FutureShortRealizedPNL(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)
		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'FutureShortRealizedPNL (LT)'
		,'FutureShortRealizedPNL(LT)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)
		END

		IF EXISTS (
		SELECT * from  T_ActivityType where Acronym='FutureOptionLongRealizedPNL'
		)
		BEGIN
	 SET @ActvitySource =(Select ActivitySource From T_ActivityType where Acronym ='FutureOptionLongRealizedPNL')
	 SET @BalanceType =(Select BalanceType From T_ActivityType where Acronym ='FutureOptionLongRealizedPNL')
	 
	 INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'FutureOptionLongRealizedPNL (LT)'
		,'FutureOptionLongRealizedPNL(LT)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)

		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'FutureOptionLongRealizedPNL (ST)'
		,'FutureOptionLongRealizedPNL(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)

		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'FutureOptionShortRealizedPNL (ST)'
		,'FutureOptionShortRealizedPNL(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)
		END

		IF EXISTS (
		SELECT * from  T_ActivityType where Acronym='FXLongRealizedPNL'
		)
		BEGIN
		SET @ActvitySource =(Select ActivitySource From T_ActivityType where Acronym ='FXLongRealizedPNL')
	 SET @BalanceType =(Select BalanceType From T_ActivityType where Acronym ='FXLongRealizedPNL')
	 
	 INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'FXLongRealizedPNL (LT)'
		,'FXLongRealizedPNL(LT)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)

		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'FXLongRealizedPNL (ST)'
		,'FXLongRealizedPNL(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)

		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'FXShortRealizedPNL (ST)'
		,'FXShortRealizedPNL(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)
		END

		IF EXISTS (
		SELECT * from  T_ActivityType where Acronym='FXForwardLongRealizedPNLActivityId'
		)
		BEGIN
		SET @ActvitySource =(Select ActivitySource From T_ActivityType where Acronym ='FXForwardLongRealizedPNLActivityId')
	 SET @BalanceType =(Select BalanceType From T_ActivityType where Acronym ='FXForwardLongRealizedPNLActivityId')
	 
	 INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'FXForwardLongRealizedPNLActivityId (LT)'
		,'FXForwardLongRealizedPNLActivityId(LT)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)

		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'FXForwardLongRealizedPNLActivityId (ST)'
		,'FXForwardLongRealizedPNLActivityId(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)

		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'FXForwardShortRealizedPNLActivityId (ST)'
		,'FXForwardShortRealizedPNLActivityId(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)
		END

		IF EXISTS (
		SELECT * from  T_ActivityType where Acronym='PrivateEquityLongRealizedPNL'
		)
		BEGIN
		SET @ActvitySource =(Select ActivitySource From T_ActivityType where Acronym ='PrivateEquityLongRealizedPNL')
	 SET @BalanceType =(Select BalanceType From T_ActivityType where Acronym ='PrivateEquityLongRealizedPNL')
	 
	 INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'PrivateEquityLongRealizedPNL (LT)'
		,'PrivateEquityLongRealizedPNL(LT)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)

		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'PrivateEquityLongRealizedPNL (ST)'
		,'PrivateEquityLongRealizedPNL(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)

		INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'PrivateEquityShortRealizedPNL (ST)'
		,'PrivateEquityShortRealizedPNL(ST)'
		,NULL
		,@BalanceType
		,@ActvitySource
		)
		END

		IF EXISTS (
		SELECT * from  T_ActivityType where Acronym='FxForward_Settled'
		)
		BEGIN
		SET @ActvitySource =(Select ActivitySource From T_ActivityType where Acronym ='FxForward_Settled')
	 SET @BalanceType =(Select BalanceType From T_ActivityType where Acronym ='FxForward_Settled')
	 
	 INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'FxForwardLongLT_Settled'
		,'FxForwardLongLT_Settled'
		,NULL
		,@BalanceType
		,@ActvitySource
		)
 INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'FxForwardLongST_Settled'
		,'FxForwardLongST_Settled'
		,NULL
		,@BalanceType
		,@ActvitySource
		)
 INSERT INTO T_ActivityType (
		Acronym
		,ActivityType
		,Description
		,BalanceType
		,ActivitySource
		)
	VALUES (   
	     'FxForwardShortST_Settled'
		,'FxForwardShortST_Settled'
		,NULL
		,@BalanceType
		,@ActvitySource
		)
		END
END

--------------------------------------------------------------------------------------------------------------------------------

-------------------------------------------------AddRealizedPNLLongShortActivityJournalMapping-------------------------------------
IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_ActivityJournalMapping'
			AND COLUMN_NAME = 'Id'
		)

BEGIN
	DECLARE @Id INT
	DEClARE @AmountTypeId_FK INT
	DECLARE @CashValueType INT
	DECLARE @ActivityDateType INT
	DECLARE @ActivityTypeId_FK INT
	DECLARE @CreditAccount INT
	DECLARE @DebitAccount INT
	
	 IF EXISTS (
		SELECT * from  T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquityLongRealizedPNL')
		)
	 BEGIN
	 SET @Id=(select Min(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquityLongRealizedPNL'))
	 SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
	 SET @CashValueType =(SELECT CashValueType from  T_ActivityJournalMapping where Id=@Id)
	 SET @ActivityDateType =(SELECT ActivityDateType from  T_ActivityJournalMapping where Id=@Id)
	 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='EquityLongRealizedPNL (LT)')
	 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='EquityRealizedPNL(LT)')
	 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)

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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		 SET @Id=(select Max(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquityLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CreditAccount=(SELECT CreditAccount from  T_ActivityJournalMapping where Id=@Id)
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)

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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)

         SET @Id=(select Min(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquityLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='EquityRealizedPNL(ST)')
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='EquityLongRealizedPNL (ST)')

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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)

		 SET @Id=(select Max(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquityLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
		 SET @CreditAccount=(SELECT CreditAccount from  T_ActivityJournalMapping where Id=@Id)

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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		 SET @Id=(select Min(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquityLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='EquityShortRealizedPNL')
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='EquityShortRealizedPNL (ST)')
		 SET @Id=(select Min(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquityShortRealizedPNL'))
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)

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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType

		)

		 SET @Id=(select Max(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquityLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
		 SET @CreditAccount=(SELECT CreditAccount from  T_ActivityJournalMapping where Id=@Id)

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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType

		)
		END

			IF EXISTS (
				SELECT * from  T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquityOptionLongRealizedPNL')
				)
		 BEGIN
		 SET @Id=(select Min(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquityOptionLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CashValueType =(SELECT CashValueType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityDateType =(SELECT ActivityDateType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='EquityOptionLongRealizedPNL (LT)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='EquityOptionRealizedPNL(LT)')
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)

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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		 SET @Id=(select Max(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquityOptionLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
		 SET @CreditAccount=(SELECT CreditAccount from  T_ActivityJournalMapping where Id=@Id)


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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		SET @Id=(select Min(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquityOptionLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='EquityOptionRealizedPNL(ST)')
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='EquityOptionLongRealizedPNL (ST)')

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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		SET @Id=(select Max(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquityOptionLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
		 SET @CreditAccount=(SELECT CreditAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		SET @Id=(select Min(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquityOptionLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='EquityOptionShortRealizedPNL')
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='EquityOptionShortRealizedPNL (ST)')
		 SET @Id=(select Min(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquityOptionShortRealizedPNL'))
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)

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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		SET @Id=(select Max(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquityOptionLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
		 SET @CreditAccount=(SELECT CreditAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		END

		IF EXISTS (
				SELECT * from  T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquitySwapLongRealizedPNL')
				)
		 BEGIN
		 SET @Id=(select Id from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='EquitySwapLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CashValueType =(SELECT CashValueType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityDateType =(SELECT ActivityDateType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='EquitySwapLongRealizedPNL (LT)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='EquitySwapRealizedPNL(LT)')
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='EquitySwapRealizedPNL(ST)')
		SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='EquitySwapLongRealizedPNL (ST)')
		
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='EquitySwapShortRealizedPNL')
		SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='EquitySwapShortRealizedPNL (ST)')
	
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		END

		IF EXISTS (
				SELECT * from  T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='BondLongRealizedPNL')
				)
		 BEGIN
		 SET @Id=(select Id from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='BondLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CashValueType =(SELECT CashValueType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityDateType =(SELECT ActivityDateType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='BondLongRealizedPNL (LT)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='BondRealizedPNL(LT)')
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='BondRealizedPNL(ST)')
		SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='BondLongRealizedPNL (ST)')
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='BondShortRealizedPNL')
		SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='BondShortRealizedPNL (ST)')
		SET @Id=(select Id from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='BondShortRealizedPNL'))
		SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)

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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		END

		IF EXISTS (
				SELECT * from  T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FutureOptionLongRealizedPNL')
				)
		 BEGIN
		 SET @Id=(select Min(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FutureOptionLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CashValueType =(SELECT CashValueType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityDateType =(SELECT ActivityDateType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='FutureOptionLongRealizedPNL (LT)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='FutureOptionRealizedPNL(LT)')
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		 SET @Id=(select Max(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FutureOptionLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CreditAccount=(SELECT CreditAccount from  T_ActivityJournalMapping where Id=@Id)
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		SET @Id=(select Min(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FutureOptionLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='FutureOptionLongRealizedPNL (ST)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='FutureOptionRealizedPNL(ST)')
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		SET @Id=(select Max(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FutureOptionLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CreditAccount=(SELECT CreditAccount from  T_ActivityJournalMapping where Id=@Id)
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		SET @Id=(select Min(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FutureOptionLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='FutureOptionShortRealizedPNL (ST)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='FutureOptionShortRealizedPNL')
		 SET @Id=(select Min(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FutureOptionShortRealizedPNL'))
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		SET @Id=(select Max(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FutureOptionLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CreditAccount=(SELECT CreditAccount from  T_ActivityJournalMapping where Id=@Id)
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		END
	
		IF EXISTS (
				SELECT * from  T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FutureRealizedPNL')
				)
		 BEGIN
		 SET @Id=(select Min(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FutureRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CashValueType =(SELECT CashValueType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityDateType =(SELECT ActivityDateType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='FutureLongRealizedPNL (ST)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='FutureLongRealizedPNL(ST)')
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)

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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)

		 SET @Id=(select Max(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FutureRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CreditAccount=(SELECT CreditAccount from  T_ActivityJournalMapping where Id=@Id)
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)

		 SET @Id=(select Min(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FutureRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='FutureLongRealizedPNL (LT)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='FutureLongRealizedPNL(LT)')
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		 SET @Id=(select Max(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FutureRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CreditAccount=(SELECT CreditAccount from  T_ActivityJournalMapping where Id=@Id)
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		 SET @Id=(select Min(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FutureRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id) 
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='FutureShortRealizedPNL (LT)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='FutureShortRealizedPNL(LT)')
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		 SET @Id=(select Max(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FutureRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CreditAccount=(SELECT CreditAccount from  T_ActivityJournalMapping where Id=@Id)
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		SET @Id=(select Min(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FutureRealizedPNL'))
	    SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id) 
		SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='FutureShortRealizedPNL (ST)')
		SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='FutureShortRealizedPNL(ST)')
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		 SET @Id=(select Max(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FutureRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CreditAccount=(SELECT CreditAccount from  T_ActivityJournalMapping where Id=@Id)
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		END

		IF EXISTS (
				SELECT * from  T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FXLongRealizedPNL')
				)
		 BEGIN
		 SET @Id=(select Id from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FXLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CashValueType =(SELECT CashValueType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityDateType =(SELECT ActivityDateType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='FXLongRealizedPNL (LT)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='FXRealizedPNL(LT)')
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='FXLongRealizedPNL (ST)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='FXRealizedPNL(ST)')
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType

		)
		SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='FXShortRealizedPNL (ST)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='FXShortRealizedPNL')
		 SET @Id=(select Id from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FXShortRealizedPNL'))
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
		 
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		END

		IF EXISTS (
				SELECT * from  T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FXForwardLongRealizedPNLActivityId')
				)
		 BEGIN
		 SET @Id=(select Id from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FXForwardLongRealizedPNLActivityId'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CashValueType =(SELECT CashValueType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityDateType =(SELECT ActivityDateType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='FXForwardLongRealizedPNLActivityId (LT)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='FXForwardRealizedPNL(LT)')
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)

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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='FXForwardLongRealizedPNLActivityId (ST)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='FXForwardRealizedPNL(ST)')
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='FXForwardShortRealizedPNLActivityId (ST)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='FXForwardShortRealizedPNL')
		 SET @Id=(select Id from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FXForwardShortRealizedPNLActivityId'))
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)

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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		END

		IF EXISTS (
				SELECT * from  T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='PrivateEquityLongRealizedPNL')
				)
		 BEGIN
		 SET @Id=(select Id from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='PrivateEquityLongRealizedPNL'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CashValueType =(SELECT CashValueType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityDateType =(SELECT ActivityDateType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='PrivateEquityLongRealizedPNL (LT)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='PrivateEquityRealizedPNL(LT)')
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
	     SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='PrivateEquityLongRealizedPNL (ST)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='PrivateEquityRealizedPNL(ST)')
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='PrivateEquityShortRealizedPNL (ST)')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='PrivateEquityShortRealizedPNL')
		 SET @Id=(select Id from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='PrivateEquityShortRealizedPNL'))
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)

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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		END

		IF EXISTS (
				SELECT * from  T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FxForward_Settled')
				)
		 BEGIN
		 SET @Id=(select Min(Id) from T_ActivityJournalMapping where ActivityTypeId_FK=(select ActivityTypeId from T_ActivityType where Acronym='FxForward_Settled'))
	     SET @AmountTypeId_FK =(SELECT AmountTypeId_FK from  T_ActivityJournalMapping where Id=@Id)
		 SET @CashValueType =(SELECT CashValueType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityDateType =(SELECT ActivityDateType from  T_ActivityJournalMapping where Id=@Id)
		 SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='FxForwardLongLT_Settled')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='FXForwardRealizedPNL(LT)')
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='FxForwardLongST_Settled')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='FXForwardRealizedPNL(ST)')
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
	SET @ActivityTypeId_FK=( Select ActivityTypeId from T_ActivityType where Acronym='FxForwardShortST_Settled')
		 SET @CreditAccount=(Select SubAccountID from T_SubAccounts where Acronym='FXForwardShortRealizedPNL')
		 SET @DebitAccount=(SELECT DebitAccount from  T_ActivityJournalMapping where Id=@Id)
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
		,@AmountTypeId_FK
		,@DebitAccount
		,@CreditAccount
		,@CashValueType
		,@ActivityDateType
		)
		END
END


--------------------------------------------------------------------------------------------------------------------------------