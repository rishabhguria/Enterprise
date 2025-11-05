  
  
/****************************************************************************                                                                                              
Name :   [P_GetLayoutDetailsByID]                                                                                              
Purpose:  Gets the appearance and other layout details against the given layout id.    
Author : Bhupesh Bareja    
Usage: exec P_GetLayoutDetailsByID 1, 1  
Module: PranaLayout                             
****************************************************************************/  
    
CREATE PROCEDURE [dbo].[P_GetLayoutDetailsByID] (          
 @LayoutID int,        
  @userID int           
 )          
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
 LCD.UserID,
 QTTIndex          
 From T_LayoutComponentDetails LCD INNER JOIN T_Layout L ON LCD.LayoutID = L.LayoutID left outer join      
   T_LayoutComponents LC ON LCD.ModuleID = LC.ComponentID      
 Where LCD.LayoutID = @LayoutID AND LCD.UserID = @userID   