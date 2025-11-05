 /****** Object:  Stored Procedure dbo.P_DeleteCompanyModules    Script Date: 02/15/2006 3:30:22 PM ******/  
CREATE PROCEDURE dbo.P_DeleteCompanyThirdParties  
 (  
  @companyID int,  
  @companyThirdPartiesID varchar(max) = ''  
 )  
AS  
   
 if(@companyThirdPartiesID = '')   
 begin  
  Delete T_CompanyThirdPartyCVIdentifier Where CompanyThirdPartyID_FK IN (Select CompanyThirdPartyID From T_CompanyThirdParty Where CompanyID = @companyID)   
    
  Delete T_CompanyThirdPartyMappingDetails Where CompanyThirdPartyID_FK IN (Select CompanyThirdPartyID From T_CompanyThirdParty Where CompanyID = @companyID)   
    
  Delete T_CompanyThirdPartyFileFormats Where CompanyPrimeBrokerClearerID_FK IN (Select CompanyThirdPartyID From T_CompanyThirdParty Where CompanyID = @companyID)   
  Delete T_CompanyThirdPartyFileFormats Where CompanyCustodianID_FK IN (Select CompanyThirdPartyID From T_CompanyThirdParty Where CompanyID = @companyID)   
  Delete T_CompanyThirdPartyFileFormats Where CompanyAdministratorID_FK IN (Select CompanyThirdPartyID From T_CompanyThirdParty Where CompanyID = @companyID)   
    
  Delete T_CompanyThirdParty Where CompanyID = @companyID   
 end  
 else  
 begin  
   
  exec ( 'Delete T_CompanyThirdPartyCVIdentifier  
       Where convert(varchar, CompanyThirdPartyID_FK) NOT IN( ' + @companyThirdPartiesID + ')AND CompanyThirdPartyID_FK IN (Select CompanyThirdPartyID From T_CompanyThirdParty Where CompanyID = ' + @companyID + ')' )  
    
  exec ( 'Delete T_CompanyThirdPartyMappingDetails  
       Where convert(varchar, CompanyThirdPartyID_FK) NOT IN( ' + @companyThirdPartiesID + ')AND CompanyThirdPartyID_FK IN (Select CompanyThirdPartyID From T_CompanyThirdParty Where CompanyID = ' + @companyID + ')' )  
    
  exec ( 'Delete T_CompanyThirdPartyFileFormats  
       Where convert(varchar, CompanyPrimeBrokerClearerID_FK) NOT IN( ' + @companyThirdPartiesID + ')AND CompanyPrimeBrokerClearerID_FK IN (Select CompanyThirdPartyID From T_CompanyThirdParty Where CompanyID = ' + @companyID + ')' )  
         
     exec ( 'Delete T_CompanyThirdPartyFileFormats  
       Where convert(varchar, CompanyCustodianID_FK) NOT IN( ' + @companyThirdPartiesID + ')AND CompanyCustodianID_FK IN (Select CompanyThirdPartyID From T_CompanyThirdParty Where CompanyID = ' + @companyID + ')' )  
         
     exec ( 'Delete T_CompanyThirdPartyFileFormats  
       Where convert(varchar, CompanyAdministratorID_FK) NOT IN( ' + @companyThirdPartiesID + ')AND CompanyAdministratorID_FK IN (Select CompanyThirdPartyID From T_CompanyThirdParty Where CompanyID = ' + @companyID + ')' )  
    
  exec ('Delete T_CompanyThirdParty  
    Where convert(varchar, CompanyThirdPartyID) NOT IN(' + @companyThirdPartiesID + ') AND CompanyID = ' + @companyID)  
  
 end  
  
  