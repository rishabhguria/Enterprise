

  
-- =============================================    
-- Author:  <Harsh>    
-- Create date: <01/30/2008>    
-- Description: <PickUp AUECID, MarketStart and MarketEnd Times>    
-- =============================================    
CREATE PROCEDURE [dbo].[P_GetAllMarketStartEndTimes] AS    
BEGIN    
     
 SET NOCOUNT ON;    
    
SELECT DISTINCT
T_AUEC.AUECID,RegularTradingStartTime,RegularTradingEndTime 
from T_CompanyAUEC join
T_AUEC on T_AUEC.AUECID = T_CompanyAUEC.AUECID

END 



