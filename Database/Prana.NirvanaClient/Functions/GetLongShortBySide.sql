-- =============================================      
-- Author:  Sandeep Singh      
-- Create date: 03 Feb 2010      
-- Description: Returns the side multiplier. For buy and sell sides it is Long else Short 
--Select * from T_Side   
--Select [dbo].[GetLongShortBySide](2) 
-- =============================================      
CREATE FUNCTION [dbo].[GetLongShortBySide]
(      
 @SideId varchar(1)      
)      
RETURNS  Varchar(10)      
AS      
BEGIN    
    
declare @LS Varchar(10)     
   
set @LS = case @sideID    
       When '1' THEN  'Long'  --Buy 
       When '2' THEN  'Long'  --Sell  
       When 'A' THEN  'Long'   --Buy to Open
       When 'D' THEN  'Long'   --Sell to Close
       When '5' THEN  'Short'  --Sell short                 
       When 'B' THEN  'Short'  --Buy to Close  
       When 'C' THEN 'Short'   --Sell to Open  
       End  
    
RETURN (@LS)      
      
END  

