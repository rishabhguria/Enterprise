

/****************************************************************************                                                                                            
Name :   [P_DeleteOldLayoutDetails]                                                                                            
Purpose:  Delete all the layouts against the given layoutid.  
Author : Bhupesh Bareja  
Parameters:  @LayoutID int  
Usage: exec P_DeleteOldLayoutDetails 1  
Module: PranaLayout                           
****************************************************************************/    
    
CREATE PROCEDURE [dbo].[P_DeleteOldLayoutDetails] (    
  @LayoutID int    
 )    
AS     
--Declare @result int    
Declare @total int     
Set @total = 0    
    
Select @total = Count(*)    
From T_LayoutComponentDetails    
Where LayoutID = @LayoutID     
    
if(@total > 0)    
begin    
    
Delete From    
T_LayoutComponentDetails    
Where LayoutID = @LayoutID      
end
