CREATE procedure P_SaveGeneralPreferences  
(  
@userId int,  
@IsShowServiceIcons Bit
)  
AS  
  
   
if ((select count(1) from T_GeneralPreferences where UserID = @userId) = 0)  
begin  
Insert into T_GeneralPreferences(UserID, IsShowServiceIcons)   
Values(@userID,@IsShowServiceIcons)  
end  
ELSE  
begin  
Update T_GeneralPreferences   
set IsShowServiceIcons = @IsShowServiceIcons
Where UserID = @userId

END  