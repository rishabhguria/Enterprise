
/****** Object:  StoredProcedure [dbo].[P_GetCashTransactionsExceptions]          
Script Date: 04/19/2013 19:23:48 ******/      
--TODO: Need to update this sp, this sp is old      
--exec P_GetCashTransactionsExceptions @symbol='', @fromDate='2013-06-01 00:00:00:000',@toDate='2014-06-30 00:00:00:000',@fundIDs=N'1270,1223,1249,1252,1254,1257,1258,1269,1271'  
CREATE PROC [dbo].[P_GetCashTransactionsExceptions]          
(          
 @symbol VARCHAR(50),        
 @fromDate DATETIME,          
 @toDate DATETIME,  
 @fundIDs VARCHAR(MAX)          
)          
AS
    
CREATE TABLE #funds  
(  
	fundID Int  
)  

INSERT INTO #funds  
SELECT Items FROM dbo.Split(@fundIDs,',') 

-- If on trade unallocation or deletion, activities and journals are not deleted somehow then on getting Exceptions, we will delete them.
SELECT * INTO #Activity FROM T_AllActivity
WHERE FKID NOT IN (SELECT TaxlotID FROM PM_Taxlots)
AND fundid IN (SELECT * FROM #funds) 
AND TransactionSource = 1

DELETE FROM T_AllActivity 
WHERE ActivityID in(SELECT ActivityID FROM #Activity)

DELETE FROM T_Journal
WHERE ActivityId_FK in (SELECT ActivityId FROM #Activity)
     
IF @symbol = ''  OR @symbol is null      
	BEGIN        
	  SELECT ca.CashTransactionId, ca.FundID, ca.TaxlotId, ca.Symbol, ca.Amount, ca.PayoutDate, ca.ExDate, ca.CurrencyID,ca.ActivityTypeId,ca.Description, ca.ModifyDate, ca.EntryDate
	  FROM T_CashTransactions ca INNER JOIN T_CashPreferences tcpref ON ca.FundID=tcpref.FundID  
	  WHERE DATEDIFF(d,ca.ExDate,@fromDate)<=0 AND DATEDIFF(d,ca.ExDate,@toDate)>=0       
	  AND CAST(ca.CashTransactionId AS VARCHAR(100)) NOT IN (SELECT DISTINCT FKId FROM T_AllActivity)  
	  AND DATEDIFF(d,ca.ExDate,tcpref.CashMgmtStartDate)<=0   
	  AND ca.FundID IN (SELECT #funds.fundID from #funds)   
	END        
ELSE        
	BEGIN        
	  SELECT ca.CashTransactionId, ca.FundID, ca.TaxlotId, ca.Symbol, ca.Amount, ca.PayoutDate, ca.ExDate, ca.CurrencyID,ca.ActivityTypeId,ca.Description, ca.ModifyDate, ca.EntryDate
	  FROM T_CashTransactions ca INNER JOIN T_CashPreferences tcpref ON ca.FundID=tcpref.FundID  
	  WHERE DATEDIFF(d,ca.ExDate,@fromDate)<=0 AND DATEDIFF(d,ca.ExDate,@toDate)>=0       
	  AND CAST(ca.CashTransactionId AS VARCHAR(100)) NOT IN (SELECT DISTINCT FKId FROM T_AllActivity) 
	  AND (ca.Symbol = @symbol)  
	  AND DATEDIFF(d,ca.ExDate,tcpref.CashMgmtStartDate)<=0      
	  AND ca.FundID in (SELECT #funds.fundID FROM #funds)   
	End      

DROP TABLE #Activity

