-- =============================================
-- Author:		<Harsh Kumar>
-- Create date: <21 july 2008>
-- Description:	<it returns pairs of Company Funds and master Fund that the comp. fund belongs to>
-- =============================================
CREATE PROCEDURE GetCompanyFundMasterFundRelationShip
AS
BEGIN
    select T_CompanyFunds.CompanyFundID,T_CompanyFunds.FundName,T_CompanyMasterFunds.CompanyMasterFundID,T_CompanyMasterFunds.MasterFundName from T_CompanyFunds join T_CompanyMasterFundSubAccountAssociation 
on T_CompanyFunds.CompanyFundID = T_CompanyMasterFundSubAccountAssociation.CompanyFundID
join T_CompanyMasterFunds on T_CompanyMasterFundSubAccountAssociation.CompanyMasterFundID =  T_CompanyMasterFunds.CompanyMasterFundID

	
END
