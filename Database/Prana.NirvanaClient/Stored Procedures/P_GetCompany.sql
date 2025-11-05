-----------------------------------------------------------------  
--Modified BY: Manvendra  
--Date: 4-JUNE-2015  
--Purpose: Remove Secondary and Technology Contact fields  
-----------------------------------------------------------------  
CREATE PROCEDURE [dbo].[P_GetCompany]  
 (  
  @companyID int  
 )  
AS  
 SELECT  CompanyID, Name, Address1, Address2, CompanyTypeID, Telephone, Fax,   
   PrimaryContactFirstName, PrimaryContactLastName, PrimaryContactTitle,   
   PrimaryContactEMail, PrimaryContactTelephone, PrimaryContactCell,  
   ShortName, Login, Password, CountryID, StateID, Zip,  
   BaseCurrencyID, SupportsMultipleCurrencies, City, region, TimeZone, AlertEmails, SendAllocationsViaFix
 FROM  T_Company  
 Where CompanyID = @companyID  