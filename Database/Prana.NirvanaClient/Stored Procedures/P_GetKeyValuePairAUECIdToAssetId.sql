
/*                  
Author  : Rajat                  
Date  : 27 Dec 2007                  
Description : Returns the key value pair of AUECId to AssetId.
--Drop Procedure [P_GetKeyValuePairAUECIdToAssetId]                  
Usage :                   
P_GetKeyValuePairAUECIdToAssetId '2007-12-26 02:10:00'                  
*/                  
CREATE PROCEDURE [dbo].[P_GetKeyValuePairAUECIdToAssetId] As                  
                  
BEGIN                  
   
select AUECId, AssetId from T_AUEC
     
END
