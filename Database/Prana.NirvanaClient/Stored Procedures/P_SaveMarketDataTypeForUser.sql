
Create PROCEDURE [dbo].[P_SaveMarketDataTypeForUser]
	(
		@userID int,
		@marketDataTypeID int
	)
AS

	insert T_CompanyUserMarketDataTypes(CompanyUserID, MarketDataTypeID) Values (@userID, @marketDataTypeID)
