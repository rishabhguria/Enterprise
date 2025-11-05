CREATE PROCEDURE [dbo].[P_GetFactSetContractTypeQueryDataInformation]
	@CompanyId INT,
	@FactSetContractType INT OUTPUT
AS
BEGIN
	SET @FactSetContractType = 
	(
		SELECT [FactSetContractType] FROM [dbo].[T_CompanyMarketDataProvider]
		WHERE [CompanyID] = @CompanyId
	);
END
