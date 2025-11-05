/*  --Author : bhavana 
    --Dated : 22 April 2014  
*/

/****** Object:  Stored Procedure dbo.P_GetAllClientsByMappedFunds    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE [dbo].[P_GetAllClientsByMappedFunds]
(
@groupID int
)
AS

SELECT   distinct T_CompanyFunds.CompanyID AS CompanyID, T_Company.Name AS CompanyName
FROM      T_CompanyFunds INNER JOIN T_GroupFundMapping 
ON T_CompanyFunds.CompanyFundID = T_GroupFundMapping.FundID INNER JOIN T_Company
ON T_CompanyFunds.CompanyID = T_Company.CompanyID
WHERE T_GroupFundMapping.FundGroupID = @groupID

