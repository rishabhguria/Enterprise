CREATE PROCEDURE [dbo].[P_GetMainFormLayoutForUser] (          
  @userID int           
 )          
AS   

SELECT
LayoutModuleID,
LeftX ,
RightY ,
Height ,
Width ,        
WindowState ,        
IsInUse , 
LayoutID,
ModuleID,  
--ComponentName,      
UserID    
FROM
T_LayoutComponentDetails
WHERE ModuleID = 52
and LayoutID in
(
Select  top(1)    
LayoutID      
From T_Layout      
Where UserID = @userID    
Order By LastUsed DESC
)
