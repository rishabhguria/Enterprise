


/****** Object:  Stored Procedure dbo.P_GetAllCompanyClearingFirmsPrimeBrokers    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_GetAllCompanyClearingFirmsPrimeBrokers
AS
	SELECT   CompanyClearingFirmsPrimeBrokersID, ClearingFirmsPrimeBrokersName, ClearingFirmsPrimeBrokersShortName, CompanyID
FROM         T_CompanyClearingFirmsPrimeBrokers



