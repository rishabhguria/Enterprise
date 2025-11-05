CREATE PROCEDURE [dbo].[P_SaveSecuritiesList]
	@CompanyId int,
	@Symbols VARCHAR(MAX),
	@RestrictedOrAllowed int,
	@IsTickerSymbology bit
AS
	SELECT Items AS Symbol  INTO #Symbols
		FROM dbo.Split(@Symbols, ',') 
	
	DELETE from T_RestrictedAllowedSecuritiesList where RestrictedOrAllowed = @RestrictedOrAllowed

	INSERT into T_RestrictedAllowedSecuritiesList(CompanyID, Symbol, RestrictedOrAllowed, IsTickerSymbology)
	VALUES (@CompanyId, @Symbols, @RestrictedOrAllowed, @IsTickerSymbology)

