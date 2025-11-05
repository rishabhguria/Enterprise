

/*Created by :<Kanupriya>
 date :<10/09/2006>
 Purpose:<To fetch the company funds permitted to a selected thirdPartyID .>
 Name : <dbo.P_GetThirdPartyPermittedFunds> */


CREATE PROCEDURE [dbo].[P_GetThirdPartyPermittedFunds]
	
	(
	@thirdPartyID int 
	)
	
AS
	SELECT     T_ThirdPartyPermittedFunds.ThirdPartyFundID_PK, T_ThirdPartyPermittedFunds.ThirdPartyID, T_ThirdPartyPermittedFunds.CopanyFundID, 
	                      T_CompanyFunds.FundShortName, T_CompanyFunds.FundTypeID
	FROM         T_ThirdPartyPermittedFunds INNER JOIN
	                      T_CompanyFunds ON T_ThirdPartyPermittedFunds.CopanyFundID = T_CompanyFunds.CompanyFundID
	WHERE     (T_ThirdPartyPermittedFunds.ThirdPartyID = @thirdPartyID AND T_CompanyFunds.IsActive = 1)

