CREATE PROCEDURE [dbo].[P_GetCashDividendFromActivities]    
(  
   @AUECDateString VARCHAR(MAX)  
)       
AS
CREATE TABLE #AUECDatesTable (AUECID int,CurrentAUECDate DateTime)
INSERT INTO #AUECDatesTable Select * From dbo.GetAllAUECDatesFromString(@AUECDateString)

SELECT CT.TaxlotId, CT.Symbol, CT.Amount, CT.CurrencyId, CT.ExDate, CT.FXRate,CT.FXConversionMethodOperator, CT.CashTransactionId FROM T_CashTransactions CT
INNER JOIN T_AllActivity AA ON AA.FKID = cast(CT.CashTransactionId  as varchar(max))
INNER JOIN T_ActivityType AT ON AT.ActivityTypeId = AA.ActivityTypeId_FK
INNER JOIN V_Taxlots VT on CT.TaxlotId = VT.TaxlotId
INNER JOIN #AUECDatesTable AUECDates on AUECDates.AUECId = VT.AUECID
WHERE AT.Acronym IN ('DividendIncome','DividendExpense','WithholdingTax') AND DATEDIFF(d,AUECDates.CurrentAUECDate, CT.ExDate) = 0

