  
  
-- =============================================  
-- Author:  <Harsh Kumar>  
-- Create date: <03/03/2008>  
-- Description: <Picks AUEC settlement days from T_AUEC -> main AUEC form on Admin>  
-- =============================================  
CREATE PROCEDURE [dbo].[P_GetAUECSettlementPeriods] AS  
BEGIN  
select AUECID, SettlementDaysBuy,SettlementDaysSell from T_AUEC  
END  