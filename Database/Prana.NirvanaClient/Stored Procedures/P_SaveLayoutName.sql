    
    
/****************************************************************************                                                                                                
Name :   [P_SaveLayoutName]                                                                                                
Purpose:  Save the Layout Name and user id passed.    
Author : Bhupesh Bareja      
Usage: exec P_SaveLayoutName 'Blotter', 1    
Module: PranaLayout                               
****************************************************************************/      
CREATE PROCEDURE [dbo].[P_SaveLayoutName] (        
  @LayoutName varchar(50),      
  @userID int        
 )        
        
AS        
        
Declare @LayoutID int         
Set @LayoutID = -1        
        
Select @LayoutID = LayoutID From T_Layout        
 Where LayoutName = @LayoutName  and UserID = @userID     
        
If @LayoutID  = -1        
Begin        
 Declare @result int        
  INSERT T_Layout        
  (         
  LayoutName,      
  UserID,  
  LastUsed      
  )        
  Values(@LayoutName, @userID, GetUtcDate() )        
         
  Set @result = scope_identity()        
        
  Select @result         
End        
Else        
Begin        
   Select @LayoutID        
End        
           
          
           
        
--select * from  T_Layout