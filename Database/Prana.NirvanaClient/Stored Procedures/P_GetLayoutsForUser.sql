  

/****************************************************************************                                                                                              
Name :   [P_GetLayoutsForUser]                                                                                              
Purpose:  To fetch all the Layouts corresponding to a particular user id passed.  
Author : Bhupesh Bareja    
Usage: exec P_GetLayoutsForUser 1    
Module: PranaLayout                             
****************************************************************************/    
CREATE PROCEDURE [dbo].[P_GetLayoutsForUser] (        
  @userID int         
 )        
AS        
Select     
 LayoutID,        
 LayoutName    
 From T_Layout    
 Where UserID = @userID  
  Order By LastUsed DESC        
