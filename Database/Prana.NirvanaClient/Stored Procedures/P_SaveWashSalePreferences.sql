CREATE PROCEDURE [dbo].[P_SaveWashSalePreferences](@xmlPreferences NTEXT)
AS
DECLARE @handle1 INT

EXEC sp_xml_preparedocument @handle1 OUTPUT
	,@xmlPreferences

BEGIN TRANSACTION;

BEGIN TRY

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
			)

--Update the T_WashSalePreferences table according to the selected WashSaleStartDate in #TempPrefs	
	UPDATE T_WashSalePreferences
	SET T_WashSalePreferences.WashSaleStartDate = #TempPrefs.colWashSaleStartDate		
	FROM T_WashSalePreferences
	INNER JOIN #TempPrefs ON T_WashSalePreferences.FundID = #TempPrefs.colFundID

--Insert new accounts details
	INSERT INTO T_WashSalePreferences(
		FundID,
		WashSaleStartDate
	)
	SELECT colFundID, colWashSaleStartDate
	FROM #TempPrefs
	WHERE colFundID NOT IN (
		SELECT FundID
		FROM T_WashSalePreferences
	)

--Delete the records of accounts from T_WashSaleOnBoarding that have trade dates on or before WashSaleStartDate
	DELETE FROM T_WashSaleOnBoarding 
	WHERE TaxlotID
	IN (SELECT TaxlotID
	FROM T_WashSaleOnBoarding AS ws
	INNER JOIN T_CompanyFunds AS cf
	ON ws.Account = cf.FundName
	INNER JOIN #TempPrefs
	ON cf.CompanyFundID = #TempPrefs.colFundID
	WHERE ws.TradeDate <= #TempPrefs.colWashSaleStartDate
	)	

	DROP TABLE #TempPrefs

	COMMIT TRANSACTION;

	EXEC sp_xml_removedocument @handle1

END TRY

BEGIN CATCH
	ROLLBACK TRANSACTION;
END CATCH
