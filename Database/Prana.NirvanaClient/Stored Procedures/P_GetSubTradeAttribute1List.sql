 create PROCEDURE [dbo].[P_GetSubTradeAttribute1List]                                                
AS
BEGIN
	select distinct TradeAttribute1 from T_Sub WITH (NOLOCK) where TradeAttribute1 is not null and TradeAttribute1 != ''
END    
  

