CREATE PROCEDURE [dbo].[P_GetAutoGroupingRulePref]
AS
	SELECT TOP 1 
			CompanyId,
			AutoGroup,
			STUFF((SELECT ',' + CAST(FundId AS varchar) FROM T_AutoGroupingFunds where AutoGroup=1 FOR XML PATH('')), 1 ,1, '') as FundList,
			TradeAttribute1,
			TradeAttribute2,
			TradeAttribute3,
			TradeAttribute4,
			TradeAttribute5, 
			TradeAttribute6,
			[Broker],
			Venue, 
			[TradingAC],
			TradeDate,
			ProcessDate FROM [T_AutoGroupingPref] WHERE CompanyId <> -1
RETURN 0
