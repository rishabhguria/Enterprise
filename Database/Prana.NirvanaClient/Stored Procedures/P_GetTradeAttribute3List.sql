CREATE PROCEDURE [dbo].[P_GetTradeAttribute3List]                                                
AS
BEGIN
	select distinct TradeAttribute3 from T_Group G WITH (NOLOCK)
END


