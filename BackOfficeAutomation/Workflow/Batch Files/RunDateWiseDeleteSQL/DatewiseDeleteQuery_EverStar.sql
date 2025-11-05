USE [Everstar]

BEGIN TRAN TRAN1   

Declare @ErrorMessage varchar(500)                                                               
Declare @ErrorNumber int   
Declare @Date datetime

Set @Date = CONVERT(VARCHAR(10), CONVERT(DATETIME, CONVERT(VARCHAR(10), GETDATE(), 23), 102), 23)
                                                                                                                                 
SELECT GroupID 
Into T_TempGroupIDTable 
From T_Group WHERE DateDiff(Day,AUECLOcalDate,@Date) > 0

-- Selecting CLORDERIDs into temp table based on GroupIDs to DELETE data from Fill level tables.
SELECT ClOrderID into T_TempCLOrderIDs 
FROM T_SUB
Where DateDiff(Day,AUECLOcalDate,@Date) > 0

Select ParentClOrderID into T_TempParentCLOrderIDs
from T_Sub 
Where ClOrderID In (SELECT ClOrderID FROM T_TempCLOrderIDs)

-----------Deletion takes place here (Fill level).-----------------------------

--DELETE FROM T_Fills 
--WHERE ClOrderID in(SELECT ClOrderID FROM T_TempCLOrderIDs)

--DELETE FROM T_Sub
--WHERE ParentClOrderID in(SELECT ParentClOrderID FROM T_TempParentCLOrderIDs)

--DELETE FROM T_Order 
--WHERE ParentClOrderID In (SELECT ParentClOrderID FROM T_TempParentCLOrderIDs)

--DELETE FROM T_TradedOrders 
--WHERE ClOrderID In (SELECT ClOrderID FROM T_TempCLOrderIDs)

-----------Deletion takes place here (Group onwards).-----------------------------

DELETE FROM T_Level2Allocation
WHERE GroupID in(SELECT GroupID from T_TempGroupIDTable)

DELETE FROM T_FundAllocation
WHERE GroupID in(SELECT GroupID from T_TempGroupIDTable)

DELETE FROM T_Journal
WHERE TaxLotID in(SELECT taxlotID FROM PM_Taxlots WHERE GroupID in(SELECT GroupID FROM T_TempGroupIDTable))

DELETE FROM T_AllActivity
WHERE FKID in(SELECT taxlotID FROM PM_Taxlots WHERE GroupID in(SELECT GroupID FROM T_TempGroupIDTable))

DELETE FROM PM_TaxlotClosing
WHERE TaxLotClosingID In (SELECT TaxLotClosingId_Fk FROM PM_Taxlots WHERE GroupID in(SELECT GroupID FROM T_TempGroupIDTable))

DELETE FROM PM_Taxlots
WHERE GroupID In (SELECT GroupID FROM T_TempGroupIDTable)

DELETE FROM T_PBWiseTaxlotState
WHERE TaxLotID Not In (SELECT TaxLotID FROM PM_Taxlots)

DELETE FROM T_Group
WHERE GroupID In (SELECT GroupID FROM T_TempGroupIDTable)

DELETE PM_DayMarkPrice 
WHERE DateDiff(Day,Date,@Date) > 0

DELETE T_CurrencyConversionRate 
WHERE DateDiff(Day,Date,@Date) > 0

	DELETE T_DeletedTaxLots
	DELETE T_SubAccountBalances
	DELETE T_LastCalculatedBalanceDate	
	DELETE PM_Taxlots_DeletedAudit
	DELETE T_Group_DeletedAudit
	DELETE T_TradeAudit
	DELETE T_AdminAuditTrail

	DELETE PM_NAVValue
	DELETE PM_CompanyFundCashCurrencyValue
	DELETE T_CashTransactions
	
	DELETE PM_DailyOutStandings
	DELETE T_PMDataDump	
	DELETE PM_CorpActionTaxlots
	

	IF OBJECT_ID (N'T_SubAccountCashValue', N'U') IS NOT NULL 
	BEGIN
		DELETE T_SubAccountCashValue
	END 
---------------------  Drop Table -----------------------------------------
DROP TABLE T_TempCLOrderIDs,T_TempGroupIDTable, T_TempParentCLOrderIDs

COMMIT TRANSACTION TRAN1