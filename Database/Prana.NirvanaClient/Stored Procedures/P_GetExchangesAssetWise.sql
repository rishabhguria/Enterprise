-- =============================================  
-- Created by: Bharat Raturi
-- date: 03 jun, 2014
-- Description: Get the exchanges by assets
--usage: P_GetExchangesAssetWise
-- =============================================  
CREATE PROCEDURE [dbo].[P_GetExchangesAssetWise]
AS  
BEGIN    

 SELECT T_Asset.AssetID, ExchangeID, DisplayName
 from 
T_Asset inner JOIN 
(
select AssetID,T_Exchange.ExchangeID,T_Exchange.DisplayName 
from T_AUEC inner JOIN T_Exchange 
on T_AUEC.ExchangeID=T_Exchange.ExchangeID) t1  
on 
T_Asset.AssetID=T1.AssetID  ORDER BY T_Asset.AssetID
END   
