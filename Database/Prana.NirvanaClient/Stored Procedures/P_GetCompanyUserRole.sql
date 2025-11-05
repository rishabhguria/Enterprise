CREATE PROCEDURE [dbo].[P_GetCompanyUserRole]  
 (  
  @userID  int
 )  
AS   
  
 Select isnull(RoleID,0)as RoleID , CompanyID 
   From T_CompanyUser  
   Where UserID = @userID and isActive=1  