Declare @errormsg varchar(max)

Set @errormsg=''

Create table   #temp (tableName varchar(50),RawData int)

IF  EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_MW_Transactions')
BEGIN
Insert into #temp (tableName,RawData) select 'T_MW_Transactions' as tableName ,Count(*) as RawData from  T_MW_Transactions where fund not in (select fundname from T_companyfunds)
End

IF  EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_NT_Transactions')
BEGIN
Insert into #temp (tableName,RawData) select 'T_NT_Transactions' as tableName ,Count(*) as RawData  from  T_NT_Transactions where acctname not in (select AcctName  from T_companyfunds)
End
IF  EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_MW_GenericPNL')
BEGIN
Insert into #temp (tableName,RawData) select 'T_MW_GenericPNL' as tableName ,Count(*) as RawData  from  T_MW_GenericPNL where fund not in (select fundname from T_companyfunds)
End
IF  EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_NT_GenericPNL')
BEGIN
Insert into #temp (tableName,RawData) select 'T_NT_GenericPNL' as tableName ,Count(*) as RawData  from  T_NT_GenericPNL where acctname not in (select AcctName from T_companyfunds)
End

IF  EXISTS( select sum(RawData) from #temp HAVING sum(RawData)>0)
BEGIN

SET @errormsg='Need to clean raw data, Please contact to Tech-Team for this!'
SELECT * FROM #temp

END

SELECT @errormsg AS ErrorMsg
Drop table #temp

