


/****** Object:  Stored Procedure dbo.P_GetAllCurrencies    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllCurrencies
AS
	Select CurrencyID, CurrencyName, CurrencySymbol
	From T_Currency Order by  CurrencySymbol
	--Where DeletedID = 1


