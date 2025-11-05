


/****** Object:  Stored Procedure dbo.P_GetAllFutureMonthCodes    Script Date: 09/12/2005 12:37:21 PM ******/
CREATE PROCEDURE dbo.P_GetAllFutureMonthCodes
AS
	Select FutureMonthCodeID, FutureMonth, Abbreviation
	From T_FutureMonthCode Order By FutureMonth
	


