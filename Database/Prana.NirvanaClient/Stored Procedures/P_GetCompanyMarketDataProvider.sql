/* 
	Usage:
	DECLARE @MarketDataProvider INT;
	EXEC P_GetCompanyMarketDataProvider 5, @MarketDataProvider OUTPUT;
	SELECT @MarketDataProvider;
*/

CREATE PROCEDURE P_GetCompanyMarketDataProvider
	@CompanyId INT,
	@MarketDataProvider INT OUTPUT
AS
BEGIN
	SET @MarketDataProvider = 
	(
		SELECT [MarketDataProvider] FROM [dbo].[T_CompanyMarketDataProvider]
		WHERE [CompanyID] = @CompanyId
	);
END