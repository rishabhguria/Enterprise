


/****** Object:  Stored Procedure dbo.P_GetThirdPartyType    Script Date: 08/02/2006 3:10:22 PM ******/
CREATE PROCEDURE dbo.P_GetThirdPartyType
	(
		@thirdPartyTypeID int		
	)
AS
	SELECT     ThirdPartyTypeID, ThirdPartyTypeName
	FROM         T_ThirdPartyType
	Where ThirdPartyTypeID = @thirdPartyTypeID
		


