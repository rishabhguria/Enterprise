CREATE PROCEDURE [dbo].[P_GetAllWashSalePreferences]
AS
SELECT FundID,
	   WashSaleStartDate	
FROM T_WashSalePreferences