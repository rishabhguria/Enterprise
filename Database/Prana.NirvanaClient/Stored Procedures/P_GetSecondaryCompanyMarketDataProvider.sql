CREATE PROCEDURE [dbo].[P_GetSecondaryCompanyMarketDataProvider]
	@CompanyId INT,
	@SecondaryMarketDataProvider INT OUTPUT
AS
BEGIN
	SET @SecondaryMarketDataProvider = 
	(
		SELECT [SecondaryMarketDataProvider] FROM [dbo].[T_CompanyMarketDataProvider]
		WHERE [CompanyID] = @CompanyId
	);
END
