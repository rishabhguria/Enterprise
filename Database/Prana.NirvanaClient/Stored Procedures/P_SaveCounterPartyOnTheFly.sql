
/*********************************************************
Author: Narendra Kumar Jangir
Creation Date: 09 July 2014
Purpose: Save all details of counter party on the fly, permit counter party for all AUEC

Exectution: EXEC P_SaveCounterPartyOnTheFly 'Narendra Kumar Jangir', 'NKJ', 1
**********************************************************/ 
 
CREATE PROCEDURE [dbo].[P_SaveCounterPartyOnTheFly]  
(   
  @counterPartyFullName varchar(50),  
  @shortName varchar(50),
  @counterPartyTypeID int   
 )  
AS   
BEGIN

Declare @counterPartyID int
Declare @counterPartyVenueID int

--TODO: Here country id and state id are hard-coded, need to pick id by name
INSERT T_CounterParty(FullName, ShortName, Address, Phone, Fax, ContactName1, Title1, Email1,  
           ContactName2, Title2, Email2, CounterPartyTypeID, Address2, CountryID, StateID, Zip,   
           ContactName1_LastName, ContactName1_WorkPhone, ContactName1_Cell, ContactName2_LastName,   
           ContactName2_WorkPhone, ContactName2_Cell, City)  
Values(@counterPartyFullName, @shortName, '', '1-212-902-1000', '', '', '', 'abc@xyz.com',  
           '', '', '', @counterPartyTypeID, '', '1', '33', '',   
           '', '', '', '',   
           '', '', '') 
set @counterPartyID = scope_identity()
INSERT INTO T_CompanyCounterParties(CompanyId,CounterPartyID) 
VALUES(-1,@counterPartyID)  

END

Declare @autoVenueID int
select @autoVenueID = VenueID from t_venue where venuename = 'Auto'

--TODO: Currently venue id is hard-coded, venue id should be of drops as discussed with joginder
--Here symbol convention id and currency id are hard-coded.
BEGIN  
	--Insert CounterPartyVenue  
	INSERT T_CounterPartyVenue(CounterPartyID, VenueID, DisplayName, IsElectronic, OatsIdentifier,   
	 SymbolConventionID, CurrencyID)  
	Values(@counterPartyID, @autoVenueID, @counterPartyFullName, 1, '', 
	7, 1)     
	Set @counterPartyVenueID = scope_identity() 
	INSERT into T_companyCounterPartyVenues(CompanyId,counterpartyvenueId,CounterPartyID)
	VALUES(-1,@counterPartyVenueID,@counterPartyID) 
END  

--Permit newly added counter party for all auec
BEGIN  
  --Insert Data  
  INSERT INTO T_CVAUEC(CounterPartyVenueID, AUECID)      
  SELECT @counterPartyVenueID,auecid from T_AUEC 
 
END  

