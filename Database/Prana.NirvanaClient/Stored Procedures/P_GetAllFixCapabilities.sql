


/****** Object:  Stored Procedure dbo.P_GetAllFixCapabilities    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllFixCapabilities
AS
	SELECT   FixCapabilityID, Description
FROM         T_FIXCapability Order By Description


