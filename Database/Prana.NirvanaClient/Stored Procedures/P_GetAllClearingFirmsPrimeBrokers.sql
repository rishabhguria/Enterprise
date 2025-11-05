


/****** Object:  Stored Procedure dbo.P_GetAllClearingFirmsPrimeBrokers    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllClearingFirmsPrimeBrokers
AS
	SELECT   ClearingFirmsPrimeBrokersID, ClearingFirmsPrimeBrokersName, ClearingFirmsPrimeBrokersShortName
FROM         T_ClearingFirmsPrimeBrokers



