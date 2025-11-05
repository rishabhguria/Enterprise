
/*
Name :<P_DeleteThirdPartyPermittedFunds>
Created by :<Kanupriya>
Date :<10/12/2006>
Purpose :<To Delete the thirdParty Funds.>

*/


CREATE PROCEDURE [dbo].[P_DeleteThirdPartyPermittedFunds]
	(
		@thirdPartyID int 
	)
	
AS
	DELETE FROM T_ThirdPartyPermittedFunds
	WHERE     (ThirdPartyID = @thirdPartyID)

