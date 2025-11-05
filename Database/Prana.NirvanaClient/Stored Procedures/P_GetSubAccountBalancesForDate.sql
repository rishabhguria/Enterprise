/*    
Author  : Rajat tandon    
Date  : 31 Jan 2012    
Description : It fetches the Subaccount balance info for the supplied date. First it initiates the calculation of balances and then fetch the data.    
Usage  : P_GetSubAccountBalancesForDate '2012-01-26'    
*/
CREATE PROCEDURE [dbo].[P_GetSubAccountBalancesForDate] (
	@date DATETIME,
	@fundIDs VARCHAR(max)
	)
AS
CREATE TABLE #funds (fundID INT)
INSERT INTO #funds
SELECT Items
FROM dbo.Split(@fundIDs, ',')
--select * from #funds
SELECT SubBal.SubAccountID,
	SubAcc.NAME AS SubAccName,
	SubCat.SubCategoryID,
	SubCat.SubCategoryName,
	MastCat.MasterCategoryID,
	MastCat.MasterCategoryName,
	SubAcc.TransactionTypeId,
	AccType.TransactionType,
	SubBal.FundId,
	Funds.FundShortName AS FundName,
	SubBal.CurrencyId,
	Curr.CurrencySymbol,
	SubBal.OpenBalDate,
	SubBal.TransactionDate,
	SubBal.OpenDrBal,
	SubBal.OpenCrBal,
	SubBal.DayDr,
	SubBal.DayCr,
	SubBal.CloseDrBal,
	SubBal.CloseCrBal,
	SubBal.OpenDrBalBase,
	SubBal.OpenCrBalBase,
	SubBal.DayDrBase,
	SubBal.DayCrBase,
	SubBal.CloseDrBalBase,
	SubBal.CloseCrBalBase,
	Curr1.CurrencySymbol AS BaseCurrency
FROM T_SubAccountBalances SubBal WITH(NOLOCK)
INNER JOIN T_SubAccounts SubAcc WITH(NOLOCK) ON SubBal.SubAccountID = SubAcc.SubAccountID
INNER JOIN T_CompanyFunds Funds WITH(NOLOCK) ON SubBal.FundID = Funds.CompanyFundID
INNER JOIN T_Currency Curr1 WITH(NOLOCK) ON Funds.LocalCurrency = Curr1.CurrencyID
INNER JOIN T_Currency Curr WITH(NOLOCK) ON SubBal.CurrencyID = Curr.CurrencyID
INNER JOIN T_SubCategory SubCat WITH(NOLOCK) ON SubAcc.SubCategoryID = SubCat.SubCategoryID
INNER JOIN T_MasterCategory MastCat WITH(NOLOCK) ON SubCat.MasterCategoryID = MastCat.MasterCategoryID
INNER JOIN T_TransactionType AccType WITH(NOLOCK) ON SubAcc.TransactionTypeId = AccType.TransactionTypeId
INNER JOIN T_CashPreferences tcpref WITH(NOLOCK) ON SubBal.FundID = tcpref.FundID
WHERE DateDiff(d, TransactionDate, @date) = 0
	AND DATEDIFF(d, SubBal.TransactionDate, tcpref.CashMgmtStartDate) <= 0
	AND SubBal.FundID IN (
		SELECT FundID
		FROM #funds
		)
DROP TABLE #funds
