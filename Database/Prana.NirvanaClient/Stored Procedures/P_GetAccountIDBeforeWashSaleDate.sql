CREATE PROCEDURE [dbo].[P_GetAccountIDBeforeWashSaleDate]
(@xmlPreferences NTEXT)
AS
DECLARE @handle1 INT

EXEC sp_xml_preparedocument @handle1 OUTPUT
	,@xmlPreferences

BEGIN

--Temporary table that stores the selected WashSaleStartDate for individual accounts 
	CREATE TABLE #TempPrefs (
		colFundID INT
		,colWashSaleStartDate DATETIME
		)
	INSERT INTO #TempPrefs (
		colFundID
		,colWashSaleStartDate		
		)
	SELECT AccountID
		,WashSaleStartDate		
	FROM openXML(@handle1, 'DsPref/DtPref', 2) WITH (
			AccountID INT
			,WashSaleStartDate DATETIME
			);

--Select the accounts that have trade dates on or before WashSaleStartDate
	WITH AccountDetails(AccountName, AccountID, FirstTradeDate) AS
	(
	Select Account, CompanyFundID, MIN(TradeDate) TradeDate
	from T_WashSaleOnBoarding ws
	INNER JOIN T_CompanyFunds cf 
	ON ws.Account = cf.FundName
	GROUP BY WS.Account, cf.CompanyFundID
	)
	SELECT AccountName, colWashSaleStartDate
	FROM AccountDetails AS AD
	INNER JOIN #TempPrefs
	ON AD.AccountID = #TempPrefs.colFundID
	WHERE AD.FirstTradeDate <= #TempPrefs.colWashSaleStartDate

	DROP TABLE #TempPrefs

	EXEC sp_xml_removedocument @handle1

END