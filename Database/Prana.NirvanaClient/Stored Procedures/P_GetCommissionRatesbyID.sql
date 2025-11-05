

/****** Object:  Stored Procedure dbo.P_GetCommissionRatesbyID  Script Date: 2/18/2006 2:30:21 PM ******/
CREATE PROCEDURE dbo.P_GetCommissionRatesbyID
(
@commissionRateID int
)
AS
	Select CommissionRateID, CommissionRateType
	From T_CommissionRateType
	where CommissionRateID=@commissionRateID


