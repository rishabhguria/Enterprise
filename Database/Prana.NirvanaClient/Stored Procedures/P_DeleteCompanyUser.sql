/****** Object:  Stored Procedure dbo.P_DeleteCompanyUser    Script Date: 11/17/2005 9:50:24 AM ******/  
CREATE PROCEDURE [dbo].[P_DeleteCompanyUser]  
 (  
  @companyUserID int  
 )  
AS  
   
 --T_RMUserTradingAccount  
 Declare @total int  
 Select @total = Count(1)   
  From T_RMCompanyUsersOverall  
  Where CompanyUserID = @companyUserID  
   
   if ( @total = 0)  
   begin  
     
    Select @total = Count(1)   
    From T_RMCompanyUserUI  
    Where CompanyUserID = @companyUserID  
     
    if ( @total = 0)  
    begin  
     
     Select @total = Count(1)   
     From T_RMUserTradingAccount  
     Where CompanyUserID = @companyUserID  
      
      if ( @total = 0)  
      begin  
       --Delete Corresponding Company Client Details   
       -- If Company User is referenced anywhere and still we want to delete it.  
       --Delete Company User and related information.  
       -------------------------- Start : Delete Company User Permissions --------------------------  
       
        
       -- Delete Company User CounterpartyVenues.  
         
       Delete T_CompanyUserCounterPartyVenues   
       Where CompanyUserID = @companyUserID  
         
       -- Delete Company User Application Component \ Module.  
       Delete T_CompanyUserModule   
       Where CompanyUserID = @companyUserID  
         
       -- Delete Company User AUEC's.  
       Delete T_CompanyUserAUEC   
       Where CompanyUserID = @companyUserID  
         
       -- Delete Company User Trading Account.  
       Delete T_CompanyUserTradingAccounts   
       Where CompanyUserID = @companyUserID  
         
		 --Delete from T_AuditTrailOtherPermissions
		 delete from T_AuditTrailOtherPermissions
		 where UserId=@companyUserID
        
        --Delete from T_CompanyUserFundGroupMapping
		 delete from T_CompanyUserFundGroupMapping
		 where CompanyUserID=@companyUserID
		
		 
       -------------------------- End : Delete Company User Permissions --------------------------  
         
       update T_CompanyUser SET IsActive=0
       Where UserID = @companyUserID  
         
       return @companyUserID  
      end  
    end  
    else  
    begin  
     return -1  
    end   
   end  
   else  
   begin  
    return -1  
   end  
