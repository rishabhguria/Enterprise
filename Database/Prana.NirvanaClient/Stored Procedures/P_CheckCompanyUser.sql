CREATE PROCEDURE [dbo].[P_CheckCompanyUser]    
(    
  @userID varchar(50),    
  @shortName varchar(50),    
  @login varchar(50)
)    
AS     

declare @count int    
set @count = 0    
    
 select @count = count(*)    
  from T_CompanyUser    
  Where (ShortName = @shortName OR Login = @login) AND UserID <> @userID

  select @login = login
  from T_CompanyUser 
  Where UserID = @userID

  select @count,@login