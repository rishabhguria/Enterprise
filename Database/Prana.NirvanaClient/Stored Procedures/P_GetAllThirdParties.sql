


/****** Object:  Stored Procedure dbo.P_GetAllThirdParties    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllThirdParties
AS
	/*SELECT     ThirdPartyID, ThirdPartyName, Description
	FROM         T_ThirdParty WHERE ThirdPartyTypeID = 1

--ThirdPartyTypeID condition is checked to include only third parties but not vendors as thirdpartytype have id = 1.
	*/
	
	SELECT     ThirdPartyID, ThirdPartyName, Description, ThirdPartyTypeID
	FROM         T_ThirdParty



