
CREATE PROCEDURE [dbo].[P_GetPTTDetails]  
 @AllocationPrefID int,  
 @OrderSideId varchar(10)  
AS  
DECLARE @OriginalAllocationPrefId int = (SELECT TOP 1 PTTId FROM T_PTTAllocationMapping WHERE AllocationPrefId = @AllocationPrefID);  
SELECT TOP 1  
 [T_PTTDefinition].[Symbol],  
 CONVERT(DECIMAL(32,10), [T_PTTDefinition].[Target]) as [Target],  
 [T_PTTDefinition].[Type],  
 [T_PTTDefinition].[Add_Set],  
 [T_PTTDefinition].[MasterFundOrAccount],  
 [T_PTTDefinition].[CombinedAccountsTotalValue],  
 CONVERT(DECIMAL(32,10), [T_PTTDefinition].[Price]) as Price,
 [T_PTTDefinition].[IsTradeBreak],
 [T_PTTDefinition].[IsRoundLot],
 [T_PTTDefinition].[SelectedFundIds]
FROM [T_PTTDefinition]  
WHERE [T_PTTDefinition].[PTTId] = @OriginalAllocationPrefId  
  
SELECT  
 [T_PTTDetails].AccountId,  
 CONVERT(DECIMAL(32,10),[T_PTTDetails].[StartingPosition]) as StartingPosition,  
 CONVERT(DECIMAL(32,10), [T_PTTDetails].[StartingValue]) as StartingValue,  
 [T_PTTDetails].AccountNAV,  
 CONVERT(DECIMAL(32,10), [T_PTTDetails].[StartingPercentage]) as StartingPercentage,  
 CONVERT(DECIMAL(32,10), [T_PTTDetails].[PercentageType]) as PercentageType,  
 CONVERT(DECIMAL(32,10), [T_PTTDetails].TradeQuantity) as TradeQuantity,  
 CONVERT(DECIMAL(32,10), [T_PTTDetails].[EndingPercentage]) as EndingPercentage,  
 CONVERT(DECIMAL(32,10), [T_PTTDetails].[EndingPosition]) as EndingPosition,  
 CONVERT(DECIMAL(32,10),[T_PTTDetails].[EndingValue]) as EndingValue,  
 CONVERT(DECIMAL(32,10), [T_PTTDetails].[PercentageAllocation]) as PercentageAllocation, 
 [T_PTTDetails].[OrderSideID]
FROM [T_PTTDetails]  
WHERE [T_PTTDetails].[PTTId] = @OriginalAllocationPrefId AND [T_PTTDetails].OrderSideID=@OrderSideId  
RETURN 0