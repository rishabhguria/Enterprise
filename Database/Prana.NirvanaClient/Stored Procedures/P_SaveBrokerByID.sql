 
CREATE PROCEDURE [dbo].[P_SaveBrokerByID]    
(     
  @counterPartyID int,
  @counterPartyFullName varchar(50)       
 )    
AS     
BEGIN  
  
Declare @counterPartyVenueID int  
Declare @CompanyID int
Select @CompanyID=companyid from T_Company

set IDENTITY_INSERT T_CounterParty on  
--TODO: Here country id and state id are hard-coded, need to pick id by name  
INSERT T_CounterParty(CounterPartyID, FullName, ShortName, Address, Phone, Fax, ContactName1, Title1, Email1,    
           ContactName2, Title2, Email2, CounterPartyTypeID, Address2, CountryID, StateID, Zip,     
           ContactName1_LastName, ContactName1_WorkPhone, ContactName1_Cell, ContactName2_LastName,     
           ContactName2_WorkPhone, ContactName2_Cell, City)    
Values(@counterPartyID, @counterPartyFullName, @counterPartyFullName, 'New York', '1-212-902-1000', '', '@counterPartyFullName', '', 'abc@xyz.com',    
           '', '', '', 1, 'New York', '1', '33', '',     
           'KKA', '11111', '11111', '',     
           '', '', 'New York') 

set IDENTITY_INSERT T_CounterParty OFF

INSERT INTO T_CompanyCounterParties(CompanyId,CounterPartyID)   
VALUES(@CompanyID,@counterPartyID)    
  
END  
   
BEGIN    
 --Insert CounterPartyVenue    
 INSERT T_CounterPartyVenue(CounterPartyID, VenueID, DisplayName, IsElectronic, OatsIdentifier,     
  SymbolConventionID, CurrencyID)    
 SELECT   @counterPartyID, VenueID,@counterPartyFullName,1,'',1,1 from T_Venue
  
 Set @counterPartyVenueID = scope_identity()   
 INSERT into T_companyCounterPartyVenues(CompanyId,counterpartyvenueId,CounterPartyID)  
 VALUES(@companyID,@counterPartyVenueID,@counterPartyID)   
END    
  
--Permit newly added counter party for all auec  
BEGIN    
  --Insert Data    
  INSERT INTO T_CVAUEC(CounterPartyVenueID, AUECID)        
  SELECT @counterPartyVenueID,auecid from T_AUEC   
   
END    

