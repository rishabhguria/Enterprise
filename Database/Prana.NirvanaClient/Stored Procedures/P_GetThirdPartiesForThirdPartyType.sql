


/****** Object:  Stored Procedure dbo.P_GetThirdPartiesForThirdPartyType    Script Date: 02/09/2006 8:15:23 PM ******/
CREATE PROCEDURE dbo.P_GetThirdPartiesForThirdPartyType
	(
		@thirdPartyTypeID int		
	)
AS
	SELECT     ThirdPartyID, ThirdPartyName, Description, ThirdPartyTYpeID
	FROM         T_ThirdParty
	Where ThirdPartyTypeID = @thirdPartyTypeID



