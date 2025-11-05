--Select dbo.[GetSideMultiplierForClosing] ('0','5','1')          
CREATE FUNCTION [dbo].[GetSideMultiplierForClosing]           
(                
 @OpeningPositionOrderSideTagValue Varchar(1),                
 @ClosingPositionOrderSideTagValue Varchar(1)          
)                
RETURNS  int                
AS                
BEGIN              
              
declare @sideMultiplier int               
          
set @sideMultiplier =       
  CASE               
   WHEN @OpeningPositionOrderSideTagValue='1' Or @OpeningPositionOrderSideTagValue='A' Or @OpeningPositionOrderSideTagValue='B'
   THEN  --'Buy'   --A,1,B    2,D              
		CASE              
			 WHEN @ClosingPositionOrderSideTagValue = '2' Or @ClosingPositionOrderSideTagValue='5' Or @ClosingPositionOrderSideTagValue='C' Or @ClosingPositionOrderSideTagValue='D'            
			 THEN 1 		          
	   --   Else 0          
		END              
           
 WHEN @OpeningPositionOrderSideTagValue='5' Or @OpeningPositionOrderSideTagValue='C' Or @OpeningPositionOrderSideTagValue='2' Or @OpeningPositionOrderSideTagValue='D'
 THEN  --Sell short            
	CASE             
		 WHEN @ClosingPositionOrderSideTagValue='1' Or @ClosingPositionOrderSideTagValue='A' Or @ClosingPositionOrderSideTagValue='B'            
		 THEN -1             		          
	   --   Else 0          
	END              
             

  End    
           
              
RETURN (@sideMultiplier)                
                
END       