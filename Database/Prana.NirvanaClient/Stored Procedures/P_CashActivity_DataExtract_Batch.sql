CREATE PROCEDURE P_CashActivity_DataExtract_Batch 
 @fromDate DATETIME
,@toDate DATETIME
AS

SET NOCOUNT On  

BEGIN

CREATE TABLE #Journal (
Symbol varchar(50),
Account varchar(50),
Currency varchar(50),
TransactionSource varchar(50),
DR float,
CR float
)

INSERT INTO #Journal
SELECT J.Symbol,TCF.FundName AS AccountName,TC.CurrencyName AS Currency,

CASE

	WHEN J.TransactionSource=1

		THEN 'Trading'

	WHEN J.TransactionSource=2

		THEN 'ManualJournalEntry'

	WHEN J.TransactionSource=3

		THEN 'DailyCalculation'

	WHEN J.TransactionSource=4

		THEN 'CorpAction'

	WHEN J.TransactionSource=5

		THEN 'CashTransaction'

	WHEN J.TransactionSource=6

		THEN 'ImportedEditableData'

	WHEN J.TransactionSource=7

		THEN 'Closing'

	WHEN J.TransactionSource=8

		THEN 'OpeningBalance'

	WHEN J.TransactionSource=9

		THEN 'Revaluation'

	WHEN J.TransactionSource=10

		THEN 'Unwinding'

	WHEN J.TransactionSource=11

		THEN 'SettlementTransaction'

	WHEN J.TransactionSource=12

		THEN 'TradeImport'

END AS TransactionSource,

J.DR,
J.CR

FROM T_Journal J
INNER JOIN T_SubAccounts S ON J.SubAccountID = S.SubAccountID
INNER JOIN T_TransactionType T ON S.TransactionTypeID = T.TransactionTypeID
INNER JOIN T_Currency TC ON J.CurrencyID = TC.CurrencyID
INNER JOIN T_CompanyFunds TCF ON J.FundID = TCF.CompanyFundID
WHERE DateDiff(DD, TransactionDate, @fromDate) <= 0
AND datediff(DD, TransactionDate, @toDate) >= 0
AND T.TransactionType = 'Cash'
ORDER BY TransactionID

SELECT * FROM #Journal

DROP TABLE #Journal
END