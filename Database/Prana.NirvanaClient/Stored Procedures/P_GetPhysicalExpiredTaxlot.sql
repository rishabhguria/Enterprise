    
-- =============================================          
-- Author:  <Author,,Name>          
-- Create date: <Create Date,,>          
-- Description: <Description,,>          
-- =============================================          
CREATE PROCEDURE [dbo].[P_GetPhysicalExpiredTaxlot] (@TaxlotID  varchar(50))           
AS          
BEGIN          
Select FA.AllocationID, G.Symbol,G.OrderSideTagValue,G.CumQty,G.AvgPrice,    
G.AssetID,FA.FundID,ES.SettlementQty,G.AUECLocalDate ,GC.Commission,GC.Fees ,G.CumQty,ES.PK_Settlement_expirationID  
    
from T_FundAllocation FA    
inner join T_Group G on G.GroupID = FA.GroupID   
inner join T_GroupCommission GC on GC.GroupID_Fk =G.GroupID   
inner join T_Expire_Settlement ES on ES.GeneratedTaxlotID=FA.AllocationID      
 where ES.Expire_SettleTaxlotID =@TaxlotID      
    
END  