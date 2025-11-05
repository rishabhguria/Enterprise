
/****** Object:  Stored Procedure dbo.P_GetThirdParty    Script Date: 11/17/2005 9:50:22 AM ******/      
CREATE PROCEDURE [dbo].[P_GetThirdParty] (      
  @thirdPartyID int        
 )      
AS      
 SELECT     ThirdPartyID, ThirdPartyName, Description, ThirdPartyTypeID, ShortName, ContactPerson, Address1,      
    Address2, CellPhone, WorkTelephone, Fax, Email, CountryID, StateID, Zip, ContactPersonLastName,       
    ContactPersonTitle, ContactPersonFax, ContactPersonWorkPhone,SecurityIdentifierTypeID,BrokerCode, CounterPartyID --,Delimiter,DelimiterName,FileExtension     
 FROM         T_ThirdParty      
 Where ThirdPartyID = @thirdPartyID 

