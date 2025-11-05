CREATE PROCEDURE P_ChangedFundBeforeSymbolWiseAcrrualDate @FundIds varchar(100),@startDate DateTime,@endDate DateTime
AS
BEGIN

CREATE TABLE #Funds (FundID INT)

	INSERT INTO #Funds
	SELECT Items AS FundID
	FROM dbo.Split(@FundIDs, ',')

SELECT DISTINCT J.FundID,TCF.FundName FROM T_LastCalculatedBalanceDate J
INNER JOIN #Funds F ON F.FundId=J.FundID
INNER JOIN T_SubAccounts SubAccounts ON SubAccounts.SubAccountID = J.SubAcID
INNER JOIN T_TransactionType TransType ON SubAccounts.TransactionTypeId = TransType.TransactionTypeId
INNER JOIN T_CashPreferences TCP ON TCP.FundID=J.FundID
INNER JOIN T_CompanyFunds TCF ON TCF.CompanyFundID=F.FundID
WHERE TransType.TransactionType = 'Accrued Balance' AND 
TCP.SymbolWiseRevaluationDate IS NOT NULL
AND DATEDIFF(d,TCP.SymbolWiseRevaluationDate,J.LastCalcDate)<=0

drop table #Funds
END
