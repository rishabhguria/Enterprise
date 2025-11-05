/*
Name : <P_TPGetThirdPartyMappingDetailsperFund>
Created by : <Kanupriya>
Dated :<10/28/2006>
Purpose : <To fetch the mapping details for a particular third party fund.>
*/
CREATE PROCEDURE dbo.P_TPGetThirdPartyMappingDetailsperFund
	
	(
	@thirdPartyID int,
	@companyFundID int
	)
	
AS
	SELECT     CompanyID_FK, CompanyThirdPartyID_FK, InternalFundNameID_FK, MappedName, FundAccntNo,  FundTypeID_FK
	FROM         T_CompanyThirdPartyMappingDetails
	WHERE     (CompanyThirdPartyID_FK = @thirdPartyID) AND (InternalFundNameID_FK = @companyFundID)
