   create PROCEDURE [dbo].[P_GetCompanyFundsC] AS  
 SELECT     CompanyFundID,FundShortName, FundName, CompanyID  
 FROM         T_CompanyFunds where isactive = 1

order by T_CompanyFunds.UIOrder, T_CompanyFunds.FundShortName

