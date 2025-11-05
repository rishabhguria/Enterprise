

/****** Object:  Stored Procedure dbo.P_GetCalculationbyID  Script Date: 2/18/2006 1:20:21 PM ******/
CREATE PROCEDURE dbo.P_GetCalculationbyID
(
	@calculationID  int
)
AS
	Select CommissionCalculationID, CalculationType
	From T_CommissionCalculation
	Where  CommissionCalculationID = @calculationID 
	


