CREATE PROCEDURE [dbo].[P_GetTradeAttribute5List]                                                
AS
BEGIN
	select distinct TradeAttribute5 from T_Group G WITH (NOLOCK)	
END



