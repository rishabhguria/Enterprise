
/****** Object:  Stored Procedure dbo.P_GetCompanyThirdPartyMappingDetails    Script Date: 03/22/2006 11:20:22 PM ******/
			
/*  Name :   P_GetCompanyThirdPartyMappingDetails
	Date Created: <03/22/2006 11:20:22 PM > 
	Purpose: <To fetch a collection of all the ThirdParty Mapping details of a company.  >
	Author: <Bhupesh Bareja>

	Date Modified: <10/06/2006 >
	Description:  <Now the Sp has been modified to fetch ThirdParty Mapping details collection of a particular ThirdParty Of a company.
				And deleted the redundant commented SPs.>
	Modified By:  <Kanupriya>
*/

CREATE PROCEDURE [dbo].[P_GetCompanyThirdPartyMappingDetails]
	(
		@thirdPartyID int
	)
AS

SELECT     CompanyID_FK, CompanyThirdPartyID_FK, InternalFundNameID_FK, MappedName, FundAccntNo, FundTypeID_FK
FROM         T_CompanyThirdPartyMappingDetails
WHERE      (CompanyThirdPartyID_FK = @thirdPartyID)


