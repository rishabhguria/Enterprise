/* Name :<P_GetFundDetail>
Created by : <Kanupriya>
Date : <10/16/2006>
Purpose :<To fetch the details of a fund for a particular fundName.>
*/

CREATE PROCEDURE [dbo].[P_GetFundDetail]
	
	(
	@fundName varchar(50)
	)
	
AS
	SELECT     CompanyFundID, FundName, FundShortName, CompanyID, FundTypeID
	FROM         T_CompanyFunds
	WHERE     (FundShortName = @fundName)
