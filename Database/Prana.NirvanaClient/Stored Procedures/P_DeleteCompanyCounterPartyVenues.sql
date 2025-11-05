 /****** Object:  Stored Procedure dbo.P_DeleteCompanyCounterPartyVenues    Script Date: 11/17/2005 9:50:22 AM ******/  
  
CREATE PROCEDURE dbo.P_DeleteCompanyCounterPartyVenues  
 (    
  @companyID int,  
  @companyCounterPartyCVID varchar(max) = ''  
 )  
AS  
 if(@companyCounterPartyCVID = '')   
 begin   
  Delete T_CompanyUserCounterPartyVenues Where CompanyCounterPartyCVID    
   in (Select CUCPV.CompanyCounterPartyCVID   
    From T_CompanyUserCounterPartyVenues CUCPV, T_CompanyCounterPartyVenues CCPV   
     where CUCPV.CompanyCounterPartyCVID = CCPV.CompanyCounterPartyCVID and CCPV.CompanyID=@companyID)    
   /*in (Select CUCPV.CompanyCounterPartyCVID From T_CompanyUserCounterPartyVenues CUCPV,   
    T_CompanyCounterPartyVenues CCPV Where CCPV.CompanyID = @companyID)*/  
    
    
  Delete T_CompanyThirdPartyCVCommissionRules Where CompanyCounterPartyCVID IN (Select CompanyCounterPartyCVID From T_CompanyCounterPartyVenues Where CompanyID = @companyID)   
    
  Delete T_CompanyThirdPartyCVIdentifier Where CompanyCounterPartyVenueID_FK IN (Select CompanyCounterPartyCVID From T_CompanyCounterPartyVenues Where CompanyID = @companyID)   
    
  Delete T_CompanyCounterPartyVenueDetails Where CompanyCounterPartyVenueID IN (Select CompanyCounterPartyCVID From T_CompanyCounterPartyVenues Where CompanyID = @companyID)   
    
  Delete T_CompanyCounterPartyVenues Where CompanyID = @companyID  
    
  Delete T_CompanyCounterParties Where CompanyID = @companyID  
      
 end  
 else  
 begin  
   
  exec ('Delete T_CompanyThirdPartyCVCommissionRules  
  Where convert(varchar, CompanyCounterPartyCVID) NOT IN(' + @companyCounterPartyCVID + ') AND CompanyCounterPartyCVID IN (Select CompanyCounterPartyCVID From T_CompanyCounterPartyVenues Where CompanyID = ' + @companyID + ')')   
  
  exec ('Delete T_CompanyThirdPartyCVIdentifier  
  Where convert(varchar, CompanyCounterPartyVenueID_FK) NOT IN(' + @companyCounterPartyCVID + ') AND CompanyCounterPartyVenueID_FK IN (Select CompanyCounterPartyCVID From T_CompanyCounterPartyVenues Where CompanyID = ' + @companyID + ')')   
    
  exec ('Delete T_CompanyCounterPartyVenueDetails  
  Where convert(varchar, CompanyCounterPartyVenueID) NOT IN(' + @companyCounterPartyCVID + ') AND CompanyCounterPartyVenueID IN (Select CompanyCounterPartyCVID From T_CompanyCounterPartyVenues Where CompanyID = ' + @companyID + ')')   
    
  exec ('Delete T_CompanyUserCounterPartyVenues  
  Where convert(varchar, CompanyCounterPartyCVID) NOT IN(' + @companyCounterPartyCVID + ') AND CompanyCounterPartyCVID IN (Select CompanyCounterPartyCVID From T_CompanyCounterPartyVenues Where CompanyID = ' + @companyID + ')')   
    
  exec ('Delete T_CompanyCounterPartyVenues  
  Where convert(varchar, CompanyCounterPartyCVID) NOT IN(' + @companyCounterPartyCVID + ') AND CompanyID = ' + @companyID)  
    
     
 end  
  
  
  
  
  
  
  
  
/*ALTER PROCEDURE dbo.P_DeleteCompanyCounterPartyVenues  
 (  
  @companyID int  
  --@counterPartyID int,  
  --@venueID int  
 )  
AS*/  
 /*Declare @CounterPartyVenueID int  
 Set @CounterPartyVenueID = 0  
   
 -- This is done to look for the registered or existing venue for the equivalent counterparty.  
  Select @CounterPartyVenueID = CounterPartyVenueID  
 FROM T_CounterPartyVenue  
 Where CounterPartyID = @counterPartyID  
  AND VenueID = @venueID  
   
 Delete T_CompanyUserCounterPartyVenues  
 Where (CompanyUserID in (SELECT UserID   
       FROM T_CompanyUser WHERE   
       CompanyID = @companyID) )  
 AND (CounterPartyVenueID not in ( SELECT CounterPartyVenueID  
          FROM T_CompanyCounterPartyVenues  
          WHERE CompanyID = @companyID))   
 Delete T_CompanyCounterPartyVenues  
 Where CompanyID = @companyID */  
   
   
  
  