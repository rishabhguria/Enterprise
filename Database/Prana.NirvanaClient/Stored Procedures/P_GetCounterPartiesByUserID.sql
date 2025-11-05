 CREATE  PROCEDURE dbo.P_GetCounterPartiesByUserID  
 (  
    
  @userID int    
 )  
AS  
  
SELECT   distinct T_CounterParty.CounterPartyID,FullName,ShortName,IsAlgoBroker,T_CounterParty.IsOTDorEMS from T_CompanyUserCounterPartyVenues  
join T_CompanyCounterPartyVenues  
on T_CompanyUserCounterPartyVenues.CounterPartyVenueID=T_CompanyCounterPartyVenues.CounterPartyVenueID  
join T_CounterParty  
on T_CompanyCounterPartyVenues.CounterPartyID=T_CounterParty.CounterPartyID  
  
where T_CompanyUserCounterPartyVenues.CompanyUserID=@userID  
  
  order by ShortName