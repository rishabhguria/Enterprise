-- Date : 4th Dec, 08	
-- Purpose : Selects all the prime broker & clearers.
-- Usage : P_GetAllPrimeBrokerAndClearers
CREATE PROCEDURE [dbo].[P_GetAllPrimeBrokerAndClearers]

AS
	SELECT     ThirdPartyID, ThirdPartyName, Description, ThirdPartyTYpeID
	FROM         T_ThirdParty
	Where ThirdPartyTypeID = 1




