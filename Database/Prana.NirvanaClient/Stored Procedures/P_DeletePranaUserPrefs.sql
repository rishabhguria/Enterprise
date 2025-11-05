CREATE procedure [dbo].[P_DeletePranaUserPrefs] (    
@userID int,    
@fileName varchar(40)    
)    
as    
Delete  from T_PranaUserPrefs where userid =@userID and FileName LIKE '%'+@fileName+'%'  
RETURN 0
