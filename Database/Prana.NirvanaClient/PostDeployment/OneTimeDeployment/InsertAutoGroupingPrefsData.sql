TRUNCATE TABLE [dbo].[T_AutoGroupingPref]
TRUNCATE TABLE [dbo].[T_AutoGroupingFunds]

INSERT INTO [dbo].[T_AutoGroupingPref]
SELECT T_Company.CompanyID AS CompanyID
	,0 AS AutoGroup
	,0 AS TradeAttribute1
	,0 AS TradeAttribute2
	,0 AS TradeAttribute3
	,0 AS TradeAttribute4
	,0 AS TradeAttribute5
	,0 AS TradeAttribute6
	,0 AS [Broker]
	,0 AS Venue
	,1 AS [TradingAC]
	,1 AS TradeDate
	,0 AS ProcessDate
FROM T_Company

SELECT 0 AS FundID
INTO #temp_AutoGroupFunds
UNION ALL
SELECT T_CompanyFunds.CompanyFundID AS FundID
FROM T_CompanyFunds WHERE IsActive = 1


INSERT INTO [dbo].[T_AutoGroupingFunds] (FundID)
SELECT FundID
FROM #temp_AutoGroupFunds

DROP TABLE #temp_AutoGroupFunds
