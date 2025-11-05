

CREATE PROCEDURE [dbo].[P_GetCompanyOtherCurrencies]  
 (  
  @companyID int   
 )  
AS  
   
 Select CurrencyID   
 From T_CompanyAllCurrencies
 Where CompanyID = @companyID

