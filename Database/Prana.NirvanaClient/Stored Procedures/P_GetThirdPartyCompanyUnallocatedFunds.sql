CREATE PROCEDURE [dbo].[P_GetThirdPartyCompanyUnallocatedFunds]
	
	(
	@companyID int,
	@thirdPartyTypeID int
	)
	
AS
	SELECT     CompanyFundID,FundName,  FundShortName, CompanyID
	FROM         T_CompanyFunds
	WHERE     (CompanyID = @companyID) AND CompanyFundID NOT IN
			(SELECT T_ThirdPartyPermittedFunds.CopanyFundID 
							FROM T_ThirdPartyPermittedFunds WHERE ThirdPartyID IN
			(SELECT CompanyThirdPartyID FROM T_CompanyThirdParty WHERE ThirdPartyID IN
			(SELECT ThirdPartyId FROM T_ThirdParty WHERE ThirdPartyTypeID=@thirdPartyTypeID)))
/*	                          
(SELECT     T_ThirdPartyPermittedFunds.CopanyFundID
	                            FROM          T_ThirdParty INNER JOIN
	                                                   T_ThirdPartyType ON T_ThirdParty.ThirdPartyTypeID = T_ThirdPartyType.ThirdPartyTypeID INNER JOIN
	                                                   T_ThirdPartyPermittedFunds ON T_ThirdParty.ThirdPartyID = T_ThirdPartyPermittedFunds.ThirdPartyID AND 
	                                                   T_ThirdParty.ThirdPartyID = T_ThirdPartyPermittedFunds.ThirdPartyID
	                            WHERE      (T_ThirdParty.ThirdPartyTypeID = @thirdPartyTypeID)))
*/


