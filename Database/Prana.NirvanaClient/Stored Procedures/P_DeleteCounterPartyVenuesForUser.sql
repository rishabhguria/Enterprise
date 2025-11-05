  
  
  
  
/****** Object:  Stored Procedure dbo.P_DeleteCounterPartyVenuesForUser    Script Date: 11/17/2005 9:50:24 AM ******/  
CREATE PROCEDURE [dbo].[P_DeleteCounterPartyVenuesForUser]  
 (  
  @userID int,  
  @companyUserCCPVID varchar(MAX) = ''  
 )  
AS  
 if(@companyUserCCPVID = '')   
 begin  
  Delete T_CompanyUserCounterPartyVenues  
   Where CompanyUserID = @userID   
 end  
 else  
 begin  
   
  exec ('Delete T_CompanyUserCounterPartyVenues  
  Where convert(varchar, CompanyUserCounterPartyCVID) NOT IN(' + @companyUserCCPVID + ') AND CompanyUserID = ' + @userID)  
    
 end  
   
   
 --Delete T_CompanyUserCounterPartyVenues  
 --Where CompanyUserID = @userID  
  
  
  
  