
/****** Object:  Stored Procedure dbo.P_GetCompanyThirdParty    Script Date: 04/03/2006 8:35:23 PM ******/      
CREATE PROCEDURE [dbo].[P_GetCompanyThirdParty]
 (      
  @companyThirdPartyID int        
 )      
AS      
 SELECT     TP.ThirdPartyID, ThirdPartyName, Description, ThirdPartyTypeID, ShortName, ContactPerson, Address1,      
    Address2, CellPhone, WorkTelephone, Fax, Email, CountryID, StateID, Zip, ContactPersonLastName,       
    ContactPersonTitle, ContactPersonWorkPhone, ContactPersonFax, SecurityIdentifierTypeID,BrokerCode,CounterPartyID-- , Delimiter,DelimiterName,FileExtension      
 FROM         T_CompanyThirdParty CTP inner join T_ThirdParty TP on CTP.ThirdPartyID = TP.ThirdPartyID      
 Where CTP.CompanyThirdPartyID = @companyThirdPartyID    
  
