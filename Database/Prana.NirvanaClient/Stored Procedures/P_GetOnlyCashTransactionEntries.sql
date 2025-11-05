-----------------------------------------------------------------  
--Created By: Nishant Jain  
--Date: 2018-06-01
--Purpose: Get the Cash account type entries  
-----------------------------------------------------------------  
CREATE PROCEDURE P_GetOnlyCashTransactionEntries @fundIDs VARCHAR(max)
	,@fromDate DATETIME
	,@toDate DATETIME
AS
BEGIN
	CREATE TABLE #Funds (FundID INT)

	INSERT INTO #Funds
	SELECT Items AS FundID
	FROM dbo.Split(@fundIDs, ',')

	SELECT j.*
	FROM T_Journal J
	INNER JOIN #Funds F ON J.FundId = F.FundID
	INNER JOIN T_SubAccounts S ON J.SubAccountID = S.SubAccountID
	INNER JOIN T_TransactionType T ON S.TransactionTypeID = T.TransactionTypeID	
	WHERE DateDiff(DD, TransactionDate, @fromDate) <= 0
		AND datediff(DD, TransactionDate, @toDate) >= 0		
		AND T.TransactionType = 'Cash'
	ORDER BY TransactionID
END
