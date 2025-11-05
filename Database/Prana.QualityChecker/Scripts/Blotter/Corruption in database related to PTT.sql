DECLARE @errormsg VARCHAR(MAX)
DECLARE @name VARCHAR(50)

set @errormsg=''

-- Two Tables as Column Name is  different in old and new Releases
Create table #PSTAllocation
(
ClorderID VARCHAR(50),
ParentClorderID VARCHAR(50),
StagedOrderID VARCHAR(50),
UnderlyingSymbol VARCHAR(100),
Quantity FLOAT,
InsertionTime DATETIME,
AuecLocalDate DATETIME,
PSTAllocationPreferenceId INT
)

Create table #OriginalAllocation
(
ClorderID VARCHAR(50),
ParentClorderID VARCHAR(50),
StagedOrderID VARCHAR(50),
UnderlyingSymbol VARCHAR(100),
Quantity FLOAT,
InsertionTime DATETIME,
AuecLocalDate DATETIME,
OriginalAllocationPreferenceId INT
)

-- Setting name variable on the basis of column name which exist and then later use this variable in dynamic query
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'T_Sub' AND COLUMN_NAME = 'PSTAllocationPreferenceId')
set @name = 'PSTAllocationPreferenceId'
ELSE
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'T_Sub' AND COLUMN_NAME = 'OriginalAllocationPreferenceId')
set @name = 'OriginalAllocationPreferenceId'
ELSE
set @name ='NOTExist'

-- If Column doesn't exist
IF (@name = 'NOTExist')
BEGIN
SELECT * FROM #PSTAllocation WHERE 0 = 1
set @errormsg='PTT does not exist for this release so Please ignore this script.'
END
-- If column exist
ELSE
BEGIN
DECLARE @sqlText nvarchar(2000);

-- Dynamic Query will depend on the name of the column.
IF (@name ='PSTAllocationPreferenceId')
 BEGIN
 SELECT @sqlText = N'Insert into #PSTAllocation Select ClorderID, ParentClorderID, StagedOrderID, UnderlyingSymbol, Quantity, InsertionTime, AuecLocalDate, PSTAllocationPreferenceId from T_Sub S where s.' + @name + ' <> 0 and s.' + @name + ' <> S.fundid and s.' + @name + ' not in (Select AP.Id from T_AL_AllocationPreferenceDef AP)' 
END
  ELSE
BEGIN
 SELECT @sqlText = N'Insert into #OriginalAllocation Select ClorderID, ParentClorderID, StagedOrderID, UnderlyingSymbol, Quantity, InsertionTime, AuecLocalDate, OriginalAllocationPreferenceId from T_Sub S where s.' + @name + ' <> 0 and s.' + @name + ' <> S.fundid and s.' + @name + ' not in (Select AP.Id from T_AL_AllocationPreferenceDef AP)' 
 END

Exec (@sqlText)

IF EXISTS(SELECT * from #PSTAllocation) 
BEGIN
Select ClorderID, ParentClorderID, StagedOrderID, UnderlyingSymbol, Quantity, InsertionTime, AuecLocalDate, PSTAllocationPreferenceId from #PSTAllocation
set @errormsg='Corruption in database, PST Id is missing for following symbols in T_AL_AllocationPreferenceDef' 
END
ELSE IF EXISTS(SELECT * from #OriginalAllocation) 
BEGIN
Select ClorderID, ParentClorderID, StagedOrderID, UnderlyingSymbol, Quantity, InsertionTime, AuecLocalDate, OriginalAllocationPreferenceId from #OriginalAllocation
set @errormsg='Corruption in database, PST Id is missing for following symbols in T_AL_AllocationPreferenceDef'
END
ELSE
set @errormsg=''

END

SELECT @errormsg AS ErrorMsg 

Drop table #PSTAllocation
Drop table #OriginalAllocation

