CREATE PROCEDURE [dbo].[P_GetSubTradeAttribute2List]                                                
AS
BEGIN
	select distinct TradeAttribute2 from T_Sub WITH (NOLOCK) where TradeAttribute2 is not null and TradeAttribute2 != ''
END   
  

