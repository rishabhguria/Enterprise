/* -- Fectching data based on company ID
     --Author : omshiv 
    --Dated : 31 March 2014  
*/

/****** Object:  Stored Procedure dbo.P_GetAllCompanyFunds    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE [dbo].[P_GetAllCompanyFunds]
(
@companyID int
)
AS
	SELECT   CompanyFundID, FundName, FundShortName, CompanyID
FROM         T_CompanyFunds where companyID =@companyID and IsActive=1

