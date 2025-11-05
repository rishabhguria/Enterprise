--Update T_Group table set column PSTAllocationPreferenceID or OriginalAllocationPreferenceID value null if it is 0
DECLARE @UpdateT_Group nvarchar(MAX)
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.columns where TABLE_NAME = 'T_Group'
                AND COLUMN_NAME = 'PSTAllocationPreferenceID')
BEGIN
	ALTER TABLE T_Group
	ALTER COLUMN PSTAllocationPreferenceID INT NULL
	SET @UpdateT_Group='UPDATE T_Group SET PSTAllocationPreferenceID = NULLIF(PSTAllocationPreferenceID,0)'
	EXEC (@UpdateT_Group)
END
ELSE IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.columns where TABLE_NAME = 'T_Group'
                AND COLUMN_NAME = 'OriginalAllocationPreferenceID')
BEGIN
	ALTER TABLE T_Group
	ALTER COLUMN OriginalAllocationPreferenceID INT NULL
	SET @UpdateT_Group='UPDATE T_Group SET OriginalAllocationPreferenceID = NULLIF(OriginalAllocationPreferenceID,0)'
	EXEC (@UpdateT_Group)
END

--Update T_Sub table set column PSTAllocationPreferenceID or OriginalAllocationPreferenceID value null if it is 0
DECLARE @UpdateT_Sub nvarchar(MAX)
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.columns where TABLE_NAME = 'T_Sub'
                AND COLUMN_NAME = 'PSTAllocationPreferenceID')
BEGIN
	ALTER TABLE T_Sub
	ALTER COLUMN PSTAllocationPreferenceID INT NULL
	SET @UpdateT_Sub='UPDATE T_Sub SET PSTAllocationPreferenceID = NULLIF(PSTAllocationPreferenceID,0)'
	EXEC (@UpdateT_Sub)
END
ELSE IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.columns where TABLE_NAME = 'T_Sub'
                AND COLUMN_NAME = 'OriginalAllocationPreferenceID')
BEGIN
	ALTER TABLE T_Sub
	ALTER COLUMN OriginalAllocationPreferenceID INT NULL
	SET @UpdateT_Sub='UPDATE T_Sub SET OriginalAllocationPreferenceID = NULLIF(OriginalAllocationPreferenceID,0)'
	EXEC (@UpdateT_Sub)
END

--Update T_TradedOrders table set column PSTAllocationPreferenceID or OriginalAllocationPreferenceID value null if it is 0
DECLARE @UpdateT_TradedOrders nvarchar(MAX)
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.columns where TABLE_NAME = 'T_TradedOrders'
                AND COLUMN_NAME = 'PSTAllocationPreferenceID')
BEGIN
	ALTER TABLE T_TradedOrders
	ALTER COLUMN PSTAllocationPreferenceID INT NULL
	SET @UpdateT_TradedOrders='UPDATE T_TradedOrders SET PSTAllocationPreferenceID = NULLIF(PSTAllocationPreferenceID,0)'
	EXEC (@UpdateT_TradedOrders)
END
ELSE IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.columns where TABLE_NAME = 'T_TradedOrders'
                AND COLUMN_NAME = 'OriginalAllocationPreferenceID')
BEGIN
	ALTER TABLE T_TradedOrders
	ALTER COLUMN OriginalAllocationPreferenceID INT NULL
	SET @UpdateT_TradedOrders='UPDATE T_TradedOrders SET OriginalAllocationPreferenceID = NULLIF(OriginalAllocationPreferenceID,0)'
	EXEC (@UpdateT_TradedOrders)
END

