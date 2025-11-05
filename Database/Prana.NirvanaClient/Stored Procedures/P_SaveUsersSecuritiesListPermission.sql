CREATE PROCEDURE [dbo].[P_SaveUsersSecuritiesListPermission]
	@companyUserID int,
	@readWriteID int
AS
Declare @total int   
Set @total = 0  
  
Select @total = Count(*)  
From T_UsersSecuritiesListPermission   
Where CompanyUserID = @companyUserID 

if(@total > 0)  
begin   
Update T_UsersSecuritiesListPermission  
set  
  Read_WriteID = @readWriteID
Where CompanyUserID = @companyUserID    
End 
else  
begin  
INSERT T_UsersSecuritiesListPermission(CompanyUserID, Read_WriteID)  
Values(@companyUserID, @readWriteID)        
end   

