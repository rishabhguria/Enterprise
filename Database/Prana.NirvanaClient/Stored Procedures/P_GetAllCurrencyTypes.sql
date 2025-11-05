


/****** Object:  Stored Procedure dbo.P_GetAllCurrencyTypes    Script Date: 12/07/2005 6:15:21 PM ******/
CREATE PROCEDURE dbo.P_GetAllCurrencyTypes
AS
	Select CurrencyTypeID, CurrencyType
	From T_CurrencyType Order By CurrencyType
	


