CREATE PROCEDURE [dbo].[P_GetGeneralPreferences]      
(   
@userID int    
)  
as  
SELECT IsShowServiceIcons from T_GeneralPreferences where UserID = @userID
    