
/*
Name : <P_GetFFThirdPartySaveDetails>
Created by : <Kanupriya>
Date : <10/13/2006>
Purpose : <to fetch the save details of a flat file of a particular third party.>

*/


CREATE PROCEDURE [dbo].[P_GetFFThirdPartySaveDetails]
	
	(
	@thirdPartyID int 
	)
	
AS
	SELECT     CompanyThirdPartySaveDetailID, CompanyThirdPartyID, SaveGeneratedFileIn, NamingConvention, CompanyIdentifier
	FROM         T_CompanyThirdPartyFlatFileSaveDetails
	WHERE     (CompanyThirdPartyID = @thirdPartyID)
