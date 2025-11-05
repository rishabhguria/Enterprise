  
/****** Object:  Stored Procedure dbo.P_DeleteCompanyModules    Script Date: 11/17/2005 9:50:22 AM ******/  
CREATE PROCEDURE dbo.P_DeleteCompanyAUEC  
 (  
  @companyID int,  
  @companyAUECID varchar(MAX)
 )  
AS  
 if(@companyAUECID = '')   
 begin  
    
  Set @companyAUECID = '-1,-2'  
  exec P_DeleteCompanyThirdPartyCVCommissionRulesAUEC @companyID, @companyAUECID  
    
  exec ( 'Delete T_CompanyUserAUEC  
       Where convert(varchar, CompanyAUECID) NOT IN( ' + @companyAUECID + ')AND CompanyAUECID IN (Select CompanyAUECID From T_CompanyAUEC Where CompanyID = ' + @companyID + ')' )  
    
  exec ( 'Delete T_CompanyClientAUEC  
       Where convert(varchar, CompanyAUECID) NOT IN( ' + @companyAUECID + ')AND CompanyAUECID IN (Select CompanyAUECID From T_CompanyAUEC Where CompanyID = ' + @companyID + ')' )  
    
  Delete T_CompanyAUEC  
   Where CompanyID = @companyID   
 end  
 else  
 begin  
   
  exec P_DeleteCompanyThirdPartyCVCommissionRulesAUEC @companyID, @companyAUECID  
    
    
  exec ( 'Delete T_CompanyAUECClearanceTimeBlotter Where CompanyAUECID IN (Select CompanyAUECID From T_CompanyAUEC Where CompanyID = ' + @companyID + ')' )  

  exec ( 'Delete T_CompanyUserAUEC  
       Where convert(varchar, CompanyAUECID) NOT IN( ' + @companyAUECID + ')AND CompanyAUECID IN (Select CompanyAUECID From T_CompanyAUEC Where CompanyID = ' + @companyID + ')' )  
    
  exec ( 'Delete T_CompanyClientAUEC  
       Where convert(varchar, CompanyAUECID) NOT IN( ' + @companyAUECID + ')AND CompanyAUECID IN (Select CompanyAUECID From T_CompanyAUEC Where CompanyID = ' + @companyID + ')' )  
    
  exec ( 'Delete T_CompanyAUEC  
    Where convert(varchar, CompanyAUECID) NOT IN(' + @companyAUECID + ') AND CompanyID = ' + @companyID)  
     
    
 end