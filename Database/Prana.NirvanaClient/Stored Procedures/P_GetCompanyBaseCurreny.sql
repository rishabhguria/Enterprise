CREATE PROCEDURE [dbo].[P_GetCompanyBaseCurreny]      
 (      
  @companyID int       
 )      
AS      
       
 Select BaseCurrencyID From T_Company where CompanyID = @companyID    
  