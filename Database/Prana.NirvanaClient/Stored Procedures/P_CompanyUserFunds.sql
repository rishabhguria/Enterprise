
/*        
Modified By: Ankit Gupta on 10 Oct, 2014
Description: Get only those user funds which are currently in active state.
*/
CREATE PROCEDURE [dbo].[P_CompanyUserFunds] (@UserID INT)
AS
DECLARE @RoleID INT

SELECT @RoleID = RoleID
FROM T_CompanyUser
WHERE UserID = @UserID

--For admin and system admin show data for all the funds  
IF (
		(@RoleID = 3)
		OR (@RoleID = 4)
		)
BEGIN
	SELECT DISTINCT T_CompanyFunds.CompanyFundID AS FundID
		,T_CompanyFunds.FundName AS FundName
		,T_Company.CompanyID AS CompanyID
		,T_CompanyFunds.CompanyThirdPartyID AS ThirdPartyID
		,T_CompanyFunds.FundShortName AS FundShortName
	FROM T_CompanyFunds
	INNER JOIN T_Company ON T_Company.CompanyID = T_CompanyFunds.CompanyID
	WHERE T_CompanyFunds.IsActive = 1
END
ELSE
BEGIN
	SELECT DISTINCT T_CompanyFunds.CompanyFundID AS FundID
		,T_CompanyFunds.FundName AS FundName
		,T_Company.CompanyID AS CompanyID
		,T_CompanyFunds.CompanyThirdPartyID AS ThirdPartyID
		,T_CompanyFunds.FundShortName AS FundShortName
	FROM T_CompanyUserFundGroupMapping
	INNER JOIN T_CompanyUser ON T_CompanyUserFundGroupMapping.CompanyUserID = T_CompanyUser.UserID
	INNER JOIN T_GroupFundMapping ON T_CompanyUserFundGroupMapping.FundGroupID = T_GroupFundMapping.FundGroupID
	INNER JOIN T_CompanyFunds ON T_GroupFundMapping.FundID = T_CompanyFunds.CompanyFundID
	INNER JOIN T_Company ON T_Company.CompanyID = T_CompanyFunds.CompanyID
	WHERE T_CompanyUser.UserID = @UserID
		AND T_CompanyFunds.IsActive = 1
END
