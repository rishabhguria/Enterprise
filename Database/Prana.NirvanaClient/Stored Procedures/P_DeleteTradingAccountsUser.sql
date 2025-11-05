  
  
  
  
/****** Object:  Stored Procedure dbo.P_DeleteTradingAccountsUser    Script Date: 11/17/2005 9:50:24 AM ******/  
CREATE PROCEDURE [dbo].[P_DeleteTradingAccountsUser]  
 (  
  @userID int,  
  @companyUserTAID varchar(max) = ''  
 )  
AS  
 if(@companyUserTAID = '')   
 begin  
  Delete T_CompanyUserTradingAccounts  
   Where CompanyUserID = @userID   
 end  
 else  
 begin  
   
  exec ('Delete T_CompanyUserTradingAccounts  
  Where convert(varchar, CompanyUserTradingAccountID) NOT IN(' + @companyUserTAID + ') AND CompanyUserID = ' + @userID)  
    
 end  
   
   
   
   
   
   
 --Delete T_CompanyUserTradingAccounts  
 --Where CompanyUserID = @userID  
  
  
  
  