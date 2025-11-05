


/****** Object:  Stored Procedure dbo.P_GetCVAUECCompliance    Script Date: 12/28/2005 8:25:22 PM ******/
CREATE PROCEDURE dbo.P_GetCVAUECCompliance
	(
		@cvAuecID	int	
	)
AS
	
	Select CVAUECComplianceID, CVAUECID, FollowCompliance, ShortSellConfirmation, IdentifierID, ForeignID From T_CVAUECCompliance
	Where CVAUECID = @cvAuecID


