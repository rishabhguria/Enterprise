
--Bharat Kumar Jangir (24 December 2013)      
--This stored procedure returns (all in different datatables)      
--Positions with/without Transactions      
--Transactions      
--FundWise Cash      
--FundWise Start of Day Accruals  
CREATE PROCEDURE [dbo].[PMGetFundOpenPositionsAndTransactions] (
	@ForPositionsAllAUECDatesString VARCHAR(max)
	,@ForTransactionsAllAUECDatesString VARCHAR(max)
	,@AssetIds VARCHAR(max)
	,@FundIds VARCHAR(max)
	,@Symbols VARCHAR(max)
	,@CustomConditions VARCHAR(max)
	,@IsPositionsIncludeTransactions BIT
	,@IsAccrualsNeeded BIT
	,@DateForCashValues DATETIME
	,@IsBetaNeeded BIT
	,@DateTimeStringForBetaPositions VARCHAR(max)
	)
AS
BEGIN
	DECLARE @Local_ForPositionsAllAUECDatesString VARCHAR(max)
	DECLARE @Local_ForTransactionsAllAUECDatesString VARCHAR(max)
	DECLARE @Local_AssetIds VARCHAR(max)
	DECLARE @Local_FundIds VARCHAR(max)
	DECLARE @Local_Symbols VARCHAR(max)
	DECLARE @Local_CustomConditions VARCHAR(max)
	DECLARE @Local_IsPositionsIncludeTransactions BIT
	DECLARE @Local_IsAccrualsNeeded BIT
	DECLARE @Local_DateForCashValues DATETIME
	DECLARE @Local_AllAUECDatesString VARCHAR(max)
	DECLARE @Local_IsBetaNeeded BIT
	DECLARE @Local_DateTimeStringForBetaPositions VARCHAR(max)

	SET @Local_ForPositionsAllAUECDatesString = @ForPositionsAllAUECDatesString
	SET @Local_ForTransactionsAllAUECDatesString = @ForTransactionsAllAUECDatesString
	SET @Local_FundIds = @FundIds
	SET @Local_AssetIds = @AssetIds
	SET @Local_Symbols = @Symbols
	SET @Local_CustomConditions = @CustomConditions
	SET @Local_IsPositionsIncludeTransactions = @IsPositionsIncludeTransactions
	SET @Local_IsAccrualsNeeded = @IsAccrualsNeeded
	SET @Local_DateForCashValues = @DateForCashValues
	SET @Local_IsBetaNeeded = @IsBetaNeeded
	SET @Local_DateTimeStringForBetaPositions = @DateTimeStringForBetaPositions

	IF @Local_IsPositionsIncludeTransactions = 1
		SET @Local_AllAUECDatesString = @Local_ForTransactionsAllAUECDatesString
	ELSE
		SET @Local_AllAUECDatesString = @Local_ForPositionsAllAUECDatesString

	EXEC PMGetFundOpenPositionsForDateBase_New @Local_AllAUECDatesString
		,@Local_AssetIds
		,@Local_FundIds
		,@Local_Symbols
		,@Local_CustomConditions

	EXEC P_GetTransactions @Local_ForTransactionsAllAUECDatesString
		,@Local_ForTransactionsAllAUECDatesString
		,@Local_AssetIds
		,@Local_FundIds
		,0

	EXEC P_GetOpenUnallocatedTrades @Local_ForTransactionsAllAUECDatesString

	EXEC PMGetDayEndDataInBaseCurrency @Local_DateForCashValues
		,0  

	IF @Local_IsBetaNeeded = 1
	BEGIN
		EXEC GetBetaForDate @Local_DateTimeStringForBetaPositions
	END
          
 IF @Local_IsAccrualsNeeded = 1    
 BEGIN    
  EXEC GetStartDayOfAccruals @Local_IsAccrualsNeeded    
 END  
END