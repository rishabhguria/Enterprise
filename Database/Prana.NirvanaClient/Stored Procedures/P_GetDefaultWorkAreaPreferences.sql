CREATE PROC [dbo].[P_GetDefaultWorkAreaPreferences]
AS
BEGIN
	DECLARE @CounterPartyID INT
	SET @CounterPartyID = (
			SELECT TOP (1) CounterPartyID
			FROM T_CounterParty
			)
	DECLARE @VenueID INT
	SET @VenueID = (
			SELECT TOP (1) VenueID
			FROM T_Venue
			)
	DECLARE @TIFID INT
	SET @TIFID = (
			SELECT TOP (1) TimeInForceID
			FROM T_TimeInForce
			)
	DECLARE @TradingAccountID INT
	SET @TradingAccountID = (
			SELECT TOP (1) CompanyTradingAccountsID
			FROM T_CompanyTradingAccounts
			)

	DECLARE @CommaSeparatedFunds VARCHAR(MAX)
	SET @CommaSeparatedFunds = ' '
	DECLARE @CalculationStrategy INT
	SET @CalculationStrategy = 0
        DECLARE @IsIncludeExcludeAllowed BIT
	SET @IsIncludeExcludeAllowed = 1
	DELETE
	FROM T_WorkAreaPreferences
	INSERT INTO T_WorkAreaPreferences (
		IsUseWorkAreaPreferences
		, CounterPartyID
		, VenueID
		, TIFID
		, TradingAccountID
		, CommaSeparatedFunds
		, CalculationStrategy
		, IsIncludeExcludeAllowed
		)
	VALUES (
		1
		, @CounterPartyID
		, @VenueID
		, @TIFID
		, @TradingAccountID
		, @CalculationStrategy
		, @CalculationStrategy
		, @IsIncludeExcludeAllowed
		)
  SELECT * FROM T_WorkAreaPreferences
END