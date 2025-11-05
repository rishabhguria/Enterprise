CREATE PROCEDURE [dbo].[P_GetSubTradeAttribute6List]                                                
AS
BEGIN
	select distinct TradeAttribute6 from T_Sub WITH (NOLOCK) where TradeAttribute6 is not null and TradeAttribute6 != ''
END   
  

