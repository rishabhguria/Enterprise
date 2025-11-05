--Select dbo.[GetSideMultiplierForClosing] ('0','1','1')          
CREATE FUNCTION [dbo].[GetSideMultiplierForClosing_Old]           
(                
 @ClosingMode varchar(1),          
 @OpeningPositionOrderSideTagValue Varchar(1),                
 @ClosingPositionOrderSideTagValue Varchar(1)          
)                
RETURNS  int                
AS                
BEGIN              
              
declare @sideMultiplier int               
          
set @sideMultiplier =       
CASE @ClosingMode              
  WHEN '0' THEN --OffSet             
  CASE @OpeningPositionOrderSideTagValue              
   WHEN '1'  THEN  --'Buy'   --A,1,B    2,D              
    CASE @ClosingPositionOrderSideTagValue             
     WHEN '2' --Sell             
     THEN 1             
     WHEN '5' -- Sell short             
     THEN 1   
  WHEN 'C' -- Sell to Close             
     THEN 1   
  WHEN 'D' -- Sell to Open             
     THEN 1            
   --   Else 0          
    END              
   WHEN 'A' THEN --Buy to open              
    CASE @ClosingPositionOrderSideTagValue              
    WHEN '2' --Sell             
     THEN 1             
     WHEN '5' -- Sell short             
     THEN 1   
  WHEN 'C' -- Sell to Close             
     THEN 1   
  WHEN 'D' -- Sell to Open             
     THEN 1          
   --   Else 0          
     END              
     WHEN 'B' THEN --Buy to close               
    CASE @ClosingPositionOrderSideTagValue            
     WHEN '2' --Sell             
     THEN 1             
     WHEN '5' -- Sell short             
     THEN 1   
  WHEN 'C' -- Sell to Close             
     THEN 1   
  WHEN 'D' -- Sell to Open             
     THEN 1          
   --     Else 0          
      END              
     WHEN '5' THEN  --Sell short            
    CASE @ClosingPositionOrderSideTagValue            
     WHEN '1'  --Buy            
     THEN -1             
     WHEN 'A'  --Buy to open              
     THEN -1    
     WHEN 'B'  --Buy to close              
     THEN -1          
   --   Else 0          
     END              
     WHEN 'C' THEN  --Sell to Open             
    CASE @ClosingPositionOrderSideTagValue       
     WHEN '1'  --Buy            
     THEN -1             
     WHEN 'A'  --Buy to open              
     THEN -1    
     WHEN 'B'  --Buy to close              
     THEN -1              
   --   Else 0          
     END              
     WHEN '2' THEN --Sell             
    CASE @ClosingPositionOrderSideTagValue            
     WHEN '1'  --Buy            
     THEN -1             
     WHEN 'A'  --Buy to open              
     THEN -1    
     WHEN 'B'  --Buy to close              
     THEN -1             
   --    Else 0            
     END    
 WHEN 'D' THEN --Sell to Close             
    CASE @ClosingPositionOrderSideTagValue            
     WHEN '1'  --Buy            
     THEN -1             
     WHEN 'A'  --Buy to open              
     THEN -1    
     WHEN 'B'  --Buy to close              
     THEN -1             
   --    Else 0            
     END            
    Else 0          
    END              
  WHEN '1' THEN  --Cash            
    CASE @OpeningPositionOrderSideTagValue              
      WHEN 'A'  --Buy To Open            
      THEN 1             
      WHEN 'B'  -- Buy To Close/BTC            
      THEN 1             
      WHEN 'C'  -- Sell To Open            
      THEN -1        
   WHEN '5'  -- Sell short      
      THEN -1        
--    Else 0          
    END              
  WHEN '2' THEN --Exercise            
    CASE @OpeningPositionOrderSideTagValue             
      WHEN 'A'   --Buy To Open             
      THEN 1              
      WHEN 'B'   -- Buy To Close/BTC            
      THEN 1              
      WHEN 'C'   -- Sell To Open           
      THEN -1       
      WHEN '5'   -- Sell short        
      THEN -1             
      WHEN 'D'   -- Sell To Close         
      THEN -1           
--    Else 0          
    END              
  WHEN '3' THEN   -- Expire           
    CASE @OpeningPositionOrderSideTagValue             
       WHEN 'A'    --Buy To Open           
      THEN 1             
      WHEN 'B'     -- Buy To Close/BTC            
      THEN 1             
      WHEN 'C'     -- Sell To Open             
      THEN -1        
   WHEN '5'     -- Sell short         
      THEN -1         
--     Else 0          
    END              
  WHEN '4' THEN  -- Physical              
  CASE @OpeningPositionOrderSideTagValue              
      WHEN 'A'    --Buy To Open          
      THEN 1              
      WHEN 'B'    -- Buy To Close/BTC             
      THEN 1              
      WHEN 'C'    -- Sell To Open            
      THEN -1       
      WHEN '5'    -- Sell short             
      THEN -1             
      WHEN 'D'    -- Sell To Close           
      THEN -1            
     END           
--     Else 0          
 END            
           
              
RETURN (@sideMultiplier)                
                
END       