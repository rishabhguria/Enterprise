   /****** Object:  Stored Procedure dbo.P_DeleteCompanyUserAUEC    Script Date: 03/07/2006 12:20:22 AM ******/  
CREATE PROCEDURE dbo.P_DeleteCompanyUserAUEC  
 (  
  @companyUserID int,  
  @companyUserAUECID varchar(max) = ''  
 )  
AS  
 if(@companyUserAUECID = '')   
 begin  
  Delete T_CompanyUserAUEC  
   Where CompanyUserID = @companyUserID   
 end  
 else  
 begin  
   
  exec ('Delete T_CompanyUserAUEC  
  Where convert(varchar, CompanyUserAUECID) NOT IN(' + @companyUserAUECID + ') AND CompanyUserID = ' + @companyUserID)  
    
 end  
  
  
  