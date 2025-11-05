CREATE PROCEDURE [dbo].[P_GetTradeAttribute1List]                                                
AS
BEGIN
	select distinct TradeAttribute1 from T_Group G WITH (NOLOCK)
END

