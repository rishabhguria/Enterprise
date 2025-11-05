
CREATE PROCEDURE [dbo].[P_GetCompanyOtherCurrenciesConversions]     
 (      
  @companyID int       
 )      
AS      
Select Distinct T_CompanyAllCurrencies.CurrencyID, T_CurrencyConversion.ConversionFactor    
From T_CompanyAllCurrencies, T_CurrencyConversion, T_Company
Where T_CompanyAllCurrencies.CurrencyID = T_CurrencyConversion.ToCurrencyID
And T_Company.BaseCurrencyID = T_CurrencyConversion.FromCurrencyID 
And T_CompanyAllCurrencies.CompanyID = @companyID 
And T_Company.CompanyID = @companyID 
