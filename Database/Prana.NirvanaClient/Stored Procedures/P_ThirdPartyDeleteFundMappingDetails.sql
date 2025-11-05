/*
Name : <P_ThirdPartyDeleteFundMappingDetails>
Created By : <Kanupriya>
Date : <10/24/2006>
purpose : <To delete the third party fund mapping details for a selected third party.>
*/
CREATE PROCEDURE [dbo].[P_ThirdPartyDeleteFundMappingDetails]
	
	(
	@companyThirdPartyID int
	)
	
AS
	DELETE FROM T_CompanyThirdPartyMappingDetails
	WHERE     (CompanyThirdPartyID_FK = @companyThirdPartyID)
