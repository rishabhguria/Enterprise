CREATE PROCEDURE [dbo].[P_SetSecondaryCompanyMarketDataProvider]
	@CompanyId INT,
	@SecondaryMarketDataProvider INT
AS
BEGIN
	UPDATE [dbo].[T_CompanyMarketDataProvider] SET [SecondaryMarketDataProvider] = @SecondaryMarketDataProvider
	WHERE [CompanyID] = @CompanyId;
END