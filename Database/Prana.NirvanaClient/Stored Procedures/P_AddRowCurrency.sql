


/****** Object:  Stored Procedure dbo.P_AddRowCurrency    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_AddRowCurrency
	(
		@currency varchar(50)
	)
AS 
INSERT T_Currency (CurrencyName)
  Values(@currency)	


