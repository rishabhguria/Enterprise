

/*
Name :<P_GetCompanyFundsNotPermittedToThirdParty>
created by :<Kanupriya>
Date :<10/12/2006>
Purpose : < to fetch all the companyfunds except the funds permitted to the selected thirdparty.>
*/
CREATE PROCEDURE [dbo].[P_GetCompanyFundsNotPermittedToThirdParty]
	
	(
	@companyID int ,
	@thirdPartyID int
	)
	
AS
	SELECT     CompanyFundID, FundName, FundShortName, CompanyID
	FROM         T_CompanyFunds
	WHERE     (CompanyID = @companyID) AND (CompanyFundID NOT IN
	                          (SELECT     CopanyFundID
	                            FROM          T_ThirdPartyPermittedFunds
	                            WHERE      (ThirdPartyID = @thirdPartyID)))

