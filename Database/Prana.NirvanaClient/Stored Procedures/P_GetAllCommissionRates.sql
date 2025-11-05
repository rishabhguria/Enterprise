

/****** Object:  Stored Procedure dbo.P_GetCommissionAllRates  Script Date: 2/18/2006 11:45:21 PM ******/
CREATE PROCEDURE dbo.P_GetAllCommissionRates
AS
	Select CommissionRateID, CommissionRateType
	From T_CommissionRateType
	order by CommissionRateType ASC


