/* 
	Usage:
	EXEC P_SetCompanyMarketDataProvider 5, 1, 1;
*/

CREATE PROCEDURE P_SetCompanyMarketDataProvider
	@CompanyId INT,
	@MarketDataProvider INT,
	@IsMarketDataBlocked BIT,
	@FactSetContractType INT
AS
BEGIN
	UPDATE [dbo].[T_CompanyMarketDataProvider] SET [MarketDataProvider] = @MarketDataProvider, [IsMarketDataBlocked] = @IsMarketDataBlocked, [FactSetContractType] = @FactSetContractType
	WHERE [CompanyID] = @CompanyId;
END