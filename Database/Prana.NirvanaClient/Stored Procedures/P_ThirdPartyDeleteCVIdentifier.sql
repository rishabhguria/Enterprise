/*
Name : <P_ThirdPartyDeleteCVIdentifier>
Created by : <Kanupriya>
Date : <10/24/2006>
Purpose : <To delete the existing CV Identifier details of a third Party.>
*/
CREATE PROCEDURE [dbo].[P_ThirdPartyDeleteCVIdentifier]
	
	(
	 @companyThirdPartyID int
	)
	
AS
	DELETE FROM T_CompanyThirdPartyCVIdentifier
	WHERE     (CompanyThirdPartyID_FK = @companyThirdPartyID)
