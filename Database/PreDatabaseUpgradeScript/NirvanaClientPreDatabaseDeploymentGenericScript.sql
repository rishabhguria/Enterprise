----------------------------
--SSDT Pre-Publishing Script 
----------------------------

----------------------------------------------------
--Clear unwanted stats from Nirvana Client Database
----------------------------------------------------

GO

DECLARE @sql varchar(1000);
DECLARE @tableName sysname;
DECLARE @statName sysname;
DECLARE statCursor CURSOR FOR
    SELECT OBJECT_NAME(s.[object_id]) AS TableName,
           s.[name] AS StatName
      FROM sys.stats s
     WHERE OBJECTPROPERTY(s.OBJECT_ID,'IsUserTable') = 1
           AND s.name LIKE '_dta_stat%';
 
OPEN statCursor;
FETCH statCursor INTO @tableName, @statName;
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @sql = 'DROP STATISTICS [' + @tableName + '].[' + @statName + ']';
    PRINT @sql;
    EXEC (@sql);  --Execute drop
      FETCH statCursor INTO @tableName, @statName;
END
 
CLOSE statCursor;
DEALLOCATE statCursor;

-------------------------------------------------
--Clear T_CashPreferences_Backup
-------------------------------------------------

GO
IF exists(select name FROM sys.tables where Name = N'T_CashPreferences_Backup')
begin
	Drop table T_CashPreferences_Backup;
PRINT 'Dropped Table T_CashPreferences_Backup'
end

--------------------------------------------------------------------------------------------------
--Deleting table PM_datadump as it is dynamic table and data is filled at runtime from application
--------------------------------------------------------------------------------------------------

Drop Table T_PMDataDump

--------------------------------------------------------------
--Deleting duplicate entries from activity and journal tables
--------------------------------------------------------------

Select * into #Dup_Activities from T_allactivity
Where ActivityID in
(Select ActivityID from T_allactivity 
group by activityid
having count(*)>1)

Select * into #Dup_Activities_ordered from #Dup_Activities

Alter table #Dup_Activities_ordered
add ID int identity(1,1)

Delete from #Dup_Activities_ordered where ID%2=0

Update T_Allactivity
SET ActivityID = Dup_Act.ID
From T_Allactivity Act
INNER JOIN #Dup_Activities_ordered Dup_Act
ON Act.ActivityID = Dup_Act.ActivityID
AND Act.TransactionSource = Dup_Act.TransactionSource
AND Act.FundID = Dup_Act.FundID
AND Act.Symbol = Dup_Act.Symbol
AND Act.TradeDate = Dup_Act.TradeDate
AND Act.Amount = Dup_Act.Amount

Update T_Journal
SET ActivityID_FK = Dup_Act.ID
From T_Journal J
INNER JOIN #Dup_Activities_ordered Dup_Act
ON J.ActivityID_FK = Dup_Act.ActivityID
AND J.TransactionSource = Dup_Act.TransactionSource
AND J.FundID = Dup_Act.FundID
AND J.Symbol = Dup_Act.Symbol
AND J.TransactionDate = Dup_Act.TradeDate

Drop table #Dup_Activities
Drop TABLE #Dup_Activities_ordered


--------------------------------
--Celadon database schema issue
--------------------------------


GO
delete from T_DeletedTaxLots;

GO
update T_Group set commission=0 where commission is null;

GO
update T_Group set OtherBrokerFees=0 where OtherBrokerFees is null;

GO
update T_Group set StampDuty=0 where StampDuty is null;

GO
update T_Group set TransactionLevy=0 where TransactionLevy is null;

GO
update T_Group set ClearingFee=0 where ClearingFee is null;

GO
update T_Group set TaxOnCommissions=0 where TaxOnCommissions is null;

GO
update T_Group set MiscFees=0 where MiscFees is null;

----------------------------------------------
--Remove corrupted Audit data from the tables
----------------------------------------------
GO
delete from PM_Taxlots_DeletedAudit where AuditID not in ( select auditid from T_TradeAudit);

GO
delete from T_Group_DeletedAudit where AuditID not in ( select auditid from T_TradeAudit);

GO
delete from T_SwapParameters_DeletedAudit where AuditID not in ( select auditid from T_TradeAudit);

/*
Error: The ALTER TABLE statement conflicted with the FOREIGN KEY constraint "FK_T_Journal_T_CompanyFunds". The conflict occurred in database "CEFPilotV1.10", table "dbo.T_CompanyFunds", column 'CompanyFundID'.
Error while publish CEFPilotV1.10

Reason: It occurred because while publish SSDT was tring to create a foreign key from T_CompanyFunds.CompanyFundID to T_Journal.FundID but/and the values
in T_CompanyFunds.CompanyFundID didn't match with any of the values in T_Journal.FundID. 
You cannot create a relation which violates referential integrity.

https://jira.nirvanasolutions.com:8443/browse/PRANA-19996

-Ankit Misra
*/
Delete from T_Journal where FundID NOT IN (Select CompanyFundID from T_CompanyFunds)

Delete from T_AllActivity where FundID NOT IN (Select CompanyFundID from T_CompanyFunds)