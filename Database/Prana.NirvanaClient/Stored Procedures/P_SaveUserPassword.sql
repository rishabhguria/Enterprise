CREATE PROCEDURE [dbo].[P_SaveUserPassword]
(
  @userID int,    
  @Password varchar(100)   
)
AS

declare @total int
Declare @result int
Set @total=0

Select @total = Count(*)    
From T_CompanyUser    
Where UserID = @userID 

if(@total>0)
begin
Update T_CompanyUser
set Password=@Password
where UserID=@userID 
set @result=@userID 
end
else
begin
Set @result = -1
end
select @result
