-----------------------------------------------------  
--Created By: Bharat raturi  
--Date: 04/06/2014  
--Purpose: Get the assets for defining pricing rule  
--usage: P_GetAllAssetsForPricingRules
-----------------------------------------------------  
CREATE procedure [dbo].[P_GetAllAssetsForPricingRules]  
as 
BEGIN
Select AssetID, AssetName from T_Asset
where AssetID<>6
END 
