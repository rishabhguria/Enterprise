/****** Object:  Stored Procedure dbo.P_GetAllVenueTypes    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllVenueTypes
AS
	SELECT     VenueTypeID, VenuType
FROM         T_VenuType Order by VenuType ASC
