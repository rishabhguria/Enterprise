CREATE PROCEDURE [dbo].[P_GetTradeAttribute2List]                                                
AS
BEGIN	
	select distinct TradeAttribute2 from T_Group G WITH (NOLOCK)	
END


