   /****** Object:  Stored Procedure dbo.P_DeleteCompanyUserStrategies    Script Date: 04/12/2006 01:00:22 PM ******/  
CREATE PROCEDURE dbo.P_DeleteCompanyUserStrategies  
 (  
  @companyUserID int,  
  @companyUserStrategyID varchar(max) = ''  
 )  
AS  
 if(@companyUserStrategyID = '')   
 begin  
  Delete T_CompanyUserStrategies  
   Where CompanyUserID = @companyUserID   
 end  
 else  
 begin  
   
  exec ('Delete T_CompanyUserStrategies  
  Where convert(varchar, CompanyUserStrategyID) NOT IN(' + @companyUserStrategyID + ') AND CompanyUserID = ' + @companyUserID)  
    
 end  
  
  
  