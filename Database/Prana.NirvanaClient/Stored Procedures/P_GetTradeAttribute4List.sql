CREATE PROCEDURE [dbo].[P_GetTradeAttribute4List]                                                
AS
BEGIN	
	select distinct TradeAttribute4 from T_Group G WITH (NOLOCK)
END


