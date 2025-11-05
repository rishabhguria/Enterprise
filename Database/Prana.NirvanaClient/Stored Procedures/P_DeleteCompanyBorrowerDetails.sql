  
/****** Object:  Stored Procedure dbo.P_DeleteCompanyBorrowerDetails    Script Date: 05/18/2006 8:45:23 PM ******/  
CREATE PROCEDURE dbo.P_DeleteCompanyBorrowerDetails  
 (    
  @companyID int,  
  @companyBorrowerID varchar(max) = ''  
 )  
AS  
   
 if(@companyBorrowerID = '')   
 begin  
  Delete T_CompanyBorrower  
   Where CompanyID = @companyID   
 end  
 else  
 begin  
   
  exec ('Delete T_CompanyBorrower  
  Where convert(varchar, CompanyBorrowerID) NOT IN(' + @companyBorrowerID + ') AND CompanyID = ' + @companyID)  
     
 end  