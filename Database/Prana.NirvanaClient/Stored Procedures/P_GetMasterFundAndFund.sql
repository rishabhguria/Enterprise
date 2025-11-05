
----------------------------------Procedure Start----------------------------------------------------------------
CREATE PROCEDURE P_GetMasterFundAndFund
AS
SELECT CompanyMasterFundID
	,CompanyFundID
FROM T_CompanyMasterFundSubAccountAssociation
