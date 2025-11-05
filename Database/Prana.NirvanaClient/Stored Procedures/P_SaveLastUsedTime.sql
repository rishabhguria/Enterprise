    
    
/****************************************************************************                                                                                                
Name :   P_SaveLastUsedTime                                                                                                
Purpose: Save last used time of a layout      
Author : Sumit Kakra      
Usage: exec procedure P_SaveLastUsedTime 1      
Module: PranaLayout                               
****************************************************************************/      
CREATE PROCEDURE [dbo].P_SaveLastUsedTime (              
  @LayoutID int               
 )              
AS    
Update T_Layout  
 Set LastUsed = GetUTCDate()  
  Where LayoutID = @LayoutID

