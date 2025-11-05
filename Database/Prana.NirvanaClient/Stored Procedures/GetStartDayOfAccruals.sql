CREATE PROCEDURE [dbo].[GetStartDayOfAccruals] (@DateForCashValues DATETIME,@IsIncludeTradingDayAccruals BIT=1)  
AS  
BEGIN  
  --Day End Accruals
  SELECT FundID ,CurrencyID,cast(cast(SUM(CloseDrBal - CloseCrBal) AS float) as DECIMAL(28, 12)) AS Cash    
  FROM T_SubAccountBalances WITH(NOLOCK)
  INNER JOIN T_SubAccounts WITH(NOLOCK) ON T_SubAccountBalances.SubAccountID = T_SubAccounts.SubAccountID  
  INNER JOIN T_TransactionType WITH(NOLOCK) ON T_TransactionType.TransactionTypeId = T_SubAccounts.TransactionTypeId  
  WHERE T_TransactionType.TransactionType = 'Accrued Balance'  
  AND DATEDIFF(dd, T_SubAccountBalances.TransactionDate, DATEADD(dd, -1, @DateForCashValues)) = 0  
  GROUP BY FundID ,CurrencyID

  if(@IsIncludeTradingDayAccruals=0)
  begin
  --Today's Accruals
  SELECT T_Journal.FundID, CurrencyID,cast(cast(SUM((DR-CR) ) AS float) as DECIMAL(28, 12)) AS Cash 
  FROM T_Journal WITH(NOLOCK)
  INNER JOIN T_SubAccounts WITH(NOLOCK) ON T_Journal.SubAccountID = T_SubAccounts.SubAccountID  
  INNER JOIN T_TransactionType WITH(NOLOCK) ON T_TransactionType.TransactionTypeId = T_SubAccounts.TransactionTypeId  
  WHERE T_TransactionType.TransactionType = 'Accrued Balance' AND DATEDIFF(dd, T_Journal.TransactionDate, @DateForCashValues) = 0  
and TransactionSource <> 1 
  GROUP BY T_Journal.FundID  ,CurrencyID
  end 
  else 
  begin
    --Today's Accruals
  SELECT T_Journal.FundID, CurrencyID,cast(cast(SUM((DR-CR) ) AS float) as DECIMAL(28, 12)) AS Cash 
  FROM T_Journal WITH(NOLOCK)
  INNER JOIN T_SubAccounts WITH(NOLOCK) ON T_Journal.SubAccountID = T_SubAccounts.SubAccountID  
  INNER JOIN T_TransactionType WITH(NOLOCK) ON T_TransactionType.TransactionTypeId = T_SubAccounts.TransactionTypeId  
  WHERE T_TransactionType.TransactionType = 'Accrued Balance' AND DATEDIFF(dd, T_Journal.TransactionDate, @DateForCashValues) = 0  
  GROUP BY T_Journal.FundID  ,CurrencyID
  end

END