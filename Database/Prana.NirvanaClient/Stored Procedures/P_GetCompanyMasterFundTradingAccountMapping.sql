CREATE PROCEDURE [dbo].[P_GetCompanyMasterFundTradingAccountMapping]
(
@companyID int
)
AS 
BEGIN
   SELECT
      MF.CompanyMasterFundID,
      MFTA.CompanyTradingAccountID
   FROM
      T_CompanyMasterFundTradingAccountAssociation MFTA
      INNER JOIN T_CompanyMasterFunds MF
         ON MFTA.CompanyMasterFundID = MF.CompanyMasterFundID 
END