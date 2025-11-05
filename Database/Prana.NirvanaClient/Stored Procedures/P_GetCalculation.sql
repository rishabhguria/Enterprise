

/****** Object:  Stored Procedure dbo.P_GetCalculation  Script Date: 2/18/2006 10:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetCalculation

AS
	Select CommissionCalculationID, CalculationType
	From T_CommissionCalculation
	Order By CalculationType Asc


