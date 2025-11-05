  
  
/****************************************************************************                                                                                              
Name :   [P_GetLastSavedLayoutForUser]                                                                                              
Purpose:  To fetch the last saved layout Details corresponding to a particular user id passed.    
Author : Bhupesh Bareja    
Usage: exec P_GetLastSavedLayoutForUser 1    
Module: PranaLayout                             
****************************************************************************/    
CREATE PROCEDURE [dbo].[P_GetLastSavedLayoutForUser] (            
  @userID int             
 )            
AS            
      
    
declare @layoutID int      
set @layoutID = (Select LayoutID FROM T_LayoutComponentDetails WHERE LayoutModuleID =       
           (Select MAX(LayoutModuleID) FROM T_LayoutComponentDetails Where UserID = @userID))      
      
 Select       
 LayoutID,            
 LayoutName        
 From T_Layout        
 Where LayoutID = @layoutID AND UserID = @userID      
      
      
/*Select         
 LayoutID,            
 LayoutName        
 From T_Layout        
 Where LayoutID = (Select Max(LayoutID) FROM T_Layout Where UserID = @userID)    */  