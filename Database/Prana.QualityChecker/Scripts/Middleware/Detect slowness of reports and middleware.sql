
Create Table #Indexes
(
TableName varchar(50),
IndexName varchar(max),
IndexType varchar(200),
NoOfRecordsInTable int,
UnusedMaterial int 
) 

Declare @TableCount int
declare @FromDate datetime
declare @ToDate datetime
Declare @errormsg varchar(max)

set @FromDate=''
set @ToDate=''
set @errormsg=''
Set @TableCount= (Select Count(*) from PM_Taxlots)



Insert into #Indexes
Select 
'PM_Taxlots',
name,
type_desc,
@TableCount,
0 
from sys.indexes
where object_id = (select object_id from sys.objects where name = 'PM_Taxlots')

Set @TableCount= (Select Count(*) from T_MW_GenericPNL)
Declare @UnusedDataIn int 
Select @UnusedDataIn=Count(*) from T_MW_genericpnl where fund not in (select fundname from T_companyfunds)

Insert into #Indexes
Select 
'T_MW_GenericPnl',
name,
type_desc,
@TableCount,
@UnusedDataIn 
from sys.indexes
where object_id = (select object_id from sys.objects where name = 'T_MW_GenericPnl')

Set @TableCount= (Select Count(*) from T_MW_transactions)
Select @UnusedDataIn=Count(*) from T_MW_transactions where fund not in (select fundname from T_companyfunds)

Insert into #Indexes
Select 
'T_MW_Transactions',
name,
type_desc,
@TableCount,
@UnusedDataIn 
from sys.indexes
where object_id = (select object_id from sys.objects where name = 'T_MW_Transactions')

Set @TableCount= (Select Count(*) from T_Group)

Insert into #Indexes
Select 
'T_Group',
name,
type_desc,
@TableCount,
0 
from sys.indexes
where object_id = (select object_id from sys.objects where name = 'T_Group')

Set @TableCount= (Select Count(*) from PM_TaxlotClosing)

Insert into #Indexes
Select 
'PM_TaxlotClosing',
name,
type_desc,
@TableCount,
0 
from sys.indexes
where object_id = (select object_id from sys.objects where name = 'PM_TaxlotClosing')



IF EXISTS (Select * from #Indexes)
BEGIN
set @errormsg ='Please find Indexes which are created!'
Select * from #Indexes
END

select @errormsg as ErrorMsg

---Select * from #Indexes

Drop Table #Indexes


