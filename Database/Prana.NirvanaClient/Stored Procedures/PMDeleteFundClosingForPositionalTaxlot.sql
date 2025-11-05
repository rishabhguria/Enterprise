              
  --PMDeleteFundClosingForPositionalTaxlot             
--Author  : Rajat                
--Date  : 14 March 08                
--Description : This procedure is used to unwind all the Fund closing for the supplied @PositionalTaxlotId             
--Modified By Abhishek           
--date : 20 May 2008          
--as now Expired Position can be unwind From same Sp              
CREATE Procedure [dbo].[PMDeleteFundClosingForPositionalTaxlot] (                
 @PositionalTaxlotId varchar(50)                
)                
as                
                
Delete from PM_TaxlotClosing                 
where PositionalTaxlotId = @PositionalTaxlotId              
          
          
