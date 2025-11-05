

/****************************************************************************                                                                                            
Name :   [P_SaveLayout]                                                                                            
Purpose:  Saves the given layout with its appearance details.  
Author : Bhupesh Bareja  
Usage: exec P_SaveLayout 1, 1, 1, 1, 'Normal', 1, 1, 1, 1  
Module: PranaLayout                           
****************************************************************************/
  
CREATE PROCEDURE [dbo].[P_SaveLayout] (      
@LeftX int,      
 @RightY int,      
 @Height int,      
 @Width int,      
 @WindowState varchar(50),      
 @IsInUse int,      
 @LayoutID int,      
 @ModuleID int,   
 @userID int,
 @QTTIndex int     
)      
AS       
--Declare @result int      
--Insert Layout Details      
      
 INSERT T_LayoutComponentDetails(LeftX ,      
 RightY ,      
 Height ,      
 Width ,      
 WindowState ,      
 IsInUse ,      
 LayoutID ,      
 ModuleID,  
 UserID,
 QTTIndex)      
       
 Values(@LeftX ,      
 @RightY ,      
 @Height ,      
 @Width ,      
 @WindowState ,      
 @IsInUse ,      
 @LayoutID ,      
 @ModuleID,  
 @userID,
 @QTTIndex )    
       
     
-- Set @result = 1      
       
-- Select @result      
  -- end      
      
--select @result
