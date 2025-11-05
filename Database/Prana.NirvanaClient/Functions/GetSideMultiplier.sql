
-- =============================================      
-- Author:  Rajat      
-- Create date: 02 Nov 2007      
-- Description: Returns the side multiplier. For all buy sides it is +1 and for sell sides it is -1      
-- =============================================      
CREATE FUNCTION [dbo].[GetSideMultiplier] (      
 @SideId varchar(1)      
)      
RETURNS  int      
AS      
BEGIN    
    
declare @sideMultiplier int     
   
set @sideMultiplier = case @sideID  --Find out SIde multiplier      
       When '1' THEN  1   
       When 'A' THEN  1    
       When 'B' THEN  1   
       When '3' THEN  1 	   
       When '2' THEN  -1   
       When '5' THEN  -1                     
       When 'C' THEN  -1    
       When 'D' THEN -1   
	   When 'E' THEN 1  
       end  
    
RETURN (@sideMultiplier)      
      
END 


