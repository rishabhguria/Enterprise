/* 
	Usage:
	DECLARE @IsMarketDataBlocked INT;
	EXEC P_GetMarketDataBlockedInformation 5, @IsMarketDataBlocked OUTPUT;
	SELECT @IsMarketDataBlocked;
*/

CREATE PROCEDURE P_GetMarketDataBlockedInformation
	@CompanyId INT,
	@IsMarketDataBlocked BIT OUTPUT
AS
BEGIN
	SET @IsMarketDataBlocked = 
	(
		SELECT [IsMarketDataBlocked] FROM [dbo].[T_CompanyMarketDataProvider]
		WHERE [CompanyID] = @CompanyId
	);
END
