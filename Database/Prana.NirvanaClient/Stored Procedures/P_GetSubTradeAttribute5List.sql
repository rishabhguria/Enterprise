CREATE PROCEDURE [dbo].[P_GetSubTradeAttribute5List]                                                
AS
BEGIN
	select distinct TradeAttribute5 from T_Sub WITH (NOLOCK) where TradeAttribute5 is not null and TradeAttribute5 != ''
END   
  

