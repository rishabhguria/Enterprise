  
  
/****** Object:  Stored Procedure dbo.P_GetCompanyFunds    Script Date: 11/17/2005 9:50:23 AM ******/    
Create PROCEDURE [dbo].[P_GetCompanyUserIDFromName] 
(    
  @UserName varchar(100)  
 )    
AS    
 SELECT     
UserID 
 FROM T_CompanyUser
 Where [Login] = @UserName
