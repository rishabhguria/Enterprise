CREATE PROCEDURE [dbo].[P_GetSubTradeAttribute3List]                                                
AS
BEGIN
	select distinct TradeAttribute3 from T_Sub WITH (NOLOCK) where TradeAttribute3 is not null and TradeAttribute3 != ''
END   
  

