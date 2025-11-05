CREATE PROCEDURE [dbo].[P_GetSubTradeAttribute4List]                                                
AS
BEGIN
	select distinct TradeAttribute4 from T_Sub WITH (NOLOCK) where TradeAttribute4 is not null and TradeAttribute4 != ''
END   
  

