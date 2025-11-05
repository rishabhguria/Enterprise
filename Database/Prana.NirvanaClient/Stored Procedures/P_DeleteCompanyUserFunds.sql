   /****** Object:  Stored Procedure dbo.P_DeleteCompanyUserFunds    Script Date: 04/12/2006 03:20:22 PM ******/  
CREATE PROCEDURE dbo.P_DeleteCompanyUserFunds  
 (  
  @companyUserID int,  
  @companyUserFundID varchar(max) = ''  
 )  
AS  
 if(@companyUserFundID = '')   
 begin  
  Delete T_CompanyUserFunds  
   Where CompanyUserID = @companyUserID   
 end  
 else  
 begin  
   
  exec ('Delete T_CompanyUserFunds  
  Where convert(varchar, CompanyUserFundID) NOT IN(' + @companyUserFundID + ') AND CompanyUserID = ' + @companyUserID)  
    
 end  
  
  
  