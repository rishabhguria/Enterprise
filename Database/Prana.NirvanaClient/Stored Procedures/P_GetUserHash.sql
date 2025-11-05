CREATE PROCEDURE [dbo].[P_GetUserHash]    
(      
  @login VARCHAR(20),
  @samsaraAzureId VARCHAR(50)
)    
AS     

DECLARE @count int    
set @count = 0    

if (@samsaraAzureId = '')
begin
    select @count = count(*)
    from T_CompanyUser
    Where Login = @login
          and CAST(Login AS varbinary(20)) = CAST(@login AS varbinary(20))
          and isActive = 1
    if (@count > 0)
    begin
        select UserID,
               Password,
               Login
        from T_CompanyUser
        Where Login = @login
              and CAST(Login AS varbinary(20)) = CAST(@login AS varbinary(20))
              and isActive = 1
    end
end
else
begin
    select @count = count(*)
    from T_CompanyUser
    Where LOWER(SamsaraAzureId) = @samsaraAzureId
          and isActive = 1

    if (@count > 0)
    begin
        select UserID,
               Password,
               Login
        from T_CompanyUser
        Where LOWER(SamsaraAzureId) = @samsaraAzureId
              and isActive = 1
    end
end