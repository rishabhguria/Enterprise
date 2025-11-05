CREATE PROCEDURE [dbo].[P_GetTradeAttribute6List]                                                
AS
BEGIN	
	select distinct TradeAttribute6 from T_Group G WITH (NOLOCK)
END


