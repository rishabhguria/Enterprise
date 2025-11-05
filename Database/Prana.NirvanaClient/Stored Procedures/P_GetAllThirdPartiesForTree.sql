
/*        
Modified By: Ankit Gupta on 10 Oct, 2014
Description: TO display on third party UI, get only those third parties from DB, that are currently in Active state.
*/        
CREATE PROCEDURE [dbo].[P_GetAllThirdPartiesForTree] AS      
 SELECT     ThirdPartyID, ThirdPartyName, Description, ThirdPartyTypeID, ShortName, ContactPerson, Address1,      
    Address2, CellPhone, WorkTelephone, Fax, Email, CountryID, StateID, Zip, ContactPersonLastName,       
    ContactPersonTitle, ContactPersonFax, ContactPersonWorkPhone,SecurityIdentifierTypeID,BrokerCode,CounterPartyID --,Delimiter, DelimiterName,FileExtension     
 FROM         T_ThirdParty where isActive =1
 
