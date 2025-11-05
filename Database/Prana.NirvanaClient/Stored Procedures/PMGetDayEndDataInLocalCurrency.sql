    
CREATE PROCEDURE [dbo].[PMGetDayEndDataInLocalCurrency] (      
@DateForCashValues     DATETIME,      
@IsAccrualsNeeded      BIT)      
AS      
 BEGIN      
  DECLARE @Local_DateForCashValues DATETIME      
  DECLARE @Local_IsAccrualsNeeded BIT      
      
  SET @Local_DateForCashValues = @DateForCashValues      
  SET @Local_IsAccrualsNeeded = @IsAccrualsNeeded      
      
  SELECT FundID, SUM(CashValueBase) AS Cash FROM PM_CompanyFundCashCurrencyValue      
  WHERE DATEDIFF(dd, Date, @Local_DateForCashValues) = 0  
  GROUP BY FundID  
      
  IF @Local_IsAccrualsNeeded = 1      
   BEGIN  
-- SELECT FundID, SUM(CloseDrBalBase - CloseCrBalBase) AS Cash FROM T_SubAccountBalances      
--    INNER JOIN T_SubAccounts ON T_SubAccountBalances.SubAccountID = T_SubAccounts.SubAccountID      
--    INNER JOIN T_TransactionType ON T_TransactionType.TransactionTypeId = T_SubAccounts.TransactionTypeId      
--    WHERE  T_TransactionType.TransactionType = 'Accrued Balance' AND  
-- DATEDIFF(dd, T_SubAccountBalances.TransactionDate,@Local_DateForCashValues) = 0  
--    GROUP BY FundID  
  
  SELECT FundID, SUM(CAST(Amount as float)) AS Cash, CurrencyID FROM T_AllActivity    
  WHERE  BalanceType = 2 AND  
  DATEDIFF(d, TradeDate, @Local_DateForCashValues) >= 0 and (DATEDIFF(d, settlementDate, @Local_DateForCashValues) < 0 OR settlementDate IS NULL)
  GROUP BY FundID, CurrencyID
   END      
 END   
