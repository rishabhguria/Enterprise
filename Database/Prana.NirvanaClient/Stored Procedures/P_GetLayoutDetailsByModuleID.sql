CREATE PROCEDURE [dbo].[P_GetLayoutDetailsByModuleID]
	 @LayoutID int,          
     @userID int,
     @moduleId int 
AS
	Select LayoutModuleID,            
 LeftX ,            
 RightY ,            
 Height ,            
 Width ,            
 WindowState ,            
 IsInUse ,            
 LCD.LayoutID ,            
 ModuleID,        
 ComponentName as ModuleName,        
 LCD.UserID            
 From T_LayoutComponentDetails LCD INNER JOIN T_Layout L ON LCD.LayoutID = L.LayoutID left outer join        
   T_LayoutComponents LC ON LCD.ModuleID = LC.ComponentID        
 Where LCD.LayoutID = @LayoutID AND LCD.UserID = @userID AND LCD.ModuleID = @moduleId
RETURN 0
